using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuirtySpotlight : MonoBehaviour
{
    public float timeToSpot = 2f;                   //tiempo que tarda en detectar al jugador
    private bool playerInSight = false;             //booleano que indica si el jugador est� en el foco
    private float detectionMeter = 0;               //"Barra" de detecci�n

    public GameObject thisSpotlightCamera;          //c�mara asociada a este foco

    private void Update()
    {
        FillDetectionMeter();

        if (detectionMeter > timeToSpot) //Si se llena la barra...
        {
            detectionMeter = 0f; //Se resetea para evitar que env�e la se�al varias veces
            thisSpotlightCamera.GetComponent<SecurityCamera>().PlayerSpotted(); //L� c�mara a la que corresponde el foco enviar� una se�al
        }
    }

    // @GRG -----------------------------------------------------
    // Llenar la barra si el jugador est� en el �rea de detecci�n
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
