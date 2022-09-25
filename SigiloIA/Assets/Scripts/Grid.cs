using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{

    // @IGM -----------------------------------------------------------------
    // Evento para saber si se ha cambiado el valor de una celda de la malla.
    // ----------------------------------------------------------------------
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;                      // Ancho de la malla
    private int height;                     // Alto de la malla
    private float cellSize;                 // Tamaño de las celdas
    private Vector3 originPosition;         // Posición de origen de la malla
    private TGridObject[,] gridArray;       // Array de la malla

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>,
        int, int, TGridObject> createGridObject)
    {

        // Establecemos las variables del contructor
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        // Instanciamos los array
        gridArray = new TGridObject[width, height];

        // Recorremos la malla
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {

            for (int y = 0; y < gridArray.GetLength(1); y++)
            {

                gridArray[x, y] = createGridObject(this, x, y);

            }

        }

        // Booleano para mostrar el grid
        bool showDebug = false;

        // Comprobammos si el odo debug está activo
        if(showDebug)
        {

            // Creamos el grid
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            // Recorremos la matriz de la malla
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {

                for (int y = 0; y < gridArray.GetLength(1); y++)
                {

                    // Cramos los textos para tener una referencia visual.
                    debugTextArray[x, y] = CreateWorldText(gridArray[x, y]?.ToString(), GetWorldPosition(x, y)
                        + new Vector3(cellSize, 0, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter,
                        TextAlignment.Center, 0);

                    // Dibujamos la líneas de la malla
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }

            }

            // Dibujamos las líneas que faltan
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            // Notificamos al evento de que se ha cambiado el valor
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {

                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();

            };

        }

    }

    // @IGM ----------------------------------------------------
    // Metodo para establecer el valor de una celda de la malla.
    // ---------------------------------------------------------
    public void SetGridObject (int x, int y, TGridObject value)
    {

        // Comprobamos que las coordenadas sean validas
        if (x >= 0 && y >= 0 && x < width && y < height)
        {

            // Actualizamos las matrices
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this,
            new OnGridObjectChangedEventArgs { x = x, y = y });

        }

    }

    // @IGM ----------------------------------------------------------------------------
    // Metodo para establecer el valor de una posicion de la malla sabiendo la posicion.
    // ---------------------------------------------------------------------------------
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {

        int x, y;

        // Convertimos la posicion del mundo a la malla
        GetXY(worldPosition, out x, out y);

        // Establecemos el valor
        SetGridObject(x, y, value);

    }

    // @IGM ------------------------------------------------------
    // Triger para avisar que se ha cambiado un valor de la malla.
    // -----------------------------------------------------------
    public void TriggerGridObjectChanged(int x, int y)
    {

        // Actualizamos mediante el evento el grid visual
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, 
            new OnGridObjectChangedEventArgs { x = x, y = y });

    }

    // @IGM ------------------------------------------------------------------
    // Metodo que convierte la posición en el mapa en la posición en la malla.
    // -----------------------------------------------------------------------
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {

        // Hacemos el cálculo para saber en que celda de la malla se encontraría el punto
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

    }

    // @IGM -------------------------------------------------
    // Funcion para recuperar la posición global de la celda.
    // ------------------------------------------------------
    public Vector3 GetWorldPosition(int x, int y)
    {

        return new Vector3(x, 0, y) * cellSize + originPosition;

    }

    // @IGM --------------------------------------
    // Funcion que crea la celda de manera visual.
    // -------------------------------------------
    private static TextMesh CreateWorldText(string text, Vector3 localPosition, int fontSize, Color color,
        TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {

        // Creamos el gameObject y le ponemos los atributos correspondientes
        GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;

    }

    // @IGM --------------------------------------------------------------------------------------
    // Funcion que se encarga de devolver el valor de una celda de la malla dadas sus coordenadas.
    // -------------------------------------------------------------------------------------------
    public TGridObject GetGridObject(int x, int y)
    {

        // Comprobamos que las coordenadas sean validas
        if (x >= 0 && y >= 0 && x < width && y < height)
        {

            // Devolvemos el valor del array
            return gridArray[x, y];

        }
        else
        {

            // Devolvemos un valor nulo
            return default(TGridObject);

        }

    }

    // @IGM ---------------------------------------------------------------------------------------------
    // Funcion que se encarga de devolver el valor de una celda de la malla dada su posición en el mundo.
    // --------------------------------------------------------------------------------------------------
    public TGridObject GetGridObject(Vector3 worldPosition)
    {

        int x, y;

        // Convertimos la posicion del mundo a la malla
        GetXY(worldPosition, out x, out y);

        // Devolvemos el valor
        return GetGridObject(x, y);

    }

    // @IGM ----------------------------------------
    // Funcion para devolver la anchura de la malla.
    // ---------------------------------------------
    public int GetWidth()
    {

        return width;

    }

    // @IGM --------------------------------------
    // Funcion para devolver la alura de la malla.
    // -------------------------------------------
    public int GetHeight()
    {

        return height;

    }

    // @IGM ---------------------------------------
    // Función para devolver el tamaño de la celda.
    // --------------------------------------------
    public float GetCellSize()
    {

        return cellSize;

    }

    // @IGM -------------------------------------------------
    // Función que deviuelve el punto donde empieza la malla.
    // ------------------------------------------------------SS
    public Vector3 GetOriginPosition()
    {

        return originPosition;

    }

}
