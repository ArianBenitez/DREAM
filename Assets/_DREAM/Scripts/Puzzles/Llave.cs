using UnityEngine;

// Va en la llave del maletero. Al cogerla desaparece y queda apuntado que el jugador la tiene.
// Cuando exista el inventario, irá ahí
public class Llave : MonoBehaviour
{
    public static bool Recogida { get; private set; }

    void Awake() => Recogida = false; // Cada Play empieza sin llave

    public void Coger() 
    {
        Recogida = true;
        gameObject.SetActive(false);
    }
}
