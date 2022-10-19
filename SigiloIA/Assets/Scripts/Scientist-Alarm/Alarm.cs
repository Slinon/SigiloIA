using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{

    public bool alarmaFuncional;
    public Transform player;

    private ScientistBehaviour[] cientificoingame;




    private void Start()
    {  
        cientificoingame = GameObject.FindObjectsOfType<ScientistBehaviour>();
        
    }

    public void Update()
    {  
        ActivarAlarma();

    }

    public void ActivarAlarma()
    {
        foreach (ScientistBehaviour cientifico in cientificoingame)
        {

            if (cientifico.state == State.Chase && (Vector3.Distance(transform.position, cientifico.transform.position) < cientifico.stoppingDistance))
            {

                if(alarmaFuncional)
                {
                    AIManager.Instance.CallAllGuards(player.position);
                }

            }
        }
    }

}
