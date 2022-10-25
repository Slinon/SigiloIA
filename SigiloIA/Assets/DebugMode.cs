using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    private bool _debug = false;
    public GameObject debugCheatSheet;
    public GameObject enemies;
    public GameObject pprocessing;
    KeyManager keys;

    private void Start()
    {
        keys = GetComponent<KeyManager>();
        _debug = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _debug = !_debug;
        }

        if (_debug)
        {
            debugCheatSheet.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.G))
            {
                if (enemies.activeSelf)
                {
                    enemies.SetActive(false);
                }
                else enemies.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.H))
            {
                GuardBehaviour[] guards = FindObjectsOfType<GuardBehaviour>();
                ScientistBehaviour[] scientists = FindObjectsOfType<ScientistBehaviour>();
                CameraBehaviour[] cameras = FindObjectsOfType<CameraBehaviour>();
                LaserBehaviour[] lasers = FindObjectsOfType<LaserBehaviour>();

                foreach (GuardBehaviour guard in guards)
                {
                    guard.state = State.Patrol;
                }

                foreach (ScientistBehaviour scientist in scientists)
                {
                    scientist.state = State.Patrol;
                }

                foreach (CameraBehaviour camera in cameras)
                {
                    camera.state = State.Patrol;
                }

                foreach (LaserBehaviour laser in lasers)
                {
                    laser.state = State.Patrol;
                }
            }

            if (Input.GetKeyDown(KeyCode.F3) || Input.GetKeyDown(KeyCode.J))
            {
                keys.hasRedKey = true;
                keys.hasGreenKey = true;
                keys.hasBlueKey = true;
            }

            if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.K))
            {
                GameStateManager.Instance.currentState = GameState.Victory;
            }

            if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.L))
            {
                if (pprocessing.activeSelf)
                {
                    pprocessing.SetActive(false);
                }
                else pprocessing.SetActive(true);
            }
        }

        else debugCheatSheet.SetActive(false);
    }
}