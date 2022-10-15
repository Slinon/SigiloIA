using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line 
{

    const float VERTICAL_LINE_GRADIENT = 1E5F;

    private float gradient;                     // Pendiente de la recta
    private float yIntercept;                   // Y de interceptacion
    private float gradientPerpendicular;        // Pendiente de la recta perpendicular
    private bool approachSide;                  // Booleano para averiguar si cruzamos la linea
    private Vector2 pointOnLine1;               // Punto 1 sobre la linea
    private Vector2 pointOnLine2;               // Punto 2 sobre la linea

    // @IGM ------------------------
    // Constructor de la estructura.
    // -----------------------------
    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {

        // Calculamos las distancias
        float distanceX = pointOnLine.x - pointPerpendicularToLine.x;
        float distanceY = pointOnLine.y - pointPerpendicularToLine.y;

        // Comprobamos que la distancia es 0
        if (distanceX == 0)
        {

            // Es una linea vertical
            gradientPerpendicular = VERTICAL_LINE_GRADIENT;

        }
        else
        {

            // Calculamos la pendiente de la linea perpendicular
            gradientPerpendicular = distanceY / distanceX;

        }

        // Comprobamos que ela pendiente perpendicular es 0
        if (gradientPerpendicular == 0)
        {

            // Es una linea vertical
            gradient = VERTICAL_LINE_GRADIENT;

        }
        else
        {

            // Calculamos la pendiente de la linea
            gradient = -1 / gradientPerpendicular;

        }

        // Calculamos la y
        yIntercept = pointOnLine.y - gradient * pointOnLine.x;

        // Asignamos los puntos
        pointOnLine1 = pointOnLine;
        pointOnLine2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);

    }

    // @IGM ------------------------------------------------------
    // Funcion para saber si el punto esta en el lado de la linea.
    // -----------------------------------------------------------
    private bool GetSide(Vector2 point)
    {

        return (point.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y) 
            > (point.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x);

    }

    // @IGM -----------------------------------------
    // Funcion para saber si el punto cruza la linea.
    // ----------------------------------------------
    public bool HasCrossedLine(Vector2 point)
    {

        return GetSide(point) != approachSide;

    }

    // @IGM ------------------------
    // Metodo para dibujar la linea.
    // -----------------------------
    public void DrawWithGizmos(float length)
    {

        // Dibujamos la linea
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCenter = new Vector3(pointOnLine1.x, 0, pointOnLine1.y) + Vector3.up;
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);

    }

}
