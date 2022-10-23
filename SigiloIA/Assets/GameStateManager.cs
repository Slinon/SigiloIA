using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Play, GameOver}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GameState currentState;
    public GameObject GameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        currentState = GameState.Play;
    }

    private void Update()
    {
        if (currentState == GameState.GameOver)
        {
            GameOverPanel.SetActive(true);
        }
    }
}
