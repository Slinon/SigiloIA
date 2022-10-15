using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{

    public readonly Vector3[] lookPoints;                   // Puntos hacia donde mira el objeto
    public readonly Line[] turnBoundaries;                  // Lineas de los puntos
    public readonly int finishLineIndex;                    // Indice de las lineas
    
    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public Path(Vector3[] waypoints, Vector3 startPos, float turnDist)
    {

        // Asignamos los atributos
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        // Asignamos el punto anterior
        Vector2 previousPoint = V3ToV2(startPos);

        // Recorremos los puntos
        for (int i = 0; i < lookPoints.Length; i++)
        {

            // Asignamos el punto actual
            Vector2 currentPoint = V3ToV2(lookPoints[i]);

            // Calculamos la distancia al punto actual
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;

            // Calculamos el punto de rebote
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint 
                : currentPoint - dirToCurrentPoint * turnDist;

            // Asignamos los rebotes
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDist);
            previousPoint = turnBoundaryPoint;

        }

    }

    // @IGM ---------------------------------------------
    // Funcion para transformar un Vector3 en un Vector2.
    // --------------------------------------------------
    private Vector2 V3ToV2(Vector3 vector3)
    {

        return new Vector2(vector3.x, vector3.z);

    }

    // @IGM ------------------------
    // Metodo para dibujar la linea.
    // -----------------------------
    public void DrawWithGizmos()
    {

        Gizmos.color = Color.black;

        // Recorremos el array de posiciones
        foreach (Vector3 point in lookPoints)
        {

            // Dibujamos el nodo
            Gizmos.DrawCube(point + Vector3.up, Vector3.one);

        }

        Gizmos.color = Color.white;

        // Recorremos las lineas
        foreach (Line line in turnBoundaries)
        {

            line.DrawWithGizmos(10);

        }

    }

}
