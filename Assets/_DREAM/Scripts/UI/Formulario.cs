using UnityEngine;

// Va en Monitor. Enseña las pantallas de una en una y recibe los botones de Siguiente, Atrás y Terminar.
public class Formulario : MonoBehaviour
{
    // En orden: Anuncio, Bienvenida y Preguntas_1 a Preguntas_4
    [SerializeField] GameObject[] pantallas; // en orden: Anuncia, Bienvenida, Preguntas_1 a 4
    int actual;

    void Start() => Mostrar(0);

    // Solo avanza si están respondidas todas las preguntas de la pantalla actual
    public void Siguiente()
    {
        if (PantallaRespondida()) Mostrar(actual + 1);
    }

    // Al volver atrás las respuestas ya dadas se conservan
    public void Atras() => Mostrar(actual - 1);

    // De momento solo escribe las respuestas en la Console para comprobarlas
    public void Terminar()
    {
        if (!PantallaRespondida()) return;
        foreach (Pregunta p in GetComponentsInChildren<Pregunta>(true)) //true incluye pantallas ocultas
            Debug.Log(p.name + ": " + (p.Elegida == 0 ? "A" : "B"));
    }

    // Activa la pantalla pedida y oculta las demás.
    // Clamp evita salirse de la lista en la primera y la última pantalla
    void Mostrar(int indice)
    {
        actual = Mathf.Clamp(indice, 0, pantallas.Length - 1);
        for (int i = 0; i < pantallas.Length; i++)
            pantallas[i].SetActive(i == actual);
    }

    // Busca preguntas sin responder en la pantalla actual.
    // Las pantallas sin preguntas (Anuncio, Bienvenida) siempre dan true
    bool PantallaRespondida()
    {
        foreach (Pregunta p in pantallas[actual].GetComponentsInChildren<Pregunta>())
            if (p.Elegida == -1) return false;
        return true;
    }
}
