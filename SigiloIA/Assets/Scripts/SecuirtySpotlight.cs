using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuirtySpotlight : MonoBehaviour
{
    public float timeToSpot = 2f;                   //tiempo que tarda en detectar al jugador
    private bool playerInSight = false;             //booleano que indica si el jugador está en el foco
    private float detectionMeter = 0;               //"Barra" de detección

    public GameObject thisSpotlightCamera;          //cámara asociada a este foco

    private void Update()
    {
        FillDetectionMeter();

        if (detectionMeter > timeToSpot) //Si se llena la barra...
        {
            detectionMeter = 0f; //Se resetea para evitar que envíe la señal varias veces
            thisSpotlightCamera.GetComponent<SecurityCamera>().PlayerSpotted(); //Lá cámara a la que corresponde el foco enviará una señal
        }
    }

    // @GRG -----------------------------------------------------
    // Llenar la barra si el jugador está en el área de detección
    // ----------------------------------------------------------
    void FillDetectionMeter()
    {
        if (playerInSight)
        {
            detectionMeter += Time.deltaTime;
        }

        else if (detectionMeter > 0)
        {
            detectionMeter -= Time.deltaTime;
        }
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
