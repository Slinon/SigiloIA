using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isGrounded " + player.isGrounded);
        //Debug.Log("velocity " + player.velocity.magnitude);
        //Debug.Log("isPlaying" + audioSource.isPlaying); 
        if(player.velocity.magnitude >= 8f && audioSource.isPlaying == false)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();      
        }
    }
}
