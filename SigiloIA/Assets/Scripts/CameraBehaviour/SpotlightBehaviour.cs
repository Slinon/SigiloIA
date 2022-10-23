using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // L�gica del FOCO: area de detecci�n del jugador
    // --------------------------------

    [Header("General")]
    [SerializeField] private GameObject enemyParent;        //Enemigo "padre" de este foco
    private bool playerInSight = false;                     //booleano que indica si el jugador est� en el foco
    private MeshRenderer mesh;                              //Malla del foco

    [Header("Spotlight attributes")]
    public float timeToSpot = 2f;                           //tiempo que tarda en detectar al jugador
    public float detectionMeter = 0;                       //"Barra" de detecci�n

    [Header("State colors")]
    [SerializeField] private Color patrolColor;             //Color cuando el enemigo est� patrullando
    [SerializeField] private Color searchColor;             //Color cuando el enemigo est� investigando
    [SerializeField] private Color chaseColor;              //Color cuando el enemigo est� persiguiendo

    // @GRG ---------------------------
    // Start: Inicializaci�n de variables
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
        //Si el jugador est� siendo bajo el foco y a�n no se ha "llenado la barra"
        if (playerInSight && detectionMeter < timeToSpot)
        {
            //Llenar la barra
            detectionMeter += Time.deltaTime;
        }

        //Si el jugador ya no est� bajo el foco y el valor de la barra es superior a 0
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
        //Convendr�a crear una clase g�nerica de enemigo.
        //Ahora mismo solo funcionar�a para el enemigo c�mara.
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
    // Comprobar si el jugador ha entrado en la zona de detecci�n
    // --------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = true;
        }
    }

    // @GRG ---------------------------
    // Comprobar si el jugador ha salido de la zona de detecci�n
    // --------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = false;
        }
    }

}
