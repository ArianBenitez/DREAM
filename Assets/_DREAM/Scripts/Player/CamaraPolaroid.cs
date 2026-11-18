using UnityEngine;
using UnityEngine.InputSystem;

// Va en la cámara del jugador. Con el clic derecho mantenido se apunta con la polaroid, 
// sale el marco del visor y la cámara dibuja también la capa oculta
public class CamaraPolaroid : MonoBehaviour
{
    [SerializeField] private Camera camara;
    [SerializeField] private LayerMask capaOculta;   // Lo que solo se ve por el visor
    [SerializeField] private GameObject visor;       // El marco del HUD
    [SerializeField] private PlayerMovement jugador; // Si está desactivado no funciona la polaroid

    private bool apuntando;

    private void Start()
    {
        // Al empezar, la cámara no dibuja la capa oculta
        camara.cullingMask &= ~capaOculta.value;
        visor.SetActive(false); 
    }

    private void Update()
    {
        bool clicDerecho = Mouse.current != null && Mouse.current.rightButton.isPressed;
        bool puede = jugador.enabled && Cursor.lockState == CursorLockMode.Locked;
        bool quiereApuntar = clicDerecho && puede;

        // Solo hacemos algo cuando cambia, no en cada frame
        if (quiereApuntar == apuntando) return;
        apuntando = quiereApuntar;

        visor.SetActive(apuntando);

        if (apuntando)
        {
            camara.cullingMask |= capaOculta.value; // añade la capa
        }
        else
        {
            camara.cullingMask &= ~capaOculta.value; // la quita
        }
    }
}
