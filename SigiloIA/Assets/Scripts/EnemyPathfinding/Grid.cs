using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public bool displayGridGizmos;                  // Booleano para mostrar solo el camino
    public LayerMask unwalkableMask;                // Capa de objetos por los que no se puede ir
    public Vector2 gridWorldSize;                   // Tamaño de la malla
    public float nodeRadius;                        // Tamaño de los nodos

    private Node[,] grid;                           // Malla de nodos
    private float nodeDiameter;                     // Diametro de los nodos
    private int gridSizeX, gridSizeY;               // Tamaño de la malla

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Calculamos el diametro de los nodos
        nodeDiameter = nodeRadius * 2;

        // Calculamos cuantos nodos caben en la malla
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Creamos la malla
        CreateGrid();

    }

    // @IGM ----------------------
    // Metodo para crear la malla.
    // ---------------------------
    private void CreateGrid()
    {

        // Creamos la malla
        grid = new Node[gridSizeX, gridSizeY];

        // Buscamos el la posición inicial de la malla
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2
            - Vector3.forward * gridWorldSize.y / 2;

        // Recorremos la malla
        for (int x = 0; x < gridSizeX; x++)
        {

            for (int y = 0; y < gridSizeY; y++)
            {

                // Buscamos la posicion del nodo
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius) + Vector3.up;

                // Miramos si es un nodo caminable
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // Creamos el nodo y lo añadimos a la malla
                grid[x, y] = new Node(walkable, worldPoint, x, y);

            }

        }

    }

    // @IGM --------------------------------------
    // Funcion para buscar los vecinos de un nodo.
    // -------------------------------------------
    public List<Node> GetNeighbours(Node node)
    {

        // Creamos la lista de nodos vecinos
        List<Node> neighbours = new List<Node>();

        // Recorremos la malla de nueve posiciones
        for (int x = -1; x <= 1; x++)
        {

            for (int y = -1; y <= 1; y++)
            {

                // Comprobamos si es la posicion del nodo
                if (x == 0 && y == 0)
                {

                    // Nos saltamos esta posicion
                    continue;

                }

                // Calculamos las posiciones del vecino
                int checkX = node.x + x;
                int checkY = node.y + y;

                // Comporbamos que el vecino esta dentro de la malla
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {

                    // Añadimos el vecino a la lista
                    neighbours.Add(grid[checkX, checkY]);

                }

            }

        }

        return neighbours;

    }

    // @IGM --------------------------------
    // Getter del tamaño maximo de la malla.
    // -------------------------------------
    public int MaxSize
    {

        get
        {

            return gridSizeX * gridSizeY;

        }

    }

    // @IGM ----------------------------------------------------------------------
    // Función para transformar una posicion del mundo a una posicion de la malla.
    // ---------------------------------------------------------------------------
    public Node NodeFromWorlPoint(Vector3 worldPosition)
    {

        // Buscamos el porcentaje de la posicion respecto a la malla
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Buscamos la posicion en la malla
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // Devolvemos la posición de la malla
        return grid[x, y];

    }

    // @IGM ------------------------------------------------------------
    // OnDrawGizmos draw gizmos that are also pickable and always drawn.
    // -----------------------------------------------------------------
    private void OnDrawGizmos()
    {

        // Dibujamos las dimensiones de la malla
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        // Comprobamos si se ha creado la malla
        if (grid != null && displayGridGizmos)
        {

            // Recorremos la malla
            foreach (Node node in grid)
            {

                // Comrpobamos si el nodo es caminable
                if (node.isWalkable)
                {

                    // Cambiamos el color a blanco
                    Gizmos.color = Color.white;

                }
                else
                {

                    // Cambiamos el color a rojo
                    Gizmos.color = Color.red;

                }

                // Dibujamos el nodo
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));

            }

        }

    }

}
