using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [Header("General")]
    [TextArea] public string description;                  //Breve descripción del código para otros programadores
    public State state;                                    //Estado del enemigo
    [SerializeField] GameObject ripple;                    //Efecto de comunicación con otros NPC

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
}
