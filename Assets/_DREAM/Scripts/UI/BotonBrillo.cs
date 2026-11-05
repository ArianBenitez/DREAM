using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Va en los botones de navegación (Compra, Formulario, flechas y Terminar).
// Al pasar el ratón por encima, el botón brilla y crece un poco, igual que una respuesta elegida.
public class BotonBrillo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite conBrillo; // el PNG de los botones con brillo con el mismo nombre
    [SerializeField] float escala = 1.05f;

    Image imagen;
    Sprite normal;
    Vector3 escalaInicial;

    void Awake()
    {
        imagen = GetComponent<Image>();
        normal = imagen.sprite;
        escalaInicial = transform.localScale;
    }

    // Unity llama a estas dos solas cuando el ratón entra o sale del botón
    public void OnPointerEnter(PointerEventData e) => Poner(conBrillo, escala);
    public void OnPointerExit(PointerEventData e) => Poner(normal, 1f);

    // Al pulsar, la pantalla cambia y el botón se oculta con el ratón encima, así que nunca recibe el OnPointerExit.
    // Se resetea aquí para que no vuelva brillando
    void OnDisable() => Poner(normal, 1f);

    void Poner(Sprite sprite, float s)
    {
        imagen.sprite = sprite;
        imagen.SetNativeSize(); // el sprite con brillo es más grande que el normal
        transform.localScale = escalaInicial * s;
    }
}
