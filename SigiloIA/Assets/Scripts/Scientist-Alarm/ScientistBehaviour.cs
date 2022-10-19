using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBehaviour : MonoBehaviour
{
    public float stoppingDistance;
    public State state;
    private AIMovement aIMovement;
    private bool pulsado;

    public Transform[] ScientistPoints;
    private Vector3 scientistcurrentPoint;
    private int scientistcurrentPointIndex;


    public Transform[] AlarmPoints;
    private Vector3 alarmcurrentPoint;
    private Vector3 aux;
    private Vector3 HuidaPointsite;  
    private int alarmcurrentPointIndex;
    public Transform player;
    public Transform[] HuidaPoint;


    
    void Start()
    {
        state = State.Patrol;

        scientistcurrentPointIndex = 0;
        scientistcurrentPoint = ScientistPoints[scientistcurrentPointIndex].position;

        alarmcurrentPointIndex = 0;
        alarmcurrentPoint = AlarmPoints[alarmcurrentPointIndex].position;

        aIMovement = GetComponent<AIMovement>();
        Alarm alarma=GetComponent<Alarm>();

        aIMovement.target = scientistcurrentPoint;

        pulsado=true;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Search:
                LlamarGuardia();
                break;               
            case State.Chase:
                PulsarBoton();
                break;
        }
    }


    private void Patrol()
    {
        if(Vector3.Distance(transform.position, scientistcurrentPoint) < stoppingDistance)
        {
            scientistcurrentPointIndex++;

             if (scientistcurrentPointIndex == ScientistPoints.Length)
            {

                scientistcurrentPointIndex = 0;

            }

            scientistcurrentPoint = ScientistPoints[scientistcurrentPointIndex].position;

            aIMovement.target = scientistcurrentPoint;

            
        }
    }

    private void LlamarGuardia()
    {
        
        
         
    }

    private void PulsarBoton()
    {
        aIMovement.speed=10f;
        
        for(int i=1; i<AlarmPoints.Length; i++)
        {
            Debug.Log("Iteracion");
            aux = AlarmPoints[i].position;
            float posicion1= Vector3.Distance(transform.position, alarmcurrentPoint);
            float posicion2= Vector3.Distance(transform.position, aux);

            if(posicion2<posicion1)
            {
                alarmcurrentPointIndex=i;
                alarmcurrentPoint= AlarmPoints[alarmcurrentPointIndex].position;
            }
        }
        
        if(pulsado)
        {
            aIMovement.target=alarmcurrentPoint;
        }
        


        if(Vector3.Distance(transform.position, alarmcurrentPoint) < stoppingDistance)
        {
            alarmcurrentPoint = HuidaPoint[0].position;
            aIMovement.target=alarmcurrentPoint;
            pulsado=false;
        }  
        
    }



}
