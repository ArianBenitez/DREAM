using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Va en un objeto de la escena del menú. Ese objeto no se borra al cambiar de escena, 
// así que desde cualquier prototipo F1 vuelve al menú.
public class VolverAlMenu : MonoBehaviour
{
    private static bool existe; // Para no crear otro cada vez que se vuelve al menú

    private void Awake()
    {
        if (existe)
        {
            Destroy(gameObject);
            return;
        }
        existe = true;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0); // La escena 0 de Build Profiles es el menú
        }
    }
}
