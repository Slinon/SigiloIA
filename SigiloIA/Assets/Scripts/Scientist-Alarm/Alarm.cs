using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{

    public bool alarmaFuncional;
    public Transform player;

    public ScientistBehaviour[] cientifico;




    private void Start()
    {  
        cientifico = GameObject.FindObjectsOfType<ScientistBehaviour>();
    }

    // Update is called once per frame
    public void Update()
    {  
        foreach (ScientistBehaviour cientifico in cientifico)
        {

            if (cientifico.state == State.Chase && (Vector3.Distance(transform.position, cientifico.transform.position) < cientifico.stoppingDistance))
            {

                ActivarAlarma();

            }
        }

    }

    public void ActivarAlarma()
    {
        if(alarmaFuncional)
        {
            AIManager.Instance.CallAllGuards(player.position);
        }
    }
}
