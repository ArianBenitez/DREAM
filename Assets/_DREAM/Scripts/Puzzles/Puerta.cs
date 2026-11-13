using UnityEngine;

// Va en la puerta de la cabaña. Solo se abre si el jugador ya tiene la llave del maletero.
public class Puerta : MonoBehaviour
{
    public void Abrir()
    {
        if (Llave.Recogida) gameObject.SetActive(false);
        else Debug.Log("La puerta está cerrada");
    }
}
