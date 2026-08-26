using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;

    [Header("Pulo")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Entrada do teclado
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

        // Direção do movimento
        Vector3 move = transform.right * x + transform.forward * z;

        // Evita andar mais rápido na diagonal
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        // Verifica se está no chão
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Pulo
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravidade
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}