using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    // @GRG ---------------------------
    // L�gica del efecto de "sonido"
    // para la comunicaci�n entre NPCs
    // --------------------------------

    private ParticleSystem ripple;                  //Efecto de comunicaci�n con otros NPC

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

        main.startSize = rippleSize * 2;            //Ajustar tama�o a rango de comunicaci�n 
        main.startColor = rippleColor;              //Ajustar color

        ripple.Play();
    }

}
