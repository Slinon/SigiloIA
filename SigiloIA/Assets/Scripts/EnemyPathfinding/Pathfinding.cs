using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{

    const int MOVE_STRAIGHT_COST = 10;                  // Constante de coste recto
    const int MOVE_DIAGONAL_COST = 14;                  // Constante de coste en diagonal
    
    // @IGM -----------------------------------------------
    // Patrón de clase Singleton para la clase pathfinding.
    // ----------------------------------------------------
    public static Pathfinding Instance { get; private set; }

    // @IGM ------------------------------------------------------------------
    // Metodo para crear el Singleton al principio del juego sino está creado.
    // -----------------------------------------------------------------------
    public static Pathfinding GetInstance()
    {

        // Comprobamos si existe una instancia de la clase
        if (Instance == null)
        {

            // Destruimos la clase
            Instance = new Pathfinding(10, 10);


        }

        return Instance;

    }

    private Grid<Node> grid;                            // Malla de nodos
    private List<Node> openList;                        // Lista de nodos visitables
    private List<Node> closedList;                      // Lista de nodos cerrada

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    private Pathfinding(int width, int height)
    {

        
        grid = new Grid<Node>(width, height, 10f, Vector3.zero, 
            (Grid<Node> grid, int x, int y) => new Node(grid, x, y));

        grid.GetGridObject(1, 3).isWalkable = false;
        grid.GetGridObject(2, 3).isWalkable = false;
        grid.GetGridObject(3, 3).isWalkable = false;
        grid.GetGridObject(6, 3).isWalkable = false;
        grid.GetGridObject(7, 3).isWalkable = false;
        grid.GetGridObject(8, 3).isWalkable = false;
        grid.GetGridObject(3, 6).isWalkable = false;
        grid.GetGridObject(3, 7).isWalkable = false;
        grid.GetGridObject(3, 8).isWalkable = false;
        grid.GetGridObject(7, 6).isWalkable = false;
        grid.GetGridObject(7, 7).isWalkable = false;
        grid.GetGridObject(7, 8).isWalkable = false;

    }

    // @IGM ----------------------------------------------
    // Funcion para averiguar la malla que estamos usando.
    // --------------------------------------------------- 
    public Grid<Node> GetGrid()
    {

        return grid;

    }

    // @IGM ------------------------------------------------------------
    // Funcion para encontrar el camino dadas las coordenadas del mundo.
    // -----------------------------------------------------------------
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {

        // Comprobamos si el punto está fuera de la malla
        if (endWorldPosition.x < grid.GetOriginPosition().x 
            || endWorldPosition.x > grid.GetWidth() * grid.GetCellSize()
            || endWorldPosition.z < grid.GetOriginPosition().z
            || endWorldPosition.z > grid.GetHeight() * grid.GetCellSize())
        {

            // No hay camino valido
            return null;

        }

        // Buscamos las posiciones en la malla
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        // Buscamos el camino utilizando A*
        List<Node> path = FindPath(startX, startY, endX, endY);

        // Comprobamos si hay camino valido
        if(path == null)
        {

            // No hay camino válido
            return null;

        }
        else
        {

            List<Vector3> vectorPath = new List<Vector3>();

            // Recorremos la lista de nodos del camino y recuperamos la posición en el mundo
            foreach(Node node in path)
            {

                // Añadimos la posición en el mundo del nodo
                Vector3 nodePosition = new Vector3(node.x, 0, node.y) * grid.GetCellSize()
                    + Vector3.one * grid.GetCellSize() * 0.5f;
                nodePosition.y = 1f;
                vectorPath.Add(nodePosition);

            }

            return vectorPath;

        }


    }

    // @IGM --------------------------------------------------------------
    // Función para encontrar el camino dadas las coordenadas de la malla.
    // -------------------------------------------------------------------
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {

        // Creamos el primer y ultimo nodo
        Node startNode = grid.GetGridObject(startX, startY);
        Node endNode = grid.GetGridObject(endX, endY);

        // Comprobamos que el nodo no es alcanzable
        if (!endNode.isWalkable)
        {

            // NO hay camino valido
            return null;

        }

        // Inicializamos las listas
        openList = new List<Node> { startNode };
        closedList = new List<Node>();

        // Recorremos la malla
        for(int x = 0; x < grid.GetWidth(); x++)
        {

            for (int y = 0; y < grid.GetHeight(); y++)
            {

                // Creamos un nodo por cada celda de la malla
                Node node = grid.GetGridObject(x, y);

                // Establecemos el coste g al maximo valor
                node.gCost = int.MaxValue;

                // Calculamos el coste de f
                node.CalculateFCost();

                // Establecemos el nodo anterior a null
                node.cameFromNode = null;

            }

        }

        // Establecemos los costes del nodo inicial
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Mantenmos el bucle mientras haya nodos visitables
        while(openList.Count > 0)
        {

            // Buscamos el nodo con menor coste de f de los nodos visitables
            Node currentNode = GetLowestFCostNode(openList);

            // Comprobamos si es el nodo final
            if(currentNode == endNode)
            {

                // Calculamos el camino a seguir
                return CalculatePath(endNode);

            }

            // Marcamos el nodo como visitado
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Recorremos la lista de nodos vecinos
            foreach (Node neighbourNode in GetNeighbourList(currentNode))
            {

                // Comprobamos si el nodo ya se ha visitado
                if(closedList.Contains(neighbourNode))
                {

                    // Saltamos el nodo
                    continue;

                }

                // Comprobamos si el nodo vecino no es caminable
                if (!neighbourNode.isWalkable)
                {

                    // Lo añadimos a la lista de nodos cerrados
                    closedList.Add(neighbourNode);
                    continue;

                }

                // Calculamos el coste g del nodo vecino
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost
                    (currentNode, neighbourNode);

                // Comrpobamos que el coste de g del nuevo vecino es menor que el del actual
                if(tentativeGCost < neighbourNode.gCost)
                {

                    // Convertimos el nuevo nodo vecino en el que tiene el camino correcto
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    // Comprobamos si el nodo vecinoestá dentro de los visitables
                    if (!openList.Contains(neighbourNode))
                    {

                        // Lo añadimos a la lista
                        openList.Add(neighbourNode);

                    }

                }

            }

        }

        // Nos hemos quedado sin nodos visitables
        return null;

    }

    // @IGM ------------------------------------------------
    // Función que añade una lista con los vecinos del nodo.
    // -----------------------------------------------------
    private List<Node> GetNeighbourList(Node currentNode)
    {

        List<Node> neighbourList = new List<Node>();

        // Comprobamos si hay nodo a la izquierda
        if(currentNode.x - 1 >= 0)
        {

            // Añadimos el nodo
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

            // Comrpobamos si hay nodo a la izquierda abajo
            if (currentNode.y - 1 >= 0)
            {

                // Añadimos el nodo
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

            }

            // Comrpobamos si hay nodo a la izquierda arriba
            if (currentNode.y + 1 < grid.GetHeight())
            {

                // Añadimos el nodo
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

            }

        }

        // Comprobamos si hay nodo a la derecha
        if (currentNode.x + 1 < grid.GetWidth())
        {

            // Añadimos el nodo
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            // Comrpobamos si hay nodo a la derecha abajo
            if (currentNode.y - 1 >= 0)
            {

                // Añadimos el nodo
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));

            }

            // Comrpobamos si hay nodo a la derecha arriba
            if (currentNode.y + 1 < grid.GetHeight())
            {

                // Añadimos el nodo
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));

            }

        }

        // Comprobamos si hay nodo abajo
        if (currentNode.y - 1 >= 0)
        {

            // Añadimos el nodo
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));

        }

        // Comprobamos si hay nodo arriba
        if (currentNode.y + 1 < grid.GetHeight())
        {

            // Añadimos el nodo
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        }

        return neighbourList;

    }

    // @IGM ------------------------------------------------------
    // Funcion para averiguar el nodo de una posicion de la malla.
    // -----------------------------------------------------------
    public Node GetNode(int x, int y)
    {

        return grid.GetGridObject(x, y);

    }

    // @IGM ----------------------------------
    // Funcion que calcula el camino a seguir.
    // ---------------------------------------
    private List<Node> CalculatePath(Node endNode)
    {

        List<Node> path = new List<Node>();

        // Añadimos nuestro nodo final
        path.Add(endNode);

        // Establecemos el nodo actual en el ultimo
        Node currentNode = endNode;

        // Mientras tengamos un nodo por el que hayamos venido
        while(currentNode.cameFromNode != null)
        {

            // Añadimos el nodo del que venimos a la lista
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;

        }

        // Reordenamos la lista
        path.Reverse();
        return path;

    }

    // @IGM --------------------------------------------
    // Funcion que calcula la distancia entre dos nodos.
    // -------------------------------------------------
    private int CalculateDistanceCost(Node a, Node b)
    {

        // Calculamos la distancia entre los dos nodos
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        // Aplicamos los costes
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    // @IGM --------------------------------------------------
    // Funcion para buscar el nodo con el coste de f mas bajo.
    // -------------------------------------------------------
    private Node GetLowestFCostNode(List<Node> nodeList)
    {

        // Establecemos un nodo
        Node lowestFCostNode = nodeList[0];

        // Recorremos la lista de nodos
        for(int i = 0; i< nodeList.Count; i++)
        {

            // Comprobamos si el nuevo nodo es mñas bajo que el anterior
            if(nodeList[i].fCost < lowestFCostNode.fCost)
            {

                // Actualizamos el nodo
                lowestFCostNode = nodeList[i];

            }

        }

        return lowestFCostNode;

    }

}
