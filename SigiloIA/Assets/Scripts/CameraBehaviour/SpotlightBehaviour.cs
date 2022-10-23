using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // Lógica del FOCO: area de detección del jugador
    // --------------------------------

    [Header("General")]
    [SerializeField] private GameObject enemyParent;        //Enemigo "padre" de este foco
    private bool playerInSight = false;                     //booleano que indica si el jugador está en el foco
    private MeshRenderer mesh;                              //Malla del foco

    [Header("Spotlight attributes")]
    public float timeToSpot = 2f;                           //tiempo que tarda en detectar al jugador
    public float detectionMeter = 0;                       //"Barra" de detección

    [Header("State colors")]
    [SerializeField] private Color patrolColor;             //Color cuando el enemigo está patrullando
    [SerializeField] private Color searchColor;             //Color cuando el enemigo está investigando
    [SerializeField] private Color chaseColor;              //Color cuando el enemigo está persiguiendo

    // @GRG ---------------------------
    // Start: Inicialización de variables
    // --------------------------------
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // @GRG ---------------------------
    // Update: se llama una vez por frame
    // --------------------------------
    void Update()
    {
        if (enemyParent.GetComponent<CameraBehaviour>().state != State.Chase)
        {
            UpdateDetectionMeter();
            UpdateEnemyState();
            UpdateColor();
        }   
    }

    // @GRG ---------------------------
    // Tiempo que tarda el enemigo en cambiar de estado
    // --------------------------------
    void UpdateDetectionMeter()
    {
        //Si el jugador está siendo bajo el foco y aún no se ha "llenado la barra"
        if (playerInSight && detectionMeter < timeToSpot)
        {
            //Llenar la barra
            detectionMeter += Time.deltaTime;
        }

        //Si el jugador ya no está bajo el foco y el valor de la barra es superior a 0
        else if (detectionMeter > 0)
        {
            //Vaciar la barra
            detectionMeter -= Time.deltaTime * 2;
        }
    }


    // @GRG ---------------------------
    // Cambiar de estado
    // --------------------------------
    void UpdateEnemyState()
    {
        //Si el la barra se ha llenado
        if (detectionMeter > timeToSpot)
        {
            //Resetear la barra
            detectionMeter = 0;

            timeToSpot /= 2;
            
            //LLamar a PlayerSpotted
            enemyParent.GetComponent<CameraBehaviour>().PlayerSpotted();
        }
    }

    // @GRG ---------------------------
    // Cambio de color del foco
    // --------------------------------
    void UpdateColor()
    {
        //Convendría crear una clase génerica de enemigo.
        //Ahora mismo solo funcionaría para el enemigo cámara.
        if (enemyParent.GetComponent<CameraBehaviour>().state == State.Patrol)
        {
            mesh.material.color = Color.Lerp(patrolColor, searchColor, detectionMeter / timeToSpot);
        }

        if (enemyParent.GetComponent<CameraBehaviour>().state == State.Search)
        {
            mesh.material.color = Color.Lerp(searchColor, chaseColor, detectionMeter / timeToSpot);
        }

        if (enemyParent.GetComponent<CameraBehaviour>().state == State.Chase)
        {
            mesh.material.color = chaseColor;
        }
    }


    // @GRG ---------------------------
    // Comprobar si el jugador ha entrado en la zona de detección
    // --------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = true;
        }
    }

    // @GRG ---------------------------
    // Comprobar si el jugador ha salido de la zona de detección
    // --------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = false;
        }
    }

}
