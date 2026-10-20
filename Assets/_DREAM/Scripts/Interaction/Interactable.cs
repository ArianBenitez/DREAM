using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Aviso")]
    [SerializeField] private string verb = "interactuar"; // lo que se lee en el aviso

    [Header("Ficha")]
    [SerializeField] private string displayName = "Objeto";
    [TextArea(2, 4)]
    [SerializeField] private string description = "";
    [SerializeField] private Sprite icon;

    public string Verb => verb;
    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;
    
    public void Interact()
    {
        Debug.Log("Has interactuado con: " + gameObject.name);
    }
}
