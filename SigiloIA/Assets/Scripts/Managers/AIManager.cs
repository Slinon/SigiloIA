using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public float timeInChase;                                   // Tiempo que deben estar los guardias persiguiendo al jugador
    public LayerMask enemyLayer;                                // Layer de los enemigos
    public LayerMask cameraLayer;                               // Layer de las camaras de seguridad
    public LayerMask laserLayer;                                // Layer de los laseres

    private float timerInChase;                                 // Tiempo actual si estar dentro del campo de vision de los guardias
    private bool timerStart;                                    // Booleano para indicar que el timer puede empezar
    private GuardBehaviour[] guardsInScene;                     // Array de guardias que hay en el nivel

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

        // Recorremos la lista de guardias
        foreach (GuardBehaviour guard in guardsInScene)
        {

            // Comprobamos si están en el estado de perseguir
            if (guard.state == State.Chase)
            {

                // Comprobamos si algun guardia tiene la vision del jugador
                if (guard.FieldOfView.player != null)
                {

                    // Reseteamos el timer
                    timerInChase = timeInChase;

                }

                // Marcamos que el timer puede empezar
                timerStart = true;

            }
            else
            {

                // Si no estan en estado de perseguir desactivamos el timer
                timerStart = false;
                timerInChase = timeInChase;

            }

        }

        // Comprobamos si el timer esta activado
        if (timerStart)
        {

            // Actualizamos el timer
            timerInChase -= Time.deltaTime;

        }

        // Comprobamos si el timer ha llegado a 0
        if (timerInChase <= 0)
        {

            // Recorremos la lista de guardias
            foreach (GuardBehaviour guard in guardsInScene)
            {

                // Calmamos al guardia
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

        // Comprobamos si ha detectado algun enemigo
        if (enemiesNearby.Length <= 0)
        {
     
            // No hacemos nada
            return;

        }

        // Escogemos el enemigo mas cercano
        GuardBehaviour closestGuard = enemiesNearby[0].gameObject.GetComponent<GuardBehaviour>();

        // Comprobamos si tiene el script deseado
        if (closestGuard != null && closestGuard.state != State.Chase)
        {

            // Alertamos al guardia
            closestGuard.AlertGuard(targetPosition);

            // Llamamos al guardia mas cercano para que ayude a este guardia
            CallNearestGuard(closestGuard.transform.position, targetPosition);

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
        float closestGuardDistance = Mathf.Infinity;
        GuardBehaviour closestGuard = null;

        // Recorremos los guardias que hay en escena
        for (int i = 0; i < guards.Length; i++)
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

        // Comprobamos si se ha encontrado al guardia mas cercano
        if (closestGuard != null && closestGuard.state != State.Chase)
        {
            
            // Alertamos al guardia
            closestGuard.AlertGuard(targetPosition);

        }

    }

    public void CientificosHuir()
    {

        // Buscamos todos los guardias que hay en la escena
        ScientistBehaviour[] scientists = GameObject.FindObjectsOfType<ScientistBehaviour>();

        // Recorremos los guardias que hay en escena
        for (int i = 0; i < scientists.Length; i++)
        {

            // Alarmamos al guardia

            scientists[i].Huir();


        }

    }

    // @GRG ---------------------------------
    // Cambiar el estado de todas las cámaras
    // --------------------------------------
    public void CallCameras(Vector3 originPosition, float communicationRange)
    {

        Collider[] camerasNearby = Physics.OverlapSphere(originPosition, communicationRange, cameraLayer);

        // Comprobamos si ha detectado algun enemigo
        if (camerasNearby.Length <= 0)
        {
            // No hacemos nada
            return;
        }

        foreach (Collider camera in camerasNearby)
        {

            //Si todavia esta en patrol
            if (camera.GetComponent<CameraBehaviour>().state == State.Patrol)
            {

                //Le cambiamos el estado a search
                camera.GetComponent<CameraBehaviour>().ChangeState();

            }

        }

    }

    // @IGM ----------------------------------------------
    // Metodo para cambiar el estado de todos los laseres.
    // ---------------------------------------------------
    public void CallLasers(Vector3 originPosition, float communicationRange)
    {

        Collider[] laserNearby = Physics.OverlapSphere(originPosition, communicationRange, laserLayer);

        // Comprobamos si ha detectado algun enemigo
        if (laserNearby.Length <= 0)
        {
            // No hacemos nada
            return;
        }

        foreach (Collider laser in laserNearby)
        {

            //Si todavia esta en patrol
            if (laser.GetComponent<LaserBehaviour>().state == State.Patrol)
            {

                //Le cambiamos el estado a search
                laser.GetComponent<LaserBehaviour>().ChangeState();

            }
        
        }

    }

}
