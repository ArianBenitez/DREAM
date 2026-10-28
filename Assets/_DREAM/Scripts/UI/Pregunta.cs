using UnityEngine;
using UnityEngine.UI;

// Va en el texto de cada pregunta del formulario. Apunta qué respuesta ha elegido el jugador y la marca. El botón elegido
// pasa a azul con brillo y crece un poco.
public class Pregunta : MonoBehaviour
{
    // Los dos botones de esta pregunta. Se arrastran desde la Hierarchy.
    [SerializeField] Button respuestaA;
    [SerializeField] Button respuestaB;
    [SerializeField] Sprite elegidaA; // versión azul con brillo
    [SerializeField] Sprite elegidaB;
    [SerializeField] float escalaElegida = 1.05f;

    // -1 sin responder, 0 la A, 1 la B
    // "private set" hace que solo este script pueda cambiarla.
    public int Elegida { get; private set; } = -1;

    // Sprites originales, para devolver el botón a como estaba si se cambia de respuesta
    Sprite normalA, normalB;

    // Se guarda el aspecto inicial de cada botón y se les dice qué hacer al pulsarlos,
    // así no hay que configurar su On Click en el Inspector
    void Awake()
    {
        normalA = respuestaA.image.sprite;
        normalB = respuestaB.image.sprite;
        respuestaA.onClick.AddListener(() => Elegir(0));
        respuestaB.onClick.AddListener(() => Elegir(1));
    }

    // Marca la opción pulsada y desmarca la otra
    // Si se pulsa la que ya estaba elegida, la pregunta se queda sin responder
    void Elegir(int opcion)
    {
        Elegida = opcion == Elegida ? -1 : opcion;
        Marcar(respuestaA, Elegida == 0 ? elegidaA : normalA, Elegida == 0);
        Marcar(respuestaB, Elegida == 1 ? elegidaB : normalB, Elegida == 1);
    }

    void Marcar(Button boton, Sprite sprite, bool elegido)
    {
        boton.image.sprite = sprite;
        boton.image.SetNativeSize(); // el sprite con el brillo es más grande que el normal
        boton.transform.localScale = Vector3.one * (elegido ? escalaElegida : 1f);
    }
}