using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    const int GO_STRAIGHT = 10;         // Constante de moverse en linea recta
    const int GO_DIAGONAL = 14;         // Constante de moverse en diagonal

    private Grid grid;                  // Malla en la que se ejecuta el pathfinding

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Recuperamos la malla
        grid = GetComponent<Grid>();

    }

    // @IGM ---------------------------------------------
    // Metodo que calcula el camino mas corto a un punto.
    // --------------------------------------------------
    public void FindPath(PathRequest request, Action<PathResult> callback)
    {

        // Empezamos el contador de tiempo
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Establecemos los valores para el gestor
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // Buscamos los nodos en el grid
        Node startNode = grid.NodeFromWorlPoint(request.pathStart);
        Node endNode = grid.NodeFromWorlPoint(request.pathEnd);

        if (startNode.isWalkable && endNode.isWalkable)
        {



            // Creamos las listas de nodos
            Heap<Node> openNodes = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedNodes = new HashSet<Node>();

            // Abrimos el nodo inicial
            openNodes.Add(startNode);

            // Ejecutamos el bucle hasta que no haya nodos abiertos o hasta que encontremos el camino
            while (openNodes.Count > 0)
            {

                // Establecemos el nodo actual
                Node currentNode = openNodes.RemoveFirst();

                // Cerramos el nodo
                closedNodes.Add(currentNode);

                // Comprobamos si el nodo es el último
                if (currentNode == endNode)
                {

                    // Paramos el reloj y lo mostramos
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");

                    // Marcamos el exito en la operacion
                    pathSuccess = true;

                    // Cortamos el bucle
                    break;

                }

                // Recorremos los vecinos del nodo
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {

                    // Comprobamos si no se puede acceder al nodo o está cerrado
                    if (!neighbour.isWalkable || closedNodes.Contains(neighbour))
                    {

                        // Saltamos el nodo
                        continue;

                    }

                    // Calculamos el coste de movernos al vecino
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    // Comprobamos que el coste es menor que el coste g
                    if (newMovementCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                    {

                        // Actualizamos los costes
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.CalculateFCost();

                        // Asignamos el nodo del que procede
                        neighbour.cameFromNode = currentNode;

                        // Comrpobamos si el nodo aun no esta abierto
                        if (!openNodes.Contains(neighbour))
                        {

                            // Abrimos el nodo vecino
                            openNodes.Add(neighbour);

                        }
                        else
                        {

                            // Actualizamos el nodo
                            openNodes.UpdateItem(neighbour);

                        }

                    }

                }

            }

        }

        // Comprobamos si ha encontrado camino
        if (pathSuccess)
        {

            waypoints = RetracePath(startNode, endNode);

            // Comprobamos que el camino es lo suficientemente grande
            pathSuccess = waypoints.Length > 0;

        }

        // Le decimos al gestor que ha finalizado la peticion
        callback(new PathResult(waypoints, pathSuccess, request.callback));

    }

    // @IGM -----------------------------------------------
    // Metodo para calcular la distancia de un nodo a otro.
    // ----------------------------------------------------
    private int GetDistance(Node nodeA, Node nodeB)
    {

        // Calculamos la distancia de x y de y
        int distanceX = Mathf.Abs(nodeA.x - nodeB.x);
        int distanceY = Mathf.Abs(nodeA.y - nodeB.y);

        // Comprobamos cual de las dos distancias es menor
        if (distanceX > distanceY)
        {

            // Devolvemos la distancia de ir diagonal mas la distancia de ir recto lo que queda
            return GO_DIAGONAL * distanceY + GO_STRAIGHT * (distanceX - distanceY);

        }

        // Si x es menor que y cambiamos el calculo
        return GO_DIAGONAL * distanceX + GO_STRAIGHT * (distanceY - distanceX);

    }

    // @IGM ------------------------------------------
    // Funcion que recupera el camino del pathfinding.
    // -----------------------------------------------
    private Vector3[] RetracePath(Node startNode, Node endNode)
    {

        // Creamos la lista de nodos que formaran el camino
        List<Node> path = new List<Node>();

        // Asignamos el nodo final al actual y recorremos el camino en reversa
        Node currentNode = endNode;
        while (currentNode != startNode)
        {

            // Añadimos el nodo a la lista
            path.Add(currentNode);

            // Asignamos el siguiente nodo del camino
            currentNode = currentNode.cameFromNode;

        }

        // Simplificamos el camino
        Vector3[] waypoints = SimplifyPath(path);

        // Revertimos el camino
        Array.Reverse(waypoints);
        return waypoints;

    }

    // @IGM -------------------------------------
    // Funcion que simplifica el camino a seguir.
    // ------------------------------------------
    private Vector3[] SimplifyPath(List<Node> path)
    {

        // Creamos la lista de wayponts
        List<Vector3> waypoints = new List<Vector3>();

        // Creamos la direccion a la que van los nodos
        Vector2 directionOld = Vector2.zero;

        // Recorremos el camino
        for (int i = 1; i < path.Count; i++)
        {

            // Miramos la nueva direccion del camino
            Vector2 directionNew = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);

            // Comprobamos si la direccion ha cambiado
            if (directionNew != directionOld)
            {

                // Añadimos la posicion del nodo al camino
                waypoints.Add(path[i].worldPosition);

            }

            // Actualizamos la direccion
            directionOld = directionNew;

        }

        // Devolvemos el camino
        return waypoints.ToArray();

    }

}
