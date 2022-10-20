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

    private GuardBehaviour[] guardsingame;

    private GuardBehaviour guard;

    public Transform[] AlarmPoints;
 
    private Vector3 aux;
    private Vector3 guardiacercano;
    private Vector3 HuidaPointsite;  

    public Transform player;
    public Transform[] HuidaPoint;


    //Colores/Deteccion
    private FieldOfView fieldOfView;
    public FieldOfView FieldOfView
    {
        get { return fieldOfView; }
    }

    public Color patrolColor;
    public Color searchColor;
    public float timeToSearch;
    public Color chaseColor;
    public float timeToChase;

    private bool playerSpotted;
    private float detectionMeter;
    public float timeSearching;

    private float chaseMeter;                       




    
    void Start()
    {
        state = State.Patrol;
        


        currentPointIndex = 0;
        currentPoint = ScientistPoints[currentPointIndex].position;

        guardsingame = GameObject.FindObjectsOfType<GuardBehaviour>();

        aIMovement = GetComponent<AIMovement>();
        


        aIMovement.target = currentPoint;

        fieldOfView = GetComponent<FieldOfView>();
        fieldOfView.meshRenderer.material.color = patrolColor;


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

            SetDetectiometer();
        }

        
        
    }

    //Patrulla

    private void Patrol()
    {
        aIMovement.speed=3f;
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

        DetectPlayer();

        if (detectionMeter > timeToSearch && fieldOfView.player != null && !playerSpotted)
        {

            state=State.Search;
            playerSpotted=true;
        }

        else if (chaseMeter > timeToChase && fieldOfView.player != null && playerSpotted)
        {    

            state=State.Chase;

        }
    }


    //Search
    private void LlamarGuardia()
    {
        aIMovement.speed=6f;


        GuardBehaviour[] guards = GameObject.FindObjectsOfType<GuardBehaviour>();
        float closestGuardDistance = Vector3.Distance(transform.position, guards[0].transform.position);
        GuardBehaviour closestGuard = guards[0];

        for (int i = 1; i < guards.Length; i++)
        {

            if (guards[i].transform.position == transform.position)
            {

                continue;

            }

            if (Vector3.Distance(transform.position, guards[i].transform.position) < closestGuardDistance)
            {

                closestGuardDistance = Vector3.Distance(transform.position, guards[i].transform.position);
                closestGuard = guards[i];

            }

        } 
        
        aIMovement.target=closestGuard.transform.position;

        DetectPlayer();

        AIManager.Instance.CallNearestGuard(transform.position, player.transform.position);
        if (chaseMeter > timeToChase && fieldOfView.player != null && playerSpotted)
        {   
            state=State.Chase;
            
        }
            
    }




    //Chase
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



    //Funciones
    public void Huir()
    {   
        huido=true;
        state = State.Chase;
        fieldOfView.meshRenderer.material.color = chaseColor;
        aIMovement.speed=10f;
        currentPoint = HuidaPoint[0].position;
        aIMovement.target=currentPoint;

        
    }

    public void CalmScientific()
    {

        state = State.Patrol;
        currentPoint = ScientistPoints[currentPointIndex].position;
        aIMovement.target = currentPoint;


    }



    private void SetDetectiometer()
    {

    
        if (state == State.Patrol)
        {

            if (!playerSpotted)
            {

                fieldOfView.meshRenderer.material.color = Color.Lerp(patrolColor, searchColor, detectionMeter / timeToSearch);

            }
            else
            {

                
                fieldOfView.meshRenderer.material.color = Color.Lerp(searchColor, chaseColor, chaseMeter / timeToChase);

            }

        }
        else if (state == State.Search)
        {

            fieldOfView.meshRenderer.material.color = Color.Lerp(searchColor, chaseColor, chaseMeter / timeToChase);

        }
        else
        {

            fieldOfView.meshRenderer.material.color = chaseColor;

        }

    }



    private void DetectPlayer()
    {

        
        if (!playerSpotted)
        {

            
            if (fieldOfView.player != null && detectionMeter < timeToSearch)
            {
                detectionMeter += Time.deltaTime;
            }
            else if (detectionMeter > 0)
            {
                detectionMeter -= Time.deltaTime * 2;
            }

        }
        else
        {
            if (fieldOfView.player != null && chaseMeter < timeToChase)
            {  
                chaseMeter += Time.deltaTime;
            }
            else if (chaseMeter > 0)
            {      
                chaseMeter -= Time.deltaTime * 2;
            }

        }

    }

    

}
