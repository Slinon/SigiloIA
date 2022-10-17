using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    // @GRG ---------------------------
    // Lógica del efecto de "sonido"
    // para la comunicación entre NPCs
    // --------------------------------

    private ParticleSystem ripple;                  //Efecto de comunicación con otros NPC

    private void Start()
    {
        ripple = gameObject.GetComponent<ParticleSystem>();
    }


    // @GRG ---------------------------
    // Ajustar los valores y reproducir
    // --------------------------------

    public void PlayRipple(Color rippleColor, float rippleSize)
    {
        var main = ripple.main;

        main.startSize = rippleSize * 2;            //Ajustar tamaño a rango de comunicación 
        main.startColor = rippleColor;              //Ajustar color

        ripple.Play();
    }

}
