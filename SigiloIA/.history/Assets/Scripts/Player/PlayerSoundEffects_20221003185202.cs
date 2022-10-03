using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    [SerializeField] private float radius = 4f;
    [SerializeField] private int enemyLayer = 9;
    private int enemyLayerMask;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyLayerMask = (1 << enemyLayer);
    }

    void Update()
    {
        // El jugador está corriendo
        if(playerController.velocity.magnitude >= 8f && audioSource.isPlaying == false)
        {
            // Reproducir los pasos con variaciones aleatorias
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();    

            // Detectar qué enemigos están dentro del radio
            DetectEnemiesNearby(player.transform.position , radius);
        }
    }

    // @VJT --------------------------------------------------------------
    // Metodo para parar detectar qué enemigos están cerca del jugador
    // -------------------------------------------------------------------
    void DetectEnemiesNearby(Vector3 center, float radius)
    {
        // Creamos una esfera que devuelve un array con todos los enemigos con los que colisiona 
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyLayerMask);

        foreach(var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider + " heard you");  

            // Función que tienen los enemigos
            //hitCollider.SendMessage("Alert"); 
            
        }
    }
}
