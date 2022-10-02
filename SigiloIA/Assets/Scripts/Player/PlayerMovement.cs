using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))] // Añade los componentes requeridos como dependencias
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5f; // Valor de velocidad movimiento del jugador
    [SerializeField]
    private float playerSprint = 10f; // Valor de velocidad correr del jugador
    [SerializeField]
    private float rotationSpeed = 6f; // Velocidad de rotación del jugador

    private float speed; // Velocidad del jugador

    private CharacterController controller;
    private PlayerInput playerInput; // Acciones del jugador del nuevo Input System
    private Transform camaraTransform;

    private InputAction moveAction; // Acción de moverse
    private InputAction sprintAction; // Acción de correr

    private void Awake()
    {
        speed = playerSpeed; // Inicializa velocidad del jugador

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        camaraTransform = Camera.main.transform; // Inicializa Transform de la cámara

        moveAction = playerInput.actions["Move"]; // Extrae la acción de moverse del InputSystem del jugador (Move)
        sprintAction = playerInput.actions["Sprint"]; // Extrae la acción de correr del InputSystem del jugador (Sprint)

        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor en el centro de la pantalla
    }

    void Update()
    {
        MovePlayer();
    }

    // @EMF -----------------------
    // Método para mover al jugador
    // ----------------------------

    void MovePlayer()
    {
        // Comprueba si está corriendo para asignar la velocidad correspondiente
        if (sprintAction.IsPressed())
        {
            speed = playerSprint;
        }
        else
        {
            speed = playerSpeed;
        }

        // Obtiene la dirección de movimiento dada por el Input del jugador (WASD) horizaontal/vertical
        Vector3 move = new Vector3(moveAction.ReadValue<Vector2>().x, 0f, moveAction.ReadValue<Vector2>().y);

        // Modifica la dirección de movimiento del jugador para que vaya en dirección de la cámara (sin la altura: y = 0)
        move = move.x * camaraTransform.right.normalized + move.z * camaraTransform.forward.normalized;
        move.y = 0f;

        // Mueve el jugador a la velocidad correspondiente
        controller.Move(move * speed * Time.deltaTime);

        // Rota el jugador en dirección de la cámara
        Quaternion targetRotation = Quaternion.Euler(0f, camaraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}