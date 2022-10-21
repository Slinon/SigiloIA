using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityColor { RED, GREEN, BLUE }

public class DoorBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // Lógica de las puertas
    // --------------------------------

    [Header("Door attributes")]
    public EntityColor doorColor;
    [SerializeField] MeshRenderer doorMesh;
    [SerializeField] BoxCollider doorTrigger;
    [SerializeField] Animator doorAnim;

    KeyManager keyManager;

    [Header("Colors")]
    public Color colorRed;
    public Color colorGreen;
    public Color colorBlue;

    // @GRG ---------------------------
    // Cambiar color
    // --------------------------------
    void Start()
    {
        keyManager = FindObjectOfType<KeyManager>();

        switch (doorColor)
        {
            case EntityColor.RED:
                doorMesh.material.color = colorRed;
                break;

            case EntityColor.GREEN:
                doorMesh.material.color = colorGreen;
                break;

            case EntityColor.BLUE:
                doorMesh.material.color = colorBlue;
                break;

            default:
                break;
        }
    }

    // @GRG ---------------------------
    // Comprobar si un NPC ha entrado en el trigger
    // --------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Enemy")
        {
            doorAnim.Play("doorOpening");
        }

        //Checkear si tiene la llave correspondiente a la puerta
        if (other.tag is "Player")
        {
            if (doorColor is EntityColor.RED)
            {
                if (keyManager.hasRedKey)
                {
                    doorAnim.Play("doorOpening");
                    this.enabled = false;
                }

                else
                {
                    Debug.Log("You need the Red key");
                }
            }

            if (doorColor is EntityColor.GREEN)
            {
                if (keyManager.hasGreenKey)
                {
                    doorAnim.Play("doorOpening");
                    this.enabled = false;
                }

                else
                {
                    Debug.Log("You need the Green key");
                }
               
            }     

            if (doorColor is EntityColor.BLUE)
            {
                if (keyManager.hasBlueKey)
                {
                    doorAnim.Play("doorOpening");
                    this.enabled = false;
                }

                else
                {
                    Debug.Log("You need the blue key");
                } 
            }
                
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag is "Enemy")
        {
            doorAnim.Play("doorClosing");
        }
    }
}
