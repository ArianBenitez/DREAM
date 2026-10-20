using UnityEngine;

public class TimerDial : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameTimer timer;
    [SerializeField] private RectTransform needle;

    [Header("Recorrido de la aguja")]
    [SerializeField] private float startAngle = 90f; // Izquierda
    [SerializeField] private float endAngle = -90f; // Derecha, fin de la partida

    private void Update()
    {
        if (timer == null || needle == null) return;

        float angle = Mathf.Lerp(startAngle, endAngle, timer.Progress);
        needle.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
