using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    private Grid<Node> grid;                    // Malla de nodos
    public int x;                              // Posicion x del nodo
    public int y;                              // Posicion y del nodo

    public int gCost;                           // Coste al nodo inicial
    public int hCost;                           // Coste al nodo final
    public int fCost;                           // Suma de los dos costes

    public bool isWalkable;                     // Booleano para saber si se puede ir por el nodo
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
