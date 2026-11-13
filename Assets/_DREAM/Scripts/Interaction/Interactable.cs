using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    [Header("Aviso")]
    [SerializeField] private string verb = "interactuar"; // lo que se lee en el aviso

    [Header("Interfaz 2D")]
    [FormerlySerializedAs("icon")] // conserva la imagen que ya tenían puesta los objetos de prueba
    [SerializeField] private Sprite cardImage;

    [Header("Al pulsar E")]
    [SerializeField] private UnityEvent onInteract; // lo que hace este objeto, se rellena en el Inspector como el On Click

    public string Verb => verb;
    public Sprite CardImage => cardImage;

    public void Interact()
    {
        Debug.Log("Has interactuado con: " + gameObject.name);
        onInteract.Invoke();
    }
}