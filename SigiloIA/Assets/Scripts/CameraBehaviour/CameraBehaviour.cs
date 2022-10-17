using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // Lógica del enemigo CAMERA
    // --------------------------------

    [Header("General")]
    [TextArea] public string description;                  //Breve descripción del código para otros programadores
    public State state;                                    //Estado del enemigo
    [SerializeField] GameObject ripple;                    //Efecto de comunicación con otros NPC

    [Header("Camera attributes")]
    [SerializeField] private float rotationAngle;           //Barrido de la cámara (en grados)
    public float rotationSpeed;                             //Velocidad de giro de la cámara
    [Range(0,20)] public float communicationRange;          //Alcance para la comunicación con otros NPC
    private float from;                                     //Angulo inicial
    private float to;                                       //Angulo final
    public float speedMultiplier;                           //Multiplicador de velocidad
    public float rangeMultiplier;                           //Multiplicador de rango de comunicación 

    // @GRG ---------------------------
    // Start: Inicialización de variables
    // --------------------------------
    void Start()
    {
        from = transform.rotation.eulerAngles.y - rotationAngle / 2;
        to = transform.rotation.eulerAngles.y + rotationAngle / 2;

        //if (from < 0) from = 360 + from;
        //if (to > 360) to = to - 360;
    }

    // @GRG ---------------------------
    // Update: se llama una vez por frame
    // --------------------------------
    void Update()
    {
        RotateCamera();
    }

    // @GRG ---------------------------
    // Metodo para rotar la cámara
    // --------------------------------
    void RotateCamera()
    {
        //Obtener angulo actual
        float currentAngle = transform.rotation.eulerAngles.y;

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //ESTO ES FÉTIDO LO TENGO QUE CAMBIAR
        //PERDON POR TENER EL CEREBRO TAMAÑO ALMENDRA
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Cambiar el sentido de la rotación si se sobrepasa el ángulo inicial o final
        if (from >= 0 && to >= 0) 
        {
            if (currentAngle < from || currentAngle > to) rotationSpeed *= -1;
        }

        if (from < 0)
        {
            if (currentAngle < 360 + from && currentAngle > to) rotationSpeed *= -1;
        }

        if (to > 360)
        {
            if (currentAngle < from && currentAngle < to - 360) rotationSpeed *= -1;
        }

        //Rotar la cámara
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    // @GRG ---------------------------
    // Avisar a los NPC en rango
    // --------------------------------
    public void PlayerSpotted()
    {

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //ESTO ES POCHO, CADA COSA DEBERIA SER UN
        //METODO PRIVADO Y NO SER ESTO UN POPURRI
        //SON LAS 2 AM LO ARREGLO OTRO DIA XD
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Reproducir effecto
        if (state == State.Search)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.yellow, communicationRange);
        }

        if (state == State.Chase)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.red, communicationRange);
        }

        //Crear una esfera con origen en el enmigo cámara y radio el rango de comunicación
        Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, communicationRange, 1 << 9);

        //Por cada enemigo en dicha esfera
        foreach (Collider enemy in enemiesNearby)
        {
            Debug.Log("Enemy nearby found");
        }

        //Ajustar velocidad y rango de comunicación
        rotationSpeed *= speedMultiplier;
        communicationRange *= rangeMultiplier;

    }

    // @GRG ---------------------------
    // Visualizar el alcance de comunicación
    // --------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, communicationRange);
    }
}
