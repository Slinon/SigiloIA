using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // Lógica del enemigo LASER
    // --------------------------------

    [Header("General")]
    [TextArea] public string description;                  //Breve descripción del código para otros programadores
    public State state;                                    //Estado del enemigo
    [SerializeField] GameObject ripple;                    //Efecto de comunicación con otros NPC
    [SerializeField] BoxCollider col;                      //Collider del objeto

    [Header("Laser attributes")]
    [Range(0, 20)] public float communicationRange;         //Alcance para la comunicación con otros NPC
    public float onOffSpeed;                                //tiempo entre tarda en cambiar de encendido a apagado
    public float onOffReduction;                            //Reduccion del tiempo que tarda en cambiar de encendido a apagado
    public float rangeMultiplier;                           //Multiplicador de rango de comunicación
    private bool on = true;


    [Header("State colors")]
    [SerializeField] private Color patrolColor;             //Color cuando el enemigo está patrullando
    [SerializeField] private Color searchColor;             //Color cuando el enemigo está investigando
    [SerializeField] private Color chaseColor;              //Color cuando el enemigo está persiguiendo

    public ParticleSystem[] lasers;                        //Lasers

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor();
    }

    // @GRG ---------------------------
    // Cambio de color de cada laser
    // --------------------------------
    void UpdateColor()
    { 
        for (int i = 0; i < lasers.Length; i++)
        {
            var main = lasers[i].main;

            if (state == State.Patrol)
            {
                main.startColor = patrolColor;
            }

            if (state == State.Search)
            {
                main.startColor = searchColor;
            }

            if (state == State.Chase)
            {
                main.startColor = chaseColor;
            }
        }       
    }

    // @GRG ---------------------------
    // Avisar a los NPC en rango
    // --------------------------------
    public void PlayerSpotted()
    {
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
        onOffSpeed -= onOffReduction;
        communicationRange *= rangeMultiplier;

    }

    // @GRG ---------------------------
    // Comprobar si el jugador ha entrado en la zona de detección
    // --------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            PlayerSpotted();
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
