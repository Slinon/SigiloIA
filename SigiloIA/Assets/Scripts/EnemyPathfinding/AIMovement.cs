using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    const float MIN_PATH_UPDATE_TIME = 0.2f;            // Constante de tiempo de actualizaci�n de camino
    const float PATH_UPDATE_MOVE_THRESHOLD = 0.5f;      // Constante de margen de movimiento del target

    [HideInInspector]
    public Vector3 target;                              // Objetivo del movimiento
    public bool displayPathGizmos;                      // Booleano para mostrar el camino
    public float patrolSpeed;                           // Velocidad de movimiento cuando el enemigo esta patrullando
    public float alarmSpeed;                            // Velocidad de movimiento cuando el enemigo esta en persecucion
    public float turnSpeed;                             // Velocidad de rotacion
    public float turnDistance;                          // Distancia de volteado

    public float speed;                                // Velocidad de movimiento
    private Path path;                                  // Camino a seguir

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    void Start()
    {

        // Asignamos la velocidad inicial
        speed = patrolSpeed;

        // Lanzamos la corrutina de movimiento
        StartCoroutine(UpdatePath());

    }

    // @IGM --------------------------------------------
    // Metodo para disminuir la velocidad de movimiento.
    // -------------------------------------------------
    public void DecreaseSpeed()
    {

        // Actualizamos la velocidad
        speed = patrolSpeed;

    }

    // @IGM -------------------------------------------
    // Metodo para aumentar la velocidad de movimiento.
    // ------------------------------------------------
    public void IncreaseSpeed()
    {

        // Actualizamos la velocidad
        speed = alarmSpeed;

    }

    // @IGM --------------------------------------------
    // Accion que empieza con el movimiento del guardia.
    // -------------------------------------------------
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {

        // Comprobamos que el camino se ha construido correctamente
        if (pathSuccessful)
        {

            // Lanzamos la corrutina
            path = new Path(waypoints, transform.position, turnDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }

    }

    // @IGM -----------------------------
    // Corrutina de actualizar el camino.
    // ----------------------------------
    IEnumerator UpdatePath()
    {

        // Pedimos un nuevo camino
        if (Time.timeSinceLevelLoad < 0.3f)
        {

            yield return new WaitForSeconds(0.3f);

        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));

        // Asignamos las variables
        float sqrMoveThreshold = PATH_UPDATE_MOVE_THRESHOLD * PATH_UPDATE_MOVE_THRESHOLD;
        Vector3 targetPosOld = target;

        while (true)
        {

            // Esperamos el tiempo asignado
            yield return new WaitForSeconds(MIN_PATH_UPDATE_TIME);

            // Comprobamos si la posicion del target ha cambiado lo suficiente
            if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {

                // Pedimos un nuevo camino
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));

                // Actualizamos la posicion del target
                targetPosOld = target;

            }


        }

    }

    // @IGM --------------------------------------
    // Corrutina de seguimiento hacia el objetivo.
    // -------------------------------------------
    IEnumerator FollowPath()
    {

        // Establecemos los parametros
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        // Mientras sigamos en el camino
        while (followingPath)
        {

            // Transformamos a Vector2 la posicion
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

            // Mientras no crucemos la linea
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {

                // Comprobamos si el guardia ha llegado al final
                if (pathIndex == path.finishLineIndex)
                {

                    // Ya hemos llegado al final
                    followingPath = false;
                    break;

                }
                else
                {

                    // Vamos al siguiente indice
                    pathIndex++;

                }

            }

            // Comprobamos si seguimos en el camino
            if (followingPath)
            {

                // Rotamos el guardia
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                // Movemos el guardia
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

            }

            yield return null;

        }

    }

    // @IGM ------------------------------------------------------------
    // OnDrawGizmos draw gizmos that are also pickable and always drawn.
    // -----------------------------------------------------------------
    public void OnDrawGizmos()
    {

        // Comprobamos que existe camino
        if (path != null && displayPathGizmos)
        {

            //Dibujamos el camino
            path.DrawWithGizmos();

        }

    }

}
