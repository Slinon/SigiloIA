using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))] // A�ade los componentes requeridos como dependencias
public class PlayerMovement : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera RTSCamera;

    private Transform playerTransform;

    private bool lockedCam = true;

    [SerializeField]
    private float playerSpeed = 5f; // Valor de velocidad movimiento del jugador
    [SerializeField]
    private float playerSprint = 10f; // Valor de velocidad correr del jugador
    [SerializeField]
    private float rotationSpeed = 6f; // Velocidad de rotaci�n del jugador

    private float speed; // Velocidad del jugador

    private CharacterController controller;
    private PlayerInput playerInput; // Acciones del jugador del nuevo Input System
    private Transform camaraTransform;

    private PlayerController playerController;

    private InputAction moveAction; // Acci�n de moverse
    private InputAction sprintAction; // Acci�n de correr
    private InputAction transformAction; // Acci�n de transformarse

    private void Awake()
    {
        speed = playerSpeed; // Inicializa velocidad del jugador

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerTransform = GetComponent<Transform>();

        playerController = GetComponent<PlayerController>();

        camaraTransform = Camera.main.transform; // Inicializa Transform de la c�mara

        moveAction = playerInput.actions["Move"]; // Extrae la acci�n de moverse del InputSystem del jugador (Move)
        sprintAction = playerInput.actions["Sprint"]; // Extrae la acci�n de correr del InputSystem del jugador (Sprint)
        transformAction = playerInput.actions["Transform"]; // Extrae la acci�n de transformarse del InputSystem del jugador (Transform)

        //Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor en el centro de la pantalla
    }

    void Update()
    {
        if (playerController.CheckClosestObject() != null)
        {
            playerController.ShowTButtonText(true);

            if (transformAction.IsPressed())
            {
                playerController.TranformObject();
            }
        }
        else
        {
            playerController.ShowTButtonText(false);
        }
        
        if (moveAction.IsPressed())
        {
            playerController.TransformPlayer();
            MovePlayer();
        }
        else 
        {
            PlayerSoundEffects.instance.isRunning = false;
        }

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            if (lockedCam)
            {
                lockedCam = false;
                playerCamera.gameObject.SetActive(true);
                RTSCamera.gameObject.SetActive(false);
            }
            else
            {
                RTSCamera.transform.position = new Vector3(playerTransform.position.x, 27f, playerTransform.position.z);

                lockedCam = true;
                playerCamera.gameObject.SetActive(false);
                RTSCamera.gameObject.SetActive(true);
            }
        }
    }

    // @EMF -----------------------
    // M�todo para mover al jugador
    // ----------------------------

    void MovePlayer()
    {
        // Comprueba si est� corriendo para asignar la velocidad correspondiente
        if (sprintAction.IsPressed())
        {
            speed = playerSprint;
            PlayerSoundEffects.instance.isRunning = true;
        }
        else
        {
            speed = playerSpeed;
            PlayerSoundEffects.instance.isRunning = false;
        }

        // Obtiene la direcci�n de movimiento dada por el Input del jugador (WASD) horizaontal/vertical
        Vector3 move = new Vector3(moveAction.ReadValue<Vector2>().x, 0f, moveAction.ReadValue<Vector2>().y);

        // Modifica la direcci�n de movimiento del jugador para que vaya en direcci�n de la c�mara (sin la altura: y = 0)
        //move = move.x * camaraTransform.right.normalized + move.z * camaraTransform.forward.normalized;
        //move.y = 0f;

        // Mueve el jugador a la velocidad correspondiente
        controller.Move(move * speed * Time.deltaTime);

        // Rota el jugador en direcci�n de la c�mara
        //Quaternion targetRotation = Quaternion.Euler(0f, camaraTransform.eulerAngles.y, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}