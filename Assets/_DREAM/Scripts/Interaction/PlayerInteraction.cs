using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private TMP_Text interactionPrompt;
    [SerializeField] private ItemCard itemCard;
    [SerializeField] private PlayerMovement playerMovement;

    private Interactable currentInteractable;
    private Interactable previousInteractable;

    private void LateUpdate()
    {
        // Con la ficha abierta el juego se detiene y solo escuchamos para cerrarla
        if (itemCard != null && itemCard.IsOpen)
        {
            currentInteractable = null;
            UpdatePrompt();

            if (Keyboard.current != null && (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame))
            {
                CloseCard();
            }
            return;
        }

        currentInteractable = FindInteractable();
        UpdatePrompt();
        
        if (currentInteractable == null) return;

        if (Cursor.lockState == CursorLockMode.Locked && Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();

            // Solo las notas, el manual y similares tienen imagen. 
            if (currentInteractable.CardImage != null)
            {
                OpenCard(currentInteractable);
            }
        }
    }

    private void OpenCard(Interactable item)
    {
        if (itemCard == null) return;

        itemCard.Show(item.CardImage);

        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Deja de leer las teclas y el raton
        }
    }

    private void CloseCard()
    {
        itemCard.Hide();

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private Interactable FindInteractable()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // Solo nos interesan los objetos interactuables
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                return interactable;
            }
        }
        return null;
    }

    private void UpdatePrompt()
    {
        if (interactionPrompt == null) return;

        // Si el personaje sigue mirando lo mismo que el frame anterior, no hay nada que hacer
        if (currentInteractable == previousInteractable) return;

        previousInteractable = currentInteractable;

        bool show = currentInteractable != null;

        if (show)
        {
            interactionPrompt.text = "Pulsa E para " + currentInteractable.Verb;
        }
        interactionPrompt.gameObject.SetActive(show);
    }

    // Al desactivarlo (por ejemplo, con el panel del maletero abierto) se quita el aviso de la pantalla
    private void OnDisable()
    {
        previousInteractable = null;
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }
}
