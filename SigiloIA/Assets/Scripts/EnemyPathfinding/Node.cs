using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private Grid<Node> grid;                   // Malla de nodos
    [HideInInspector]
    public int x;                              // Posicion x del nodo
    [HideInInspector]
    public int y;                              // Posicion y del nodo

    [HideInInspector]
    public int gCost;                           // Coste al nodo inicial
    [HideInInspector]
    public int hCost;                           // Coste al nodo final
    [HideInInspector]
    public int fCost;                           // Suma de los dos costes

    public bool isWalkable;                     // Booleano para saber si se puede ir por el nodo
    [HideInInspector]
    public Node cameFromNode;                   // Nodo del que venimos

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public Node(Grid<Node> grid, int x, int y)
    {

        // Establecemos las variables del constructor.
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;

    }

    // @IGM ------------------------------
    // Metodo para calcular el coste de f.
    // -----------------------------------
    public void CalculateFCost()
    {

        // Sumamos los costes g y h
        fCost = gCost + hCost;

    }

    // @IGM ----------------------------------------
    // Funcion para mostrar las coodenadas del nodo.
    // ---------------------------------------------
    public override string ToString()
    {

        return x + ", " + y;

    }

}
