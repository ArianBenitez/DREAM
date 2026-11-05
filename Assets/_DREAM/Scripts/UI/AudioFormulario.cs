using UnityEngine;
using UnityEngine.UI;

// Pone la música, el ruido del ordenador viejo, el clic de los botones y el error al intentar avanzar sin responder.
public class AudioFormulario : MonoBehaviour
{
    [SerializeField] AudioSource musica;
    [SerializeField] AudioSource ordenador;
    [SerializeField] AudioSource efectos;
    [SerializeField] AudioClip arranque, zumbido, clic, error;

    void Start()
    {
        musica.Play();
        ordenador.clip = arranque;
        ordenador.Play();
        Invoke(nameof(zumbido), arranque.length);

        // Todos los botones del formulario suenan al pulsarlos.
        foreach (Button boton in GetComponentsInChildren<Button>(true))
            boton.onClick.AddListener(() => efectos.PlayOneShot(clic));
    }

    void Zumbido ()
    {
        ordenador.clip = zumbido;
        ordenador.loop = true;
        ordenador.Play();
    }

    public void Error() => efectos.PlayOneShot(error);
}
