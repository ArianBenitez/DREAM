using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Va en el HUD. Guarda las herramientas que puede llevar el jugador y pinta el anillo de la esquina.
// Un tramo del anillo por herramienta, la activa en amarillo y con su sprite en el centro. 
// Con Q se pasa a la siguiente.
public class Inventario : MonoBehaviour
{
    [SerializeField] private GameObject anillo; // El anillo que forma el inventario
    [SerializeField] private Image tramoBase;   // El tramo base que se copia una vez por herramienta
    [SerializeField] private Image icono;       // El icono en el centro
    [SerializeField] private Color colorActivo = new Color(0.95f, 0.75f, 0.2f);    // Amarillo
    [SerializeField] private Color colorInactivo = new Color(0.55f, 0.55f, 0.55f); // Gris
    [SerializeField] private float hueco = 6f;  // Separación entre los tramos cuando hay 2 o más objetos
    [SerializeField] private int maximo = 6;    // Máx de objetos en el inventario

    private readonly List<Sprite> herramientas = new List<Sprite>(); // Los iconos de las herramientas que llevamos
    private readonly List<Image> tramos = new List<Image>();
    private int activa; // Posición en la lista de la herramienta activa

    private void Start()
    {
        tramoBase.gameObject.SetActive(false);
        Pintar();
    }

    private void Update()
    {
        // Con 0 o 1 herramientas no hay nada a lo que cambiar
        if (herramientas.Count < 2) return;

        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            // Pasa a la siguiente. El % hace que después de la última vuelva a la 0
            activa = (activa + 1) % herramientas.Count;
            Pintar();
        }
    }

    // La llama el objeto al cogerlo, desde el On Interact del Inspector
    public void Coger(Sprite sprite)
    {
        if (herramientas.Count >= maximo) return;
        herramientas.Add(sprite);
        activa = herramientas.Count - 1; // La recién cogida pasa a ser la activa
        Pintar();
    }

    // Rehace el anillo entero según lo que lleva el jugador equipado
    // Se llama al empezar, al coger algo y al pulsar Q
    private void Pintar()
    {
        // 1. Borrar los tramos que hubiera de antes (es más fácil rehacerlo que ir cambiando tramos sueltos)
        foreach (Image t in tramos) Destroy(t.gameObject);
        tramos.Clear();

        // 2. Si no llevamos nada, esconder el anillo y salir
        int n = herramientas.Count;
        anillo.SetActive(n > 0); // Sin herramientas no se ve el anillo
        if (n == 0) return;

        // 3. Repartir la vuelta entera entre las herramientas
        float grados = 360f / n;
        float separacion = n > 1 ? hueco : 0f; // Con una sola herramienta el anillo va entero

        // 4. Hacer un tramo por herramienta
        for (int i = 0; i < n; i++)
        {
            // Copia de la forma del anillo
            Image t = Instantiate(tramoBase, anillo.transform);
            t.gameObject.SetActive(true);  // El tramo base está apagado, así que la copia también
            t.fillAmount = (grados - separacion) / 360f;  // Lo que ocupa el tramo, en fracción de vuelta
            // Todas las copias empiezan a rellenarse desde arriba, una encima de otra. 
            // Se gira cada una para ponerla en su sitio
            t.rectTransform.localEulerAngles = new Vector3(0, 0, -grados * i - separacion / 2f);
            t.color = i == activa ? colorActivo : colorInactivo;
            tramos.Add(t);
        }
        // 5. Poner en el centro el icono de la herramienta activa
        icono.sprite = herramientas[activa];
    }
}
