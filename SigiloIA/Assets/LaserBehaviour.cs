using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    // @GRG ---------------------------
    // L�gica del enemigo LASER
    // --------------------------------

    [Header("General")]
    [TextArea] public string description;                  //Breve descripci�n del c�digo para otros programadores
    public State state;                                    //Estado del enemigo
    [SerializeField] GameObject ripple;                    //Efecto de comunicaci�n con otros NPC
    [SerializeField] BoxCollider col;                      //Collider del objeto

    [Header("Laser attributes")]
    [Range(0, 20)] public float communicationRange;         //Alcance para la comunicaci�n con otros NPC
    public float onOffSpeed;                                //tiempo entre tarda en cambiar de encendido a apagado
    public float onOffReduction;                            //Reduccion del tiempo que tarda en cambiar de encendido a apagado
    public float rangeMultiplier;                           //Multiplicador de rango de comunicaci�n
    private float timer;


    [Header("State colors")]
    [SerializeField] private Color patrolColor;             //Color cuando el enemigo est� patrullando
    [SerializeField] private Color searchColor;             //Color cuando el enemigo est� investigando
    [SerializeField] private Color chaseColor;              //Color cuando el enemigo est� persiguiendo

    public ParticleSystem[] lasers;                        //Lasers

    private void Start()
    {
        timer = onOffSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            col.enabled = !col.enabled;

            timer = onOffSpeed;

            for (int i = 0; i < lasers.Length; i++)
            {
                if (col.enabled)
                {
                    lasers[i].Play();
                }

                else
                {
                    lasers[i].Stop();
                }
            }
        }
    }

    // @GRG ---------------------------
    // Cambio de color de cada laser
    // --------------------------------
    void UpdateColor()
    { 
        for (int i = 0; i < lasers.Length; i++)
        {
            var laser = lasers[i].main;

            if (state == State.Patrol)
            {
                laser.startColor = patrolColor;
            }

            if (state == State.Search)
            {
                laser.startColor = searchColor;
            }

            if (state == State.Chase)
            {
                laser.startColor = chaseColor;
            }
        }       
    }

    // @GRG ---------------------------
    // Avisar a los NPC en rango
    // --------------------------------
    public void PlayerSpotted()
    {

        if (state != State.Chase)
        {
            state += 1;
            onOffSpeed -= onOffReduction;
            communicationRange *= rangeMultiplier;
        }

        UpdateColor();

        //Reproducir effecto
        if (state == State.Search)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.yellow, communicationRange);
        }

        if (state == State.Chase)
        {
            ripple.GetComponent<RippleEffect>().PlayRipple(Color.red, communicationRange);
        }

        //Crear una esfera con origen en el enmigo c�mara y radio el rango de comunicaci�n
        Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, communicationRange, 1 << 9);

        //Por cada enemigo en dicha esfera
        foreach (Collider enemy in enemiesNearby)
        {
            Debug.Log("Enemy nearby found");
        }

    }

    // @GRG ---------------------------
    // Comprobar si el jugador ha entrado en la zona de detecci�n
    // --------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            Debug.Log("Player spotted");
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
