using UnityEngine;
using UnityEngine.SceneManagement;

// Va en la escena del menú. Cada botón carga un prototipo por su nombre de escena
public class MenuPrototipos : MonoBehaviour
{
    private void Start()
    {
        // Al volver de un prototipo el ratón viene bloqueado. En el menú hay que poder usarlo.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Cargar(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void Salir()
    {
        Application.Quit(); // Solo se hace algo en el ejecutable, en el editor no
    }
}
