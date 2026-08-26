using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;

    [Header("Pulo")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse")]
    public Transform playerCamera;
    public float mouseSensitivity = 0.15f;
    public float maxLookAngle = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraRotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Prende o mouse no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Movement();
        MouseLook();
    }

    void Movement()
    {
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed)
            x = -1f;

        if (Keyboard.current.dKey.isPressed)
            x = 1f;

        if (Keyboard.current.wKey.isPressed)
            z = 1f;

        if (Keyboard.current.sKey.isPressed)
            z = -1f;

        Vector3 move = transform.right * x + transform.forward * z;

        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        // Gravidade
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Pulo
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void MouseLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Esquerda / direita
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

        // Cima / baixo
        cameraRotationX -= mouseDelta.y * mouseSensitivity;

        cameraRotationX = Mathf.Clamp(
            cameraRotationX,
            -maxLookAngle,
            maxLookAngle
        );

        playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}