using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Interfaz 2D de las notas, el manual del ascensor y los demás objetos que se leen.
public class ItemCard : MonoBehaviour
{
    [FormerlySerializedAs("iconImage")]
    [SerializeField] private Image image;

    public bool IsOpen => gameObject.activeSelf;

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
