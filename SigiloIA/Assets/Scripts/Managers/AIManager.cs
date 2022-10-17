using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public float timeInChase;                                   // Tiempo que deben estar los guardias persiguiendo al jugador
    public LayerMask enemyLayer;                                // Layer de los enemigos

    private float timerInChase;
    private bool timerStart;
    public GuardBehaviour[] guardsInScene;                     // Array de guardias que hay en el nivel

    public static AIManager Instance { get; private set; }     // Instancia de la clase para el singleton

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si existe otra instancia del manager
        if (Instance != null && Instance != this)
        {

            // La destruimos
            Destroy(this);

        }
        else
        {

            // La asignamos
            Instance = this;

        }

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Recuperamos la lista de guardias que hay en la escena
        guardsInScene = GameObject.FindObjectsOfType<GuardBehaviour>();

        // Inicializamos el timer
        timerInChase = timeInChase;

    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    private void Update()
    {

        foreach (GuardBehaviour guard in guardsInScene)
        {

            if (guard.state == State.Chase)
            {

                if (guard.FieldOfView.player != null)
                {

                    timerInChase = timeInChase;

                }

                timerStart = true;

            }
            else
            {

                timerStart = false;
                timerInChase = timeInChase;

            }

        }


        if (timerStart)
        {

            timerInChase -= Time.deltaTime;

        }

        Debug.Log(timerInChase);
        

        if (timerInChase <= 0)
        {

            foreach (GuardBehaviour guard in guardsInScene)
            {

                guard.CalmGuard();

            }

        }

    }

    // @IGM --------------------------------------------------------------
    // Metodo para alarmar a todos los guardias y que persigan al jugador.
    // -------------------------------------------------------------------
    public void CallAllGuards(Vector3 targetPosition)
    {

        // Buscamos todos los guardias que hay en la escena
        GuardBehaviour[] guards = GameObject.FindObjectsOfType<GuardBehaviour>();

        // Recorremos los guardias que hay en escena
        for (int i = 0; i < guards.Length; i++)
        {

            // Alarmamos al guardia
            guards[i].AlarmGuard();

        }

    }

    public void CalmAllGuards()
    {

        // Buscamos todos los guardias que hay en la escena
        GuardBehaviour[] guards = GameObject.FindObjectsOfType<GuardBehaviour>();

        // Recorremos los guardias que hay en escena
        for (int i = 0; i < guards.Length; i++)
        {

            // Calmamos al guardia
            guards[i].CalmGuard();

        }

    }

    // @IGM -------------------------------------------------------------------------------------
    // Metodo para alertar al guardia mas cercano a una posicion dado un rango de comunicaciones.
    // ------------------------------------------------------------------------------------------
    public void CallGuard(Vector3 originPosition, float communicationRange, Vector3 targetPosition)
    {

        //Buscamos todos los enemigos que hay en el rango
        Collider[] enemiesNearby = Physics.OverlapSphere(originPosition, communicationRange, enemyLayer);

        // Escogemos el enemigo mas cercano
        GuardBehaviour closestGuard = enemiesNearby[0].gameObject.GetComponent<GuardBehaviour>();

        // Comprobamos si tiene el script deseado
        if (closestGuard != null)
        {

            // Alertamos al guardia
            closestGuard.AlertGuard(targetPosition);

        }

    }

    // @IGM -----------------------------------------------------
    // Metodo para alertar al guardia mas cercano a una posicion.
    // ----------------------------------------------------------
    public void CallNearestGuard(Vector3 originPosition, Vector3 targetPosition)
    {

        // Buscamos todos los guardias que hay en la escena
        GuardBehaviour[] guards = GameObject.FindObjectsOfType<GuardBehaviour>();

        // Calculamos la distancia entre el guardia y la posicion dada
        float closestGuardDistance = Vector3.Distance(originPosition, guards[0].transform.position);
        GuardBehaviour closestGuard = guards[0];

        // Recorremos los guardias que hay en escena
        for (int i = 1; i < guards.Length; i++)
        {

            // Comprobamos si el guardia es el guardia que ha hecho la llamada
            if (guards[i].transform.position == originPosition)
            {

                // Lo saltamos
                continue;

            }

            // Comprobamos que la distancia entre el guardia en el array y el punto de llamada sea el menor
            if (Vector3.Distance(originPosition, guards[i].transform.position) < closestGuardDistance)
            {

                // Actualizamos las variables
                closestGuardDistance = Vector3.Distance(originPosition, guards[i].transform.position);
                closestGuard = guards[i];

            }

        }

        // Alertamos al guardia
        closestGuard.AlertGuard(targetPosition);

    }

}
