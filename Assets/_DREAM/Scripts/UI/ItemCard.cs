using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    public bool IsOpen => gameObject.activeSelf;

    public void Show(Interactable item)
    {
        nameText.text = item.DisplayName;
        descriptionText.text = item.Description;

        iconImage.sprite = item.Icon;
        iconImage.enabled = item.Icon != null; // Si no hay imagen, no dejamos un cuadro blanco

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
