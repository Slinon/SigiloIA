using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{

    private Pathfinding pathfinding;                                            // Clase que gestiona el camino

    private static PathRequestManager instance;                                 // Instancia del manager

    // Creamos la cola de resultados
    Queue<PathResult> results = new Queue<PathResult>();

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // --------------------------------------------------------
    private void Awake()
    {

        // Asignamos la instancia del manager
        instance = this;

        // Asignamos el pathfinding
        pathfinding = GetComponent<Pathfinding>();
        
    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    void Update()
    {

        // Comprobamos si hay elementos en la cola
        if(results.Count > 0)
        {

            // Almacenamos los elementos de la cola
            int ItemsInQueue = results.Count;

            // Bloqueamos la cola
            lock (results)
            {

                // Recorremos la cola
                for (int i = 0; i < ItemsInQueue; i++)
                {

                    // Desencolamos el resultado
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);

                }

            }

        }

    }

    // @IGM ----------------------------------------------------------
    // Metodo estatico para crear una consulta de busqueda de caminos.
    // ---------------------------------------------------------------
    public static void RequestPath(PathRequest request)
    {

        // Creamos el hilo
        ThreadStart threadStart = delegate
        {

            // Buscamos el camino
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);

        };

        // Lanzamos el hilo
        threadStart.Invoke();

    }

    // @IGM -----------------------------------------------
    // Metodo para notificar que ha finalizado la consulta.
    // ----------------------------------------------------
    public void FinishedProcessingPath(PathResult result)
    {

        // Bloqueamos la cola
        lock (results)
        {

            // Encolamos el resultado
            results.Enqueue(result);

        }

    }

}

// @IGM -----------------------------------------
// Estructura de la petición de encontrar camino.
// ----------------------------------------------
public struct PathRequest
{

    public Vector3 pathStart;                                               // Posicion inicial del camino
    public Vector3 pathEnd;                                                 // Posicion final del camino
    public Action<Vector3[], bool> callback;                                // Respuesta de la consulta

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {

        // Establecemos las variables del constructor.
        this.pathStart = start;
        this.pathEnd = end;
        this.callback = callback;

    }

}

// @IGM --------------------------------------------------------
// Estructura que guardala informacion del resultado del camino.
// -------------------------------------------------------------
public struct PathResult
{

    public Vector3[] path;                          // Camino a seguir
    public bool success;                            // Exito al buscar el camino
    public Action<Vector3[], bool> callback;        // Respuesta

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {

        this.path = path;
        this.success = success;
        this.callback = callback;

    }

}
