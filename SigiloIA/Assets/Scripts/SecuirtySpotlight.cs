using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuirtySpotlight : MonoBehaviour
{
    // @GRG ----------------------------------------
    // Comprobar si el jugador ha entrado en la zona
    // ---------------------------------------------

    private TestManager manager;                            //Referencia al GameManager que usaremos.

    private void Start()
    {
        manager = FindObjectOfType<TestManager>();          //Buscar el manager en la escena
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            manager.PlayerSpotted();
        }
    }

}
