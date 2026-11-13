using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Va en PanelMaletero. Es el candado del maletero, las flechas cambian el símbolo de cada
// casilla y Presiona para abrir comprueba la combinación.
public class PanelMaletero : MonoBehaviour
{
    [SerializeField] Image[] casillas; // el Simbolo de cada casilla, de izq a der
    [SerializeField] Sprite[] simbolos; // los cinco, en el orden de los archivos
    [SerializeField] int[] combinacion = { 3, 4, 2 }; // Reloj, Velero, Flor (se cuenta desde 0)
    [SerializeField] GameObject tapa; // el cubo que tapa la llave
    [SerializeField] TMP_Text mensaje;
    [SerializeField] PlayerMovement jugador;
    [SerializeField] PlayerInteraction interaccion;
    [SerializeField] float esperaBloqueo = 10f;

    int[] elegidos = new int[3];
    int fallos;
    float bloqueadoHasta;
    string textoPlaca;

    void Awake()
    {
        textoPlaca = mensaje.text;
        Pintar();
    }

    // La llama la tapa del maletero al pulsar E
    public void Abrir()
    {
        if (!tapa.activeSelf) return; // ya está abierto
        gameObject.SetActive(true);
        jugador.enabled = false;
        interaccion.enabled = false;
        Cursor.lockState = CursorLockMode.None; // hace falta el ratón para las flechas
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

    void Pintar()
    {
        for (int i = 0; i < casillas.Length; i++)
            casillas[i].sprite = simbolos[elegidos[i]];
    }

    // Presiona para abrir
    public void Comprobar()
    {
        if (bloqueadoHasta > 0) return;

        for (int i = 0; i < combinacion.Length; i++)
        {
            if (elegidos[i] != combinacion[i])
            {
                Fallo();
                return;
            }
        }
        tapa.SetActive(false);
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
        bloqueadoHasta = Time.time + esperaBloqueo;
        mensaje.text = "Sistema bloqueado. Espera " + esperaBloqueo + " segundos";
    }
}
