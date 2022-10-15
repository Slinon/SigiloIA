using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // L�gica del enemigo CAMERA
    // --------------------------------

    [Header("General")]
    [TextArea] public string description;                  //Breve descripci�n del c�digo para otros programadores
    public State state;                                    //Estado del enemigo

    [Header("Camera attributes")]
    [SerializeField] private float rotationAngle;           //Barrido de la c�mara (en grados)
    public float rotationSpeed;                             //Velocidad de giro de la c�mara
    [Range(0,20)] public float communicationRange;          //Alcance para la comunicaci�n con otros NPC
    private float from;                                     //Angulo inicial
    private float to;                                       //Angulo final
    private bool playerSpotted;                             //Booleano para indicar si el jugador ha sido detectado alguna vez

    // @GRG ---------------------------
    // Start: Inicializaci�n de variables
    // --------------------------------
    void Start()
    {
        from = transform.rotation.eulerAngles.y - rotationAngle / 2;
        to = transform.rotation.eulerAngles.y + rotationAngle / 2;

        if (from < 0) from = 360 + from;
        if (to > 360) to = to - 360;
    }

    // @GRG ---------------------------
    // Update: se llama una vez por frame
    // --------------------------------
    void Update()
    {
        RotateCamera();
    }

    // @GRG ---------------------------
    // Metodo para rotar la c�mara
    // --------------------------------
    void RotateCamera()
    {
        //Obtener angulo actual
        float currentAngle = transform.rotation.eulerAngles.y;

        //Cambiar el sentido de la rotaci�n si se sobrepasa el �ngulo inicial o final
        if (currentAngle > to && currentAngle < from)
        {
            rotationSpeed *= -1;
        }

        //Rotar la c�mara
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    // @GRG ---------------------------
    // Avisar a los NPC en rango
    // --------------------------------
    void PlayerSpotted()
    {
        //Si el jugador no ha sido detectado previamente
        if (!playerSpotted)
        {
            //Crear una esfera con origen en el enmigo c�mara y radio el rango de comunicaci�n
            Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, communicationRange, 1 << 9);

            //Por cada enemigo en dicha esfera
            foreach (Collider enemy in enemiesNearby)
            {
                Debug.Log("Enemy nearby found");
            }
        }    
    }

    // @GRG ---------------------------
    // Visualizar el alcance de comunicaci�n
    // --------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, communicationRange);
    }
}
