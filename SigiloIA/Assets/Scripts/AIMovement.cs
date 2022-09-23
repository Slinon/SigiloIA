using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public float speed =  10f;                              // Velocidad de la IA

    private int currentPathIndex;                           // Indice actual del camino
    private Vector3 targetWorldPosition;                         // Destino de la IA
    private List<Vector3> pathVectorList;                   // Lista de vectores que forman el camino

    // @IGM -------------------
    // Metodo para mover la IA.
    // ------------------------
    public void MoveAI()
    {

        // Comprobamos que tenemos un camino válido
        if (pathVectorList == null || pathVectorList.Count <= 0)
        {

            StopMoving();
            return;

        }

        // Comprobamos si estamos en el último nodo
        if (currentPathIndex >= pathVectorList.Count)
        {

            // Calculamos la dirección a la que se tiene que over la IA
            Vector3 moveDir = (targetWorldPosition - transform.position).normalized;

            // Actualizamos la posicion de la IA
            transform.position = transform.position + moveDir * speed * Time.deltaTime;

            // Comprobamos si hemos llegado al punto final
            if (Vector3.Distance(transform.position, targetWorldPosition) < 1f)
            {

                // Paramos el movimiento
                StopMoving();

            }

        }
        else
        {

            // Guardamos la posición a la que jnos vamos a mover
            Vector3 targetPosition = pathVectorList[currentPathIndex];

            // Comrpobamos si aun no hemos llegado a la posición marcada
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {

                // Calculamos la dirección a la que se tiene que over la IA
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                // Actualizamos la posicion de la IA
                transform.position = transform.position + moveDir * speed * Time.deltaTime;

            }
            else
            {

                // Hemos llegado al nodo actual, lo actualizamos
                currentPathIndex++;

            }

        }

    }

    // @IGM ----------------------------------------------------
    // Metodo para marcar la posición a la que vamos a movernos.
    // ---------------------------------------------------------
    public void SetTargetPosition(Vector3 targetPosition)
    {

        // Restablecemos el índice y el destino
        currentPathIndex = 0;
        targetWorldPosition = targetPosition;

        // Calculamos el nuevo camino
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        // Debug
        if (pathVectorList != null)
        {

            for (int i = 0; i < pathVectorList.Count - 1; i++)
            {

                Debug.DrawLine(pathVectorList[i], pathVectorList[i + 1], Color.green, 10f);

            }

        }
        // Debug end

    }

    // @IGM ------------------------------------
    // Metodo para parar el movimiento de la IA.
    // -----------------------------------------
    private void StopMoving()
    {

        pathVectorList = null;

    }

    // @IGM ---------------------------------------
    // Funcion para encontrar la posicion de la IA.
    // --------------------------------------------
    public Vector3 GetPosition()
    {

        return transform.position;

    }

}
