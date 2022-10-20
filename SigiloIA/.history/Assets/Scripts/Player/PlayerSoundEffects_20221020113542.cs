using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    public PlayerSoundEffects instance;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    public float radius;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerSoundEffects = this;
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
            DetectEnemiesNearby(transform.position, radius);
        }
    }

    // @VJT --------------------------------------------------------------
    // Metodo para parar detectar qué enemigos están cerca del jugador
    // -------------------------------------------------------------------
    void DetectEnemiesNearby(Vector3 center, float radius)
    {
        AIManager.Instance.CallGuard(center, radius, transform.position);
    }
}
