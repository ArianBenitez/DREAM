using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Candado con casillas que se cambian con flechas. Sirve para cualquier cerradura del juego
public class PanelCandado : MonoBehaviour
{
    [SerializeField] Image[] casillas;              // La imagen de cada casilla de izq a der
    [SerializeField] Sprite[] opciones;             // Lo que puede salir en cada casilla en orden
    [SerializeField] int[] combinacion;             // La solución
    [SerializeField] TMP_Text mensaje;              // El texto que aparece dependiendo del puzle
    [SerializeField] PlayerMovement jugador;        // Se apagan mientras el candado está abierto
    [SerializeField] PlayerInteraction interaccion; 
    [SerializeField] int fallosParaBloquear = 3;
    [SerializeField] float esperaBloqueo = 10f;     // Segundos de bloqueo
    [SerializeField] UnityEvent alAbrir;            // Lo que pasa cuando aciertas la solución

    int[] elegidos;
    int fallos;
    float bloqueadoHasta;
    string textoEnPantalla;
    bool resuelto;

    void Awake()
    {
        elegidos = new int[casillas.Length];
        textoEnPantalla = mensaje.text;
        Pintar();
    }

    // La llama el objeto con el candado al pulsar E
    public void Abrir()
    {
        if (resuelto) return;
        gameObject.SetActive(true);
        jugador.enabled = false;
        interaccion.enabled = false;
        Cursor.lockState = CursorLockMode.None; // Se habilita el ratón para cambiar las opciones de cada casilla
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

    // Solo se ejecuta con el panel de candado abierto
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Cerrar();

        // Cuando pasa el tiempo de bloqueo se puede volver a probar
        if (bloqueadoHasta > 0 && Time.time >= bloqueadoHasta)
        {
            bloqueadoHasta = 0;
            fallos = 0;
            mensaje.text = textoEnPantalla;
        }
    }
    
    // Las flechas. El número es la casilla, empezando en 0
    public void Subir(int casilla) => Mover(casilla, 1);
    public void Bajar(int casilla) => Mover(casilla, -1);

    void Mover(int casilla, int paso)
    {
        // Al pasar de la última opción vuelve a la primera y viceversa
        // Se suma Length antes del % para que al bajar desde 0 no salga un número negativo
        elegidos[casilla] = (elegidos[casilla] + paso + opciones.Length) % opciones.Length;
        Pintar();
    }

    // Pone en cada casilla la imagen de la opción elegida
    void Pintar()
    {
        for (int i = 0; i < casillas.Length; i++)
            casillas[i].sprite = opciones[elegidos[i]];
    }

    // El botón de comprobar
    public void Comprobar()
    {
        if (bloqueadoHasta > 0) return;

        // Al primer valor que no coincide cuenta un fallo y deja de mirar
        for (int i = 0; i < combinacion.Length; i++)
        {
            if (elegidos[i] != combinacion[i])
            {
                Fallo();
                return;
            }
        }
        resuelto = true;
        alAbrir.Invoke();
        Cerrar();
    }

    void Fallo()
    {
        if (fallosParaBloquear == 0)
        {
            mensaje.text = "Código incorrecto";
            return;
        }

        fallos++;
        if (fallos < fallosParaBloquear)
        {
            mensaje.text = "Código incorrecto (" + fallos + "/" + fallosParaBloquear + ")";
            return;
        }
        // Se guarda cuándo termina el bloqueo, Update lo quita al llegar ese momento
        bloqueadoHasta = Time.time + esperaBloqueo;
        mensaje.text = "Sistema bloqueado. Espera " + esperaBloqueo + " segundos";
    }
}
