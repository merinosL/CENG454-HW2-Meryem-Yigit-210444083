using UnityEngine;

public class SpeedHandler : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 20f;
    [SerializeField] private float glideAcceleration = 10f;
    [SerializeField] private float maxSpeed = 45f;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            currentSpeed += glideAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
}