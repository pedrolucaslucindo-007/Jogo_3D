using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f; // Velocidade de movimento do jogador

    [Header("Pulo")]
    public float jumpHeight = 2f; // Altura do pulo
    public float gravity = -9.81f; // Força da gravidade aplicada ao jogador

    [Header("Mouse")]
    public Transform playerCamera; // Referência para a câmera do jogador
    public float mouseSensitivity = 0.15f; // Sensibilidade do mouse
    public float maxLookAngle = 80f; // Limite de quanto podemos olhar para cima/baixo

    // Referência ao componente Character Controller
    private CharacterController controller;

    // Guarda a velocidade vertical do jogador
    private Vector3 velocity;

    // Guarda o quanto a câmera está inclinada para cima/baixo
    private float cameraRotationX = 0f;

    void Start()
    {
        // Pega o CharacterController que está no mesmo GameObject
        controller = GetComponent<CharacterController>();

        // Prende o mouse no centro da tela
        Cursor.lockState = CursorLockMode.Locked;

        // Esconde o cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Executa o sistema de movimentação
        Movement();

        // Executa o sistema de controle da câmera com o mouse
        MouseLook();
    }

    void Movement()
    {
        // Variáveis que armazenam o movimento horizontal e vertical
        float x = 0f;
        float z = 0f;

        // Se A estiver pressionado, movimenta para a esquerda
        if (Keyboard.current.aKey.isPressed)
            x = -1f;

        // Se D estiver pressionado, movimenta para a direita
        if (Keyboard.current.dKey.isPressed)
            x = 1f;

        // Se W estiver pressionado, movimenta para frente
        if (Keyboard.current.wKey.isPressed)
            z = 1f;

        // Se S estiver pressionado, movimenta para trás
        if (Keyboard.current.sKey.isPressed)
            z = -1f;

        // Cria a direção do movimento baseada na direção
        // que o Player está olhando
        Vector3 move = transform.right * x + transform.forward * z;

        // Impede que o jogador fique mais rápido ao andar na diagonal
        move = Vector3.ClampMagnitude(move, 1f);

        // Move o jogador usando o CharacterController
        // Time.deltaTime deixa a velocidade independente do FPS
        controller.Move(move * speed * Time.deltaTime);

        // Verifica se o jogador está no chão
        if (controller.isGrounded && velocity.y < 0)
        {
            // Mantém o jogador levemente "preso" ao chão
            velocity.y = -2f;
        }

        // Verifica se o jogador apertou espaço neste frame
        // e se ele está no chão
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            // Calcula a velocidade necessária para atingir
            // a altura definida em jumpHeight
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplica a gravidade continuamente
        velocity.y += gravity * Time.deltaTime;

        // Move o jogador verticalmente
        // Isso faz o pulo e a queda funcionarem
        controller.Move(velocity * Time.deltaTime);
    }

    void MouseLook()
    {
        // Pega o movimento do mouse neste frame
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Gira o Player para esquerda/direita
        // O eixo Y controla a rotação horizontal
        transform.Rotate(
            Vector3.up * mouseDelta.x * mouseSensitivity
        );

        // Calcula a rotação vertical da câmera
        // O sinal negativo faz o mouse funcionar de forma natural
        cameraRotationX -= mouseDelta.y * mouseSensitivity;

        // Impede a câmera de girar completamente para trás
        // evitando que ela dê uma volta de 360 graus verticalmente
        cameraRotationX = Mathf.Clamp(
            cameraRotationX,
            -maxLookAngle,
            maxLookAngle
        );

        // Aplica a rotação vertical somente na câmera
        // O Player continua responsável pela rotação horizontal
        playerCamera.localRotation = Quaternion.Euler(
            cameraRotationX,
            0f,
            0f
        );
    }
}