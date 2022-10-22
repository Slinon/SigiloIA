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
    [SerializeField] GameObject ripple;                    //Efecto de comunicaci�n con otros NPC
    [SerializeField] Transform POI;                        //Punto de inter�s al que llamar a los guardias
    PlayerController player;

    [Header("Camera attributes")]
    [SerializeField] private float rotationAngle;           //Barrido de la c�mara (en grados)
    public float rotationSpeed;                             //Velocidad de giro de la c�mara
    [Range(0,20)] public float communicationRange;          //Alcance para la comunicaci�n con otros NPC
    private float from;                                     //Angulo inicial
    private float to;                                       //Angulo final
    public float speedMultiplier;                           //Multiplicador de velocidad
    public float rangeMultiplier;                           //Multiplicador de rango de comunicaci�n 

    // @GRG ---------------------------
    // Start: Inicializaci�n de variables
    // --------------------------------
    void Start()
    {
        from = transform.rotation.eulerAngles.y - rotationAngle / 2;
        to = transform.rotation.eulerAngles.y + rotationAngle / 2;
        player = FindObjectOfType<PlayerController>();

        //if (from < 0) from = 360 + from;
        //if (to > 360) to = to - 360;
    }

    // @GRG ---------------------------
    // Update: se llama una vez por frame
    // --------------------------------
    void Update()
    {
        if (state != State.Chase) RotateCamera();
    }

    // @GRG ---------------------------
    // Metodo para rotar la c�mara
    // --------------------------------
    void RotateCamera()
    {
        //Obtener angulo actual
        float currentAngle = transform.rotation.eulerAngles.y;

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //ESTO ES F�TIDO LO TENGO QUE CAMBIAR
        //PERDON POR TENER EL CEREBRO TAMA�O ALMENDRA
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Cambiar el sentido de la rotaci�n si se sobrepasa el �ngulo inicial o final
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

        //Rotar la c�mara
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    // @GRG ---------------------------
    // Avisar a los NPC en rango
    // --------------------------------
    public void PlayerSpotted()
    {
        if (state != State.Chase)
        {
            ChangeState();
        }

        //Reproducir effecto
        if (state == State.Search)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.yellow, communicationRange);
            AIManager.Instance.CallCameras(transform.position + new Vector3 (0, -5, 0), communicationRange);
            AIManager.Instance.CallGuard(transform.position + new Vector3(0, -5, 0), communicationRange, POI.position);
        }

        if (state == State.Chase)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.red, communicationRange);
            AIManager.Instance.CallAllGuards(player.transform.position);
        }       
    }

    public void ChangeState()
    {
        state += 1;
        rotationSpeed *= speedMultiplier;
        communicationRange *= rangeMultiplier;
    }

    // @GRG ---------------------------
    // Visualizar el alcance de comunicaci�n
    // --------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, communicationRange);
    }
}
