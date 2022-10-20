using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBehaviour : MonoBehaviour
{
    public float stoppingDistance;
    public State state;
    private AIMovement aIMovement;

    private bool huido;

    public Transform[] ScientistPoints;
    private Vector3 currentPoint;
    private int currentPointIndex;


    public Transform[] AlarmPoints;

    private Vector3 aux;
    private Vector3 HuidaPointsite;  

    public Transform player;
    public Transform[] HuidaPoint;


    
    void Start()
    {
        state = State.Patrol;

        currentPointIndex = 0;
        currentPoint = ScientistPoints[currentPointIndex].position;



        aIMovement = GetComponent<AIMovement>();
        Alarm alarma=GetComponent<Alarm>();

        aIMovement.target = currentPoint;


        huido = false;
        
    }

    // Update is called once per frame
    void Update()
    {  
        if(!huido)
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
        
    }


    private void Patrol()
    {
        if(Vector3.Distance(transform.position, currentPoint) < stoppingDistance)
        {
            currentPointIndex++;

             if (currentPointIndex == ScientistPoints.Length)
            {

                currentPointIndex = 0;

            }

            currentPoint = ScientistPoints[currentPointIndex].position;

            aIMovement.target = currentPoint;

            
        }
    }

    private void LlamarGuardia()
    {
        
        
         
    }

    private void PulsarBoton()
    {
        currentPointIndex = 0;
        currentPoint = AlarmPoints[currentPointIndex].position;
        aIMovement.speed=10f;
        
        for(int i=1; i<AlarmPoints.Length; i++)
        {
            
            aux = AlarmPoints[i].position;
            float posicion1= Vector3.Distance(transform.position, currentPoint);
            float posicion2= Vector3.Distance(transform.position, aux);

            if(posicion2<posicion1)
            {
                currentPointIndex=i;
                currentPoint= AlarmPoints[currentPointIndex].position;
            }
        }
        aIMovement.target=currentPoint;
        if(Vector3.Distance(transform.position, currentPoint) < stoppingDistance)
        {

            AIManager.Instance.CientificosHuir();
        }
        
        
    }

    public void Huir()
    {   
        huido=true;
        aIMovement.speed=10f;
        currentPoint = HuidaPoint[0].position;
        aIMovement.target=currentPoint;

        
    }



}
