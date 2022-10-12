using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuirtySpotlight : MonoBehaviour
{
    private bool playerInSight = false;             //booleano que indica si el jugador est� en el foco
    public float timeToSpot = 2f;                   //tiempo que tarda en detectar al jugador
    public float detectionMeter = 0;                //"Barra" de detecci�n

    private MeshRenderer meshRenderer;              //Malla del foco
    public GameObject thisSpotlightCamera;          //c�mara asociada a este foco

    //DebugStuff
    bool sendSignal = true;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        FillDetectionMeter();               //Llenar la barra si el jugador est� siendo detectado
        ChangeColor();                      //Cambiar el color seg�n el estado de la barra

        if (detectionMeter > timeToSpot)    //Si se llena la barra...
        {
            if (sendSignal)
            {
                //L� c�mara a la que corresponde el foco enviar� una se�al
                thisSpotlightCamera.GetComponent<SecurityCamera>().PlayerSpotted(); 
                sendSignal = false;
            }           
        }
    }

    // @GRG -----------------------------------------------------
    // Llenar la barra si el jugador est� en el �rea de detecci�n
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
    // Comprobar si el jugador ha entrado en la zona de detecci�n
    // ----------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = true;
        }
    }

    // @GRG ----------------------------------------------------
    // Comprobar si el jugador ha salido de la zona de detecci�n
    // ---------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.tag is "Player")
        {
            playerInSight = false;
        }
    }

}
