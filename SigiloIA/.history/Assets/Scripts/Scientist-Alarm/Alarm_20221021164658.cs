using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{

    public bool alarmaFuncional;
    public Transform player;
    private ScientistBehaviour[] cientificoingame;
    [SerializeField] private GameObject Button;
    [SerializeField] private Material ButtonOnMat;
    [SerializeField] private Material ButtonOffMat;


    private void Start()
    {  
        alarmaFuncional= true;
        cientificoingame = GameObject.FindObjectsOfType<ScientistBehaviour>();
        
    }

    public void Update()
    {  
        
        ActivarAlarma();

    }

    public void ActivarAlarma()
    {
        if(alarmaFuncional)
        {
            foreach (ScientistBehaviour cientifico in cientificoingame)
            {

                if (cientifico.state == State.Chase && (Vector3.Distance(transform.position, cientifico.transform.position) < cientifico.stoppingDistance))
                {

                    AIManager.Instance.CallAllGuards(player.position);

                }
            }
        }
    }


    public void DesactivarAlarma()
    {
        if(alarmaFuncional)
        {
            alarmaFuncional=false;
            Button.GetComponent<Mes>

        }else
        {
            alarmaFuncional=true;
        }
    }


}
