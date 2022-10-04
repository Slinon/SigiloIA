using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadious;               // Radio de vision del enemigo
    [Range(0,360)]
    public float viewAngle;                 // Angulo de vision del enemigo
    public LayerMask playerMask;            // Capa del jugador
    public LayerMask obstacleMask;          // Capa de los obstaculos
    public Transform player;                // Posici�n del jugador

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Lanzamos la corrutina cada 0.2 segundos
        StartCoroutine("FindPlayerWithDelay", 0.2f);

    }

    // @IGM -------------------------------------------
    // Corrutina para buscar al jugador no tan seguido.
    // ------------------------------------------------
    IEnumerator FindPlayerWithDelay(float delay)
    {

        while (true)
        {

            // Esperamos el tiempo marcado y lanzamos el metodo
            yield return new WaitForSeconds(delay);
            FindPlayer();

        }

    }

    // @IGM ---------------------------------------------
    // Metodo para encontrar al jugador dentro del rango.
    // --------------------------------------------------
    private void FindPlayer()
    {

        player = null;

        // Buscamos al jugador en la capa del jugador
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadious, playerMask);

        // Recorremos el array por si hay m�s de un jugador
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {

            // Asignamos la posici�n del jugador
            Transform playeInRange = targetsInViewRadius[i].transform;

            // Buscamos la direcci�n del jugador
            Vector3 dirToPlayer = (playeInRange.position - transform.position).normalized;

            // Comprobamos que el angulo del jugador est� dentro del �ngulo de vision
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {

                // Buscamos la distancia entre el enemigo y el jugador
                float distToPlayer = Vector3.Distance(transform.position, playeInRange.position);

                // Comprobamos que no hay ning�n obst�culo por el medio
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {

                    // Asignamos la variable jugador
                    player = playeInRange;

                }

            }

        }

    }

    // @IGM -------------------------------------------------------
    // Metodo para averiguar la direcci�n dado el angulo de vision.
    // ------------------------------------------------------------
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {

        // Comprobamos si el �ngulo est� en la posici�n global
        if (!angleIsGlobal)
        {

            // A�adimos la rotacion del objeto
            angleInDegrees += transform.eulerAngles.y;

        }

        // Devolvemos la direccion del angulo
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, 
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }

}
