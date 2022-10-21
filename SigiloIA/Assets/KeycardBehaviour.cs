using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardBehaviour : MonoBehaviour
{
    public EntityColor keyColor;
    public float speed;
    public ParticleSystem particles;
    public MeshRenderer cardMesh;
    KeyManager keyManager;

    private void Start()
    {
        keyManager = FindObjectOfType<KeyManager>();
        var main = particles.main;

        if (keyColor is EntityColor.RED)
        {
            main.startColor = Color.red;
            cardMesh.material.color = Color.red;
        }

        if (keyColor is EntityColor.GREEN)
        {
            main.startColor = Color.green;
            cardMesh.material.color = Color.green;
        }

        if (keyColor is EntityColor.BLUE)
        {
            main.startColor = Color.blue;
            cardMesh.material.color = Color.blue;
        }
    }

    void Update()
    {
        //Dar vueltecitas
        transform.Rotate(0, speed * Time.deltaTime, 0) ;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            if (keyColor is EntityColor.RED)
            {
                keyManager.hasRedKey = true;
            }

            if (keyColor is EntityColor.GREEN)
            {
                keyManager.hasGreenKey = true;
            }

            if (keyColor is EntityColor.BLUE)
            {
                keyManager.hasBlueKey = true;
            }

            Destroy(gameObject);
        }
    }
}
