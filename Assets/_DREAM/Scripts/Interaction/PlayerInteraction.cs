using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Va en la cámara del jugador. Mira qué objeto tiene delante, enseña el texto "Pulsa E para..." y,
// le dice a ese objeto que ejecute lo que tiene en On Interact
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f; // Longitud del rayo de interacción
    [SerializeField] private TMP_Text interactionPrompt;     // El aviso del HUD
    [SerializeField] private ItemCard itemCard;              // La ficha 2D
    [SerializeField] private PlayerMovement playerMovement;  // Cuando abres la ficha de algún objeto se desactiva el movimiento

    private Interactable currentInteractable;  // lo que miras en el fotograma actual
    private Interactable previousInteractable; // lo que mirabas en el anterior fotograma, para no volver a pintar el aviso

    // LateUpdate va después de los Update, así el rayo sale de la cámara cuando ésta ya se ha movido en el fotograma actual
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

        // Solo con el ratón bloqueado, para no interactuar al pulsar fuera del juego en el editor
        if (Cursor.lockState == CursorLockMode.Locked && Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact(); // Ejecuta su On Interact

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

    // Lanza un rayo desde la cámara hacia delante y devuelve el primer objeto con el componente Interactable con el que choca
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
            interactionPrompt.text = "Pulsa E para " + currentInteractable.Frase;
        }
        interactionPrompt.gameObject.SetActive(show);
    }

    // Unity la llama al desactivar el componente. Quita el aviso y olvida lo último que se miraba para repintarlo al volver.
    private void OnDisable()
    {
        previousInteractable = null;
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }
}
