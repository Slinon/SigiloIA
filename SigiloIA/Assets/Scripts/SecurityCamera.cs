using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] float rotationAngle;                   //Angulo de giro de la cámara
    public float rotationSpeed;                             //Velocidad de giro de la cámara
    private float originalAngle;
   

    public GameObject thisCameraSpotlight;                  //Foco de la cámara (area de detección)


    private void Start()
    {
        originalAngle = gameObject.transform.eulerAngles.y;
    }

    private void Update()
    {
        RotateCamera(); 
    }

    // @GRG ------------------------
    // Metodo para rotar la cámara
    // -----------------------------
    void RotateCamera()
    {
        transform.localEulerAngles = new Vector3(0, Mathf.PingPong(Time.time * rotationSpeed, rotationAngle) + (originalAngle - rotationAngle / 2), 0);
    }

}
