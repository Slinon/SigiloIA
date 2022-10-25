using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    private bool _debug = false;
    public GameObject debugCheatSheet;
    public GameObject enemies;
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

            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (enemies.activeSelf)
                {
                    enemies.SetActive(false);
                }
                else enemies.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F2))
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

            if (Input.GetKeyDown(KeyCode.F3))
            {
                keys.hasRedKey = true;
                keys.hasGreenKey = true;
                keys.hasBlueKey = true;
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                GameStateManager.Instance.currentState = GameState.Victory;
            }
        }

        else debugCheatSheet.SetActive(false);
    }
}
