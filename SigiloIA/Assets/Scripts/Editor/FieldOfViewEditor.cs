using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    // @IGM ---------------------------------------------------
    // Enables the Editor to handle an event in the Scene view.
    // --------------------------------------------------------
    private void OnSceneGUI()
    {

        // Dibujamos un círculo alrededor para mostrar el radio
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadious);

        // Recuperamos la direccion de los angulos laterales
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        // Dibujamos las lineas
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadious);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadious);

        // Comprobamos si se tiene visual con el jugador
        if (fow.player != null)
        {

            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, fow.player.position);

        }

    }
}
