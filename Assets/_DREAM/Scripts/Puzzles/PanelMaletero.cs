using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Va en PanelMaletero. Es el candado del maletero, las flechas cambian el símbolo de cada
// casilla y Presiona para abrir comprueba la combinación.
public class PanelMaletero : MonoBehaviour
{
    [SerializeField] Image[] casillas;                // El Simbolo de cada casilla, de izq a der
    [SerializeField] Sprite[] simbolos;               // Los cinco símbolos, en el orden de los archivos
    [SerializeField] int[] combinacion = { 3, 4, 2 }; // La combinación correcta: Reloj, Velero, Flor
    [SerializeField] GameObject tapa;                 // El cubo que tapa la llave
    [SerializeField] TMP_Text mensaje;                // El texto que avisa los fallos y el bloqueo
    [SerializeField] PlayerMovement jugador;          // Se apaga mientras el candado está abierto
    [SerializeField] PlayerInteraction interaccion;   
    [SerializeField] float esperaBloqueo = 10f;       // El tiempo de espera cuando se bloquea

    int[] elegidos = new int[3]; // El símbolo que aparece en cada casilla, empiezan en el Oso (posición 0)
    int fallos;
    float bloqueadoHasta;        // Segundo del juego en el que termina el bloqueo
    string textoPlaca;           // El texto original del aviso, para volver a ponerlo

    // Se ejecuta la primera vez que se activa el panel del candado
    void Awake()
    {
        textoPlaca = mensaje.text;
        Pintar();
    }

    // La llama la tapa del maletero al pulsar E
    public void Abrir()
    {
        if (!tapa.activeSelf) return; // Ya está abierto
        gameObject.SetActive(true);
        jugador.enabled = false;
        interaccion.enabled = false;
        Cursor.lockState = CursorLockMode.None; // Muestra el ratón para las flechas
        Cursor.visible = true;
    }

    public void Cerrar()
    {
        gameObject.SetActive(false);
        jugador.enabled = true;
        interaccion.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Solo se ejecuta con el panel del candado abierto
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Cerrar();

        // Cuando pasa el tiempo de bloqueo se puede volver a probar
        if (bloqueadoHasta > 0 && Time.time >= bloqueadoHasta)
        {
            bloqueadoHasta = 0;
            fallos = 0;
            mensaje.text = textoPlaca;
        }
    }

    // Las flechas. El número es la casilla: 0, 1 o 2
    public void Subir(int casilla) => Mover(casilla, 1);
    public void Bajar(int casilla) => Mover(casilla, -1);

    void Mover(int casilla, int paso)
    {
        // Al pasar del último símbolo vuelve al primero y al revés
        elegidos[casilla] = (elegidos[casilla] + paso + simbolos.Length) % simbolos.Length;
        Pintar();
    }

    // Pone en cada casilla el sprite del símbolo elegido
    void Pintar()
    {
        for (int i = 0; i < casillas.Length; i++)
            casillas[i].sprite = simbolos[elegidos[i]];
    }

    // Presiona para abrir
    public void Comprobar()
    {
        if (bloqueadoHasta > 0) return;

        // Al primer símbolo que no coincide cuenta un fallo y deja de mirar
        for (int i = 0; i < combinacion.Length; i++)
        {
            if (elegidos[i] != combinacion[i])
            {
                Fallo();
                return;
            }
        }
        tapa.SetActive(false); // Se desactiva la tapa y se muestra la llave
        Cerrar();
    }

    void Fallo()
    {
        fallos++;
        if (fallos < 3)
        {
            mensaje.text = "Código incorrecto (" + fallos + "/3)";
            return;
        }
        // Se guarda cuándo termina el bloqueo y Update lo quita al llegar a ese momento
        bloqueadoHasta = Time.time + esperaBloqueo;
        mensaje.text = "Sistema bloqueado. Espera " + esperaBloqueo + " segundos";
    }
}
