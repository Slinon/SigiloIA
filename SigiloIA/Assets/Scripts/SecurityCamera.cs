using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [TextArea] public string description;

    [SerializeField] float rotationAngle;                   //Angulo de giro de la cámara
    public float rotationSpeed;                             //Velocidad de giro de la cámara
    public int communicationRange;                          //Alcance para enviar una señal a otros enemigos

    private float from;                                     //Angulo inicial
    private float to;                                       //Angulo final


    private void Start()
    {
        from =  transform.rotation.eulerAngles.y - rotationAngle / 2;
        to = transform.rotation.eulerAngles.y + rotationAngle / 2;

        Debug.Log(from + "," + to);
    }

    private void Update()
    {
        RotateCamera(); 
    }

    // @GRG ------------------------
    // Metodo para rotar la cámara
    // -----------------------------
    void RotateCamera()
    {
        //Obtener angulo acutal
        float currentAngle = transform.rotation.eulerAngles.y;

        //Cambiar dirección si el angulo actual sobrepasa
        //el angulo máximo (to) o el mínimo (from)

        ///tiene que existir una formula general para sacar esto, pero
        ///soy un inutil incapaz de deducir cómo sacarlo.
        ///He escrito manualmente los 3 casos posibles:
        ///from y to estan entre 0 y 360
        ///from "se pasa" negativamente
        ///to "se pasa" positivamente

        if (from >= 0 && to >= 0) //caso estandar
        {
            if (currentAngle < from || currentAngle > to)
            {
                rotationSpeed *= -1;
            }
        }
        
        if (from < 0) //caso si from "se pasa"
        {

            if (currentAngle < 360 + from && currentAngle > to)
            {
                rotationSpeed *= -1;
            }
        }

        if (to > 360) //caso si to "se pasa"
        {
            if (currentAngle < from && currentAngle < to - 360)
            {
                rotationSpeed *= -1;
            }
        }

        //Código para rotar
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    // @GRG -----------------------------------------------------
    // Metodo para enviar una señal a todos los enemigos en rango
    // ----------------------------------------------------------
    public void PlayerSpotted()
    {
        Debug.Log("Player spotted, communicating with nearby enemies");

        //Se comunica con todos los enemigos (layer 9 = enemies) en un rango determinado
        Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, communicationRange, 1<<9);

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
        Gizmos.DrawWireSphere(transform.position, communicationRange);
    }
}
