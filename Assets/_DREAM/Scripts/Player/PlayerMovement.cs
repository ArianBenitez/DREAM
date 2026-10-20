using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2.8f; //metros por segundo
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float maxLookAngle = 85f; //cuanto puede mirar arriba y abajo

    private CharacterController controller;
    private float verticalVelocity;
    private float pitch; //rotacion vertical acumulada de la camara

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; //el raton se queda dentro de la ventana de juego
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();

        //Salir del modo raton capturado mientras probamos en el editor
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Look()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        //El cuerpo del Player gira en horizontal, la camara lo hace en vertical
        transform.Rotate(Vector3.up, mouseDelta.x);

        pitch = Mathf.Clamp(pitch - mouseDelta.y, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        Vector2 input = ReadMoveInput();
        Vector3 direction = transform.right * input.x + transform.forward * input.y;

        //Un ligero empujon hacia abajo para que no vaya dando saltitos
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = direction * walkSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private Vector2 ReadMoveInput()
    {
        Keyboard k = Keyboard.current;
        if (k == null) return Vector2.zero;

        float x = (k.dKey.isPressed ? 1f : 0f) - (k.aKey.isPressed ? 1f : 0f);
        float y = (k.wKey.isPressed ? 1f : 0f) - (k.sKey.isPressed ? 1f : 0f);

        //Normalizamos para que en diagonal no vaya mas rapido
        return new Vector2(x, y).normalized;
    }

}
