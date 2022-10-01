using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // @GRG ------------------------
    // Manager auxiliar para pruebas
    // -----------------------------

    [SerializeField] private int securityLevel = 0;
    private bool firstTimeSpotted = true;
    [SerializeField] Material[] spotlightMaterials;

    // @GRG ----------------------------------------
    // Qué hacer cuando el jugador ha sido detectado
    // ---------------------------------------------
    public void PlayerSpotted()
    {
        securityLevel += 1;

        if (firstTimeSpotted)
        {
            firstTimeSpotted = false;
            UpdateCameras();
        }    
    }

    // @GRG ------------------------------
    // Actualizar las camaras de seguridad
    // -----------------------------------
    void UpdateCameras()
    {
        SecurityCamera[] securityCameras = FindObjectsOfType<SecurityCamera>();                     //Buscar todas las cámaras de seguridad

        foreach (SecurityCamera securityCamera in securityCameras)                                  //Cambiar el color del foco.
        {
            securityCamera.thisCameraSpotlight.GetComponent<MeshRenderer>().material = spotlightMaterials[1];
        }
    }
}
