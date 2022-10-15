using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public Vector3 worldPosition;               // Posicion del nodo en el mundo
    public int x;                               // Posicion x del nodo
    public int y;                               // Posicion y del nodo

    public int gCost;                           // Coste al nodo inicial
    public int hCost;                           // Coste al nodo final
    public int fCost;                           // Suma de los dos costes

    public bool isWalkable;                     // Booleano para saber si se puede ir por el nodo
    public Node cameFromNode;                   // Nodo del que venimos

    private int heapIndex;                      // Indice en el monton

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public Node(bool isWalkable, Vector3 worldPosition, int x, int y)
    {

        // Establecemos las variables del constructor.
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.x = x;
        this.y = y;

    }

    // @IGM ------------------------------
    // Metodo para calcular el coste de f.
    // -----------------------------------
    public void CalculateFCost()
    {

        // Sumamos los costes g y h
        fCost = gCost + hCost;

    }

    // @IGM -----------------------------
    // Get/Set de la interfaz del monton.
    // ----------------------------------
    public int HeapIndex
    {

        // Getter
        get
        {

            return heapIndex;

        }

        // Setter
        set
        {

            heapIndex = value;

        }

    }

    // @IGM ----------------------
    // Metodo para comparar nodos.
    // ---------------------------
    public int CompareTo(Node nodeToCompare)
    {

        // Comparamos el coste de f
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        // Comprobamos que es la misma comparacion
        if (compare == 0)
        {

            // Comparamos el coste de h
            compare = hCost.CompareTo(nodeToCompare.hCost);

        }

        // Devolvemos la comparacion negativa
        return -compare;

    }

}
