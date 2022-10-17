using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBehaviour : MonoBehaviour
{
    public float stoppingDistance;
    public State state;
    private AIMovement aIMovement;

    public Transform[] ScientistPoints;
    private Vector3 scientistcurrentPoint;
    private int scientistcurrentPointIndex;

    public Transform[] AlarmPoints;
    private Vector3 alarmcurrentPoint;
    private Vector3 aux;
    private int alarmcurrentPointIndex;


    
    void Start()
    {
        state = State.Chase;

        scientistcurrentPointIndex = 0;
        scientistcurrentPoint = ScientistPoints[scientistcurrentPointIndex].position;

        alarmcurrentPointIndex = 0;
        alarmcurrentPoint = AlarmPoints[alarmcurrentPointIndex].position;

        aIMovement = GetComponent<AIMovement>();

        aIMovement.target = scientistcurrentPoint;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                ActivarAlarma();
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

    private void ActivarAlarma()
    {
        
        for(int i=1; i<AlarmPoints.Length; i++)
        {
            aux = AlarmPoints[i].position;
            float posicion1= Vector3.Distance(transform.position, alarmcurrentPoint);
            float posicion2= Vector3.Distance(transform.position, aux);

            if(posicion2<posicion1)
            {
                alarmcurrentPointIndex=i;
                alarmcurrentPoint=AlarmPoints[alarmcurrentPointIndex].position;
            }
        }

        aIMovement.target=alarmcurrentPoint;
    }


}
