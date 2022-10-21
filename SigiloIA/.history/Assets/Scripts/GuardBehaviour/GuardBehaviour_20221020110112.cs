using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{
    
    [Header("General")]
    public float stoppingDistance;                  // Distancia de seguridad al punto al que se mueve el guardia
    [HideInInspector]
    public State state;                             // Estado del guardia

    private AIMovement aIMovement;                  // Gesti�n de movimiento de la IA
    private FieldOfView fieldOfView;                // Campo de vision del guardia
    public FieldOfView FieldOfView
    {
        get { return fieldOfView; }
    }
    private Vector3 currentPoint;                   // Posici�n destino dentro de la patrulla
    private bool playerSpotted;                     // Booleano que indica si ya se ha visto al jugador

    [Header("Patrol")]
    public Transform[] patrolPoints;                // Array de puntos de patrulla
    public float timeToSearch;                      // Tiempo que tarda en pillar al jugador
    public Color patrolColor;                       // Color de la linea de vision cuando el guardia esta patrullando

    private int currentPointIndex;                  // Indice del punto al que se mueve el guardia
    private float detectionMeter;                   // Barra de deteccion

    [Header("Search")]
    public float timeToChase;                       // Tiempo que tarda en perseguir al jugador
    public Color searchColor;                       // Color de la linea de vision cuando el guardia esta investigando
    public float timeSearching;                     // Tiempo que el guardia estara buscando al jugador

    private float chaseMeter;                       // Barra de captura
    private float currentSeachTime;                 // Tiempo actual de busqueda

    [Header("Chase")]
    public Color chaseColor;                        // Color de la linea de vision cuando el guardia esta persiguiendo al jugador
    public Transform player;                        // Posicion del jugador

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Establecemos el estado a patrulla
        state = State.Patrol;

        // Asignamos el primer punto del array
        currentPointIndex = 0;
        currentPoint = patrolPoints[currentPointIndex].position;

        // Asignamos el movimiento a la IA
        aIMovement = GetComponent<AIMovement>();

        // Movemos la IA al primer punto de patrulla
        aIMovement.target = currentPoint;

        // Asignamos el campo de vision
        fieldOfView = GetComponent<FieldOfView>();

        // Establecemos el color del material del campo de vision al color de patrulla
        fieldOfView.meshRenderer.material.color = patrolColor;

    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    private void Update()
    {

        // Comprobamos cual es el estado del guardia
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

        // Actualizamos el color del campo de vision
        SetDetectiometer();
        
    }

    // @IGM -------------------------------------------------
    // Metodo para alarmar un guardia y perseguir al jugador.
    // ------------------------------------------------------
    public void AlarmGuard()
    {

        // Cambiamos el estado de la patrulla
        state = State.Chase;

        // Marcamos que se ha detectado al jugador
        playerSpotted = true;

        // Actualizamos la posici�n del jugador
        currentPoint = player.position;
        aIMovement.target = currentPoint;

    }

    // @IGM ------------------------------------------------
    // Metodo para alertar y enviar un guardia a investigar.
    // -----------------------------------------------------
    public void AlertGuard(Vector3 targetPosition)
    {

        // Cambiamos el estado de la patrulla
        state = State.Search;

        // Marcamos que se ha detectado al jugador
        playerSpotted = true;

        // Actualizamos la posici�n del jugador
        currentPoint = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        aIMovement.target = currentPoint;

    }

    public void CalmGuard()
    {

        // Cambiamos el estado de la patrulla
        state = State.Patrol;

        // Actualizamos la posici�n del jugador
        currentPoint = patrolPoints[currentPointIndex].position;
        aIMovement.target = currentPoint;

        // Reseteamos el timer
        currentSeachTime = 0;

    }

    // @IGM ---------------------------------------
    // Metodo para realizar la captura del jugador.
    // --------------------------------------------
    private void ChasePlayer()
    {

        // Actualizamos la posici�n del jugador
        currentPoint = player.position;
        aIMovement.target = currentPoint;

        // Comprobamos si el guardia ha pillado al jugador
        if (Vector3.Distance(transform.position, currentPoint) < stoppingDistance)
        {

            Debug.Log("GAME OVER!!");

        }

    }

    // @IGM ---------------------------
    // Metodo para detectar al jugador.
    // --------------------------------
    private void DetectPlayer()
    {

        // Comprobamos que el jugador no ha sido detectado ya
        if (!playerSpotted)
        {

            // Comprobamos si el jugador est� dentro del rango de visi�n
            if (fieldOfView.player != null && detectionMeter < timeToSearch)
            {

                // Aumentamos la barra de deteccion
                detectionMeter += Time.deltaTime;

            }
            else if (detectionMeter > 0)
            {

                // Disminuimos la barra de deteccion
                detectionMeter -= Time.deltaTime * 2;

            }

        }
        else
        {

            // Comprobamos si el jugador esta dentro del rango de vision
            if (fieldOfView.player != null && chaseMeter < timeToChase)
            {

                // Aumentamos la barra de captura
                chaseMeter += Time.deltaTime;

            }
            else if (chaseMeter > 0)
            {

                // Disminuimos la barra de captura
                chaseMeter -= Time.deltaTime * 2;

            }

        }

    }

    // @IGM ---------------------------------------
    // Metodo para realizar la patruya del guardia.
    // --------------------------------------------
    private void Patrol()
    {

        // Comprobamos si hemos llegado al siguiente punto de la patrulla
        if (Vector3.Distance(transform.position, currentPoint) < stoppingDistance)
        {

            // Iteramos el indice
            currentPointIndex++;

            // Comprobamos si se ha llegado al final del array
            if (currentPointIndex == patrolPoints.Length)
            {

                currentPointIndex = 0;

            }

            // Actualizamos el punto al que iremos
            currentPoint = patrolPoints[currentPointIndex].position;

            // Establecemos el camino
            aIMovement.target = currentPoint;

        }

        // Actualizamos el campo de vision del guardia buscando al enemigo
        DetectPlayer();

        // Comprobamos si el jugador ha sido detectado
        if (detectionMeter > timeToSearch && fieldOfView.player != null && !playerSpotted)
        {

            // Alertamos al guardia
            AlertGuard(fieldOfView.player.position);

            // Avisamos al guardia m�s cercano para que investigue con el
            AIManager.Instance.CallNearestGuard(transform.position, fieldOfView.player.position);

        }
        else if (chaseMeter > timeToChase && fieldOfView.player != null && playerSpotted)
        {

            // Avisamos a todos los guardias y activamos las alarmas
            AIManager.Instance.CallAllGuards(fieldOfView.player.position);

        }

    }

    // @IGM ---------------------------------------------
    // Metodo para realizar la investigaci�n del guardia.
    // --------------------------------------------------
    private void SearchPlayer()
    {
        // Cambiamos el ícono
        

        // Comprobamos si hemos llegado al punto donde se vio al jugador por ultima vez
        if (Vector3.Distance(transform.position, currentPoint) < stoppingDistance)
        {

            // Comprobamos que no hemos superado el tiempo de busqueda
            if (currentSeachTime < timeSearching)
            {

                // Rotamos el guardia para que busque al jugador
                transform.Rotate(0, aIMovement.turnSpeed * 5 * Time.deltaTime, 0, Space.Self);

                // Actualizamos el tiempo
                currentSeachTime += Time.deltaTime;

            }
            else
            {

                //Calmamos al guardia
                CalmGuard();

            }
        }

        // Actualizamos el campo de vision del guardia buscando al enemigo
        DetectPlayer();

        if (chaseMeter > timeToChase && fieldOfView.player != null && playerSpotted)
        {

            // Avisamos a todos los guardias y activamos las alarmas
            AIManager.Instance.CallAllGuards(fieldOfView.player.position);

        }

    }

    // @IGM -------------------------------------------------
    // Metodo para cambiar el color de la vision del guardia.
    // ------------------------------------------------------
    private void SetDetectiometer()
    {

        // Establecemos el color del material de la linea de vision en base al estado
        if (state == State.Patrol)
        {

            // Comporbamos si aun no hemos detectado al jugador ninguna vez
            if (!playerSpotted)
            {

                // Establecemos valores de patrulla normales
                fieldOfView.meshRenderer.material.color = Color.Lerp(patrolColor, searchColor, detectionMeter / timeToSearch);

            }
            else
            {

                // Establecemos valores de patrulla de alerta
                fieldOfView.meshRenderer.material.color = Color.Lerp(searchColor, chaseColor, chaseMeter / timeToChase);

            }

        }
        else if (state == State.Search)
        {

            // El guardia esta investigando
            // Establecemos valores de alerta
            fieldOfView.meshRenderer.material.color = Color.Lerp(searchColor, chaseColor, chaseMeter / timeToChase);

        }
        else
        {

            // El guardia esta persiguiendo
            // Establecemos valores de persecucion
            fieldOfView.meshRenderer.material.color = chaseColor;

        }

    }

}
