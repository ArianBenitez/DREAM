using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Va en todo objeto con el que se puede interactuar pulsando E. 
// Guarda lo que se ve en pantalla al acercarte y lo que hace el objeto al pulsar E.
public class Interactable : MonoBehaviour
{
    [Header("Aviso")]
    [SerializeField] private string frase = "interactuar"; // Lo que se lee en al acercarte al objeto

    [Header("Interfaz 2D")]
    [SerializeField] private Sprite cardImage; // Si tiene imagen se abre en grande al pulsar E

    [Header("Al pulsar E")]
    [SerializeField] private UnityEvent onInteract; // Lo que hace este objeto, se rellena en el Inspector

    // Solo lectura
    public string Frase => frase;
    public Sprite CardImage => cardImage;

    // La llama PlayerInteraction cuando el jugador mira este objeto y pulsa E
    public void Interact()
    {
        Debug.Log("Has interactuado con: " + gameObject.name);
        onInteract.Invoke(); // Ejecuta las acciones de On Interact en el inspector
    }
}