using UnityEngine;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    [Header("Aviso")]
    [SerializeField] private string verb = "interactuar"; // lo que se lee en el aviso

    [Header("Interfaz 2D")]
    [FormerlySerializedAs("icon")] // conserva la imagen que ya tenían puesta los objetos de prueba
    [SerializeField] private Sprite cardImage;

    public string Verb => verb;
    public Sprite CardImage => cardImage;
    
    public void Interact()
    {
        Debug.Log("Has interactuado con: " + gameObject.name);
    }
}