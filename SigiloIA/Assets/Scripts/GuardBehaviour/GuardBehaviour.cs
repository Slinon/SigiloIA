using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{

    public Transform[] patrolPoints;                // Array de puntos de patrulla
    public float stoppingDistance;                  // Distancia de seguridad al punto al que se mueve el guardia
    [HideInInspector]
    public State state;                             // Estado del guardia

    private AIMovement aIMovement;                  // Gestión de movimiento de la IA
    private Transform currentPoint;                  // Posición destino dentro de la patrulla
    private int currentPointIndex;                  // Indice del punto al que se mueve el guardia

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Establecemos el estado a patrulla
        state = State.Patrol;

        // Asignamos el primer punto del array
        currentPointIndex = 0;
        currentPoint = patrolPoints[currentPointIndex];

        // Asignamos el movimiento a la IA
        aIMovement = GetComponent<AIMovement>();

        // Movemos la IA al primer punto de patrulla
        aIMovement.target = currentPoint;

    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    private void Update()
    {

        // Comprobamos cuál es el estado del guardia
        switch (state)
        {

            // Patrulla
            case State.Patrol:
                Patrol();
                break;

            // Investigar
            case State.Search:
                SearchPlayer();
                break;

            // Cazar al jugador
            case State.Chase:
                ChasePlayer();
                break;

        }
        
    }

    // @IGM ---------------------------------------
    // Metodo para realizar la patruya del guardia.
    // --------------------------------------------
    private void Patrol()
    {

        // Comprobamos si hemos llegado al siguiente punto de la patrulla
        if (Vector3.Distance(transform.position, currentPoint.position) < stoppingDistance)
        {

            // Iteramos el indice
            currentPointIndex++;

            // Comprobamos si se ha llegado al final del array
            if (currentPointIndex == patrolPoints.Length)
            {

                currentPointIndex = 0;

            }

            // Actualizamos el punto al que iremos
            currentPoint = patrolPoints[currentPointIndex];

            // Establecemos el camino
            aIMovement.target = currentPoint;

        }

    }

    // @IGM ---------------------------------------------
    // Metodo para realizar la investigación del guardia.
    // --------------------------------------------------
    private void SearchPlayer()
    {



    }

    // @IGM ---------------------------------------
    // Metodo para realizar la captura del jugador.
    // --------------------------------------------
    private void ChasePlayer()
    {



    }

}
