using NUnit.Framework;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("Duración")]
    [SerializeField] private float totalSeconds = 100f;

    [Header("Pruebas")]
    [SerializeField] private bool logEverySecond = false;

    private float remainingSeconds;
    private bool isRunning;
    private int lastLoggedSecond = -1;

    public bool IsRunning => isRunning;
    public float RemainingSeconds => remainingSeconds;

    // 0 al empezar, 1 cuando se acaba el tiempo. Es lo que usara la aguja
    public float Progress => totalSeconds <= 0f ? 1f : 1f - (remainingSeconds / totalSeconds);

    private void Awake()
    {
        remainingSeconds = totalSeconds;
    }

    [ContextMenu("Arrancar cronometro")]
    public void StartTimer()
    {
        if (isRunning) return; // Si ya esta en marcha, no se reinicia

        isRunning = true;
        Debug.Log("Cronometro en marcha: " + totalSeconds + " segundos");
    }

    private void Update()
    {
        if (!isRunning) return;

        remainingSeconds -= Time.deltaTime;

        if (logEverySecond)
        {
            int second = Mathf.CeilToInt(remainingSeconds);
            if (second != lastLoggedSecond)
            {
                lastLoggedSecond = second;
                Debug.Log("Quedan " + second + " s");
            }
        }

        if (remainingSeconds <= 0f)
        {
            remainingSeconds = 0f;
            isRunning = false;
            Debug.Log("Se acabo el tiempo");
        }
    }
}
