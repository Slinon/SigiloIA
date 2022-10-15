using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuirtySpotlight : MonoBehaviour
{
    public GameObject enemy;                        //Enemigo "padre" de este foco
    public enum State { Patrol, Search, Chase };    //Estado de este enemigo

    private bool playerInSight = false;             //booleano que indica si el jugador está en el foco
    public float timeToSpot = 2f;                   //tiempo que tarda en detectar al jugador
    public float detectionMeter = 0;                //"Barra" de detección

    private MeshRenderer meshRenderer;              //Malla del foco

    public Color patrolColor;                      
    public Color searchColor;                       //Color cuando el enemigo está modo busqueda
    public Color chaseColor;                        //Color cuando el enemigo está modo persecución

    //DebugStuff
    //bool sendSignal = true;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // @GRG --------------------------------------
    // Update del foco, lógica de cambio de color
    // y cambio de estado del enemigo
    // -------------------------------------------

    private void Update()
    {       

    }

    // @GRG -----------------------------------------------------
    // Llenar la barra si el jugador está en el área de detección
    // ----------------------------------------------------------
    void FillDetectionMeter()
    {
        if (playerInSight && detectionMeter < timeToSpot)
        {
            detectionMeter += Time.deltaTime;
        }

        else if (detectionMeter > 0)
        {
            detectionMeter -= Time.deltaTime * 2;
        }
    }

    void ChangeColor()
    {
        //if (enum).state is Patrol... (de blanco a amarillo)

        //if (enum).state is investigate... (de amarillo a rojo)
        Color myColor = new Color(1f, 1f - (detectionMeter / timeToSpot), 0f, 0.4f);
        meshRenderer.material.color = myColor;

        //if enum.start is chase... rojo
        //meshRenderer.material.color = Color.red;
    }

    // @GRG -----------------------------------------------------
    // Comprobar si el jugador ha entrado en la zona de detección
    // ----------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = true;
        }
    }

    // @GRG ----------------------------------------------------
    // Comprobar si el jugador ha salido de la zona de detección
    // ---------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = false;
        }
    }

}
