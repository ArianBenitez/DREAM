using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [Header("Balanceo")]
    [SerializeField] private float frequency = 8f;  //velocidad del balanceo          
    [SerializeField] private float verticalAmplitude = 0.05f;  //cuanto sube y baja en metros
    [SerializeField] private float horizontalAmplitude = 0.03f;
    [SerializeField] private float smoothing = 10f;  //cuanto tarda en volver al centro

    private Vector3 restPosition;
    private float timer;

    private void Awake()
    {
        restPosition = transform.localPosition; //la altura de los ojos puesta en el inspector
    }

    private void Update()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;

        Vector3 target = restPosition;

        if (speed > 0.1f && controller.isGrounded)
        {
            timer += Time.deltaTime * frequency;

            float bobY = Mathf.Sin(timer) * verticalAmplitude;
            float bobX = Mathf.Cos(timer * 0.5f) * horizontalAmplitude;

            target = restPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            timer = 0f; //al pararse, el ciclo empieza de cero la proxima vez
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * smoothing);
    }
}
