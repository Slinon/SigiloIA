using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] float rotationAngle;           //Angulo de giro de la cámara
    public float rotationSpeed;                     //Velocidad de giro de la cámara
    public int communicationRange;                  //Alcance para enviar una señal a otros enemigos

    private void Update()
    {
        RotateCamera(); 
    }

    // @GRG ------------------------
    // Metodo para rotar la cámara
    // -----------------------------
    void RotateCamera()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    // @GRG -----------------------------------------------------
    // Metodo para enviar una señal a todos los enemigos en rango
    // ----------------------------------------------------------
    public void PlayerSpotted()
    {
        Debug.Log("Player spotted, communicating with nearby enemies");

        //Se comunica con todos los enemigos (layer 9 = enemies) en un rango determinado
        Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, communicationRange, 9);
        foreach (Collider enemy in enemiesNearby)
        {
            Debug.Log("Enemy nearby found");
        }
    }

    // @GRG ------------------------
    // Debug: communicationRange
    // -----------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, communicationRange);
    }
}
