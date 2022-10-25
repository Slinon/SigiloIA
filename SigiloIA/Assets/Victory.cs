using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            GameStateManager.Instance.currentState = GameState.Victory;
        }
    }
}
