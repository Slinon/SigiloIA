using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // @GRG ------------------------
    // Manager auxiliar para pruebas
    // -----------------------------

    [SerializeField] private int securityLevel = 0;
    [SerializeField] Material[] spotlightMaterials;

    // @GRG ----------------------------------------
    // Qué hacer cuando el jugador ha sido detectado
    // ---------------------------------------------
    public void PlayerSpotted()
    {
        securityLevel += 1;
        UpdateCameras();
 
    }

    // @GRG ------------------------------
    // Actualizar las camaras de seguridad
    // -----------------------------------
    void UpdateCameras()
    {
        SecurityCamera[] securityCameras = FindObjectsOfType<SecurityCamera>();             //Buscar todas las cámaras de seguridad

        foreach (SecurityCamera securityCamera in securityCameras)                          
        {
            if (securityLevel < spotlightMaterials.Length)
            {
                securityCamera.thisCameraSpotlight.GetComponent<MeshRenderer>().material 
                    = spotlightMaterials[securityLevel];                                    //Cambiar el color del foco

                //securityCamera.rotationSpeed *= 1.5f;                                       //Aumentar la velocidad de rotación
            }     
        }
    }
}
