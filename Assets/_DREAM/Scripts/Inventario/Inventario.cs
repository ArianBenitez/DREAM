using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Va en el HUD. Guarda las herramientas que puede llevar el jugador y pinta la rueda del inventario como la rueda de un teléfono
// antiguo.
// Con Q la rueda gira para pasar a la siguiente.
public class Inventario : MonoBehaviour
{
    [SerializeField] private GameObject rueda;            // La rueda que representa el inventario
    [SerializeField] private RectTransform casillas;      // Guarda las copias de las casillas
    [SerializeField] private RectTransform casillaBase;   // La casilla que contiene cada objeto en el inventario, se copia para cada objeto
    [SerializeField] private Image icono;                 // El icono grande del centro que representa el objeto seleccionado
    [SerializeField] private float radio = 0.385f;        // Distancia de cada casilla al centro del inventario
    [SerializeField] private float anchoCasilla = 0.179f; // Ancho de la casilla
    [SerializeField] private float anguloTope = 140f;     // Dónde está el tope, en grados desde arriba
    [SerializeField] private float velocidadGiro = 540f;  // Grados por segundo
    [SerializeField] private int maximo = 6;              // número máx de objetos en el inventario

    private readonly List<Sprite> herramientas = new List<Sprite>(); // Los iconos de las herramientas que llevamos
    private readonly List<RectTransform> copias = new List<RectTransform>(); // Las casillas de la rueda
    private int activa; // Posición en la lista de la herramienta activa
    private float giro; // Lo girada que está la rueda en ese momento
    private float giroObjetivo; // Lo girada que tiene que acabar

    private void Start()
    {
        casillaBase.gameObject.SetActive(false);
        Pintar();
    }

    private void Update()
    {
        // Mientras no haya llegado a su posición objetivo, la rueda avanza un poco cada frame
        if (giro != giroObjetivo)
        {
            giro = Mathf.MoveTowards(giro, giroObjetivo, velocidadGiro * Time.deltaTime);
            Colocar();    
        }
        
        // Con 0 herramientas el % daría error y con 1 no hay a cuál cambiar
        if (herramientas.Count < 2) return;

        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            // Pasa a la siguiente. El % hace que después de la última vuelva a la 0
            activa = (activa + 1) % herramientas.Count;
            giroObjetivo += 360f / herramientas.Count; // Siempre suma, así la rueda gira siempre hacia el mismo lado
            icono.sprite = herramientas[activa];
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

    // Rehace las casillas según lo que lleva el jugador equipado
    // Se llama al empezar, al coger algo
    private void Pintar()
    {
        // 1. Borrar las casillas de antes (es más fácil rehacerlas que ir cambiándolas sueltas)
        foreach (RectTransform c in copias) Destroy(c.gameObject);
        copias.Clear();

        // 2. Si no llevamos nada, esconder el inventario y salir
        int n = herramientas.Count;
        rueda.SetActive(n > 0); 
        if (n == 0) return;

        // 3. Hacer una casilla por objeto, del tamaño que toca según la rueda
        float ancho = casillas.rect.width * anchoCasilla;
        for (int i = 0; i < n; i++)
        {
            RectTransform c = Instantiate(casillaBase, casillas);
            c.gameObject.SetActive(true);  // La rueda base está apagado, así que la copia también
            c.sizeDelta = new Vector2(ancho, ancho);
            c.GetChild(0).GetComponent<Image>().sprite = herramientas[i];
            copias.Add(c);
        }

        // 4. Girar la rueda de golpe hasta dejar la activa debajo del tope
        giroObjetivo = 360f / n * activa;
        giro = giroObjetivo;
        Colocar();

        // 5. Poner en el centro el icono de la herramienta activa
        icono.sprite = herramientas[activa];
    }

    // Pone a cada casilla en su punto del círculo según lo girada que esté la rueda
    private void Colocar ()
    {
        int n = copias.Count;
        float r = casillas.rect.width * radio;
        for (int i = 0; i < n; i++)
        {
            // Empieza en el tope, cada casilla va un trozo de vuelta más atrás y el giro las mueve a todas
            float angulo = (anguloTope - 360f / n * i + giro) * Mathf.Deg2Rad;
            // Con el ángulo medido desde arriba, el seno da lo que va a la derecha y el coseno lo que sube
            copias[i].anchoredPosition = new Vector2(Mathf.Sin(angulo) * r, Mathf.Cos(angulo) * r);
        }
    }
}