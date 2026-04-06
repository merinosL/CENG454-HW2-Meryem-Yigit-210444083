using UnityEngine;

public class AircraftFlightController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float glideAcceleration = 3f;
    [SerializeField] private float groundBrakeSpeed = 40f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float liftPower = 8f; 
    [SerializeField] private float pitchSpeed = 30f; 
    [SerializeField] private float rollSpeed = 30f; 
    [SerializeField] private float yawSpeed = 20f; 
    [SerializeField] private float autoNoseDownSpeed = 15f;
    [SerializeField] private AudioSource engineAudio;

    private Rigidbody rb;
    private float currentSpeed;
    private bool isEngineOn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        currentSpeed = 0f;
        if (engineAudio == null) engineAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.5f);

        if (Input.GetKey(KeyCode.Space))
        {
            isEngineOn = true;
            if (currentSpeed < baseSpeed) currentSpeed += glideAcceleration * Time.fixedDeltaTime;
            
            rb.AddForce(Vector3.up * liftPower, ForceMode.Acceleration);
            
            if (engineAudio != null && !engineAudio.isPlaying) engineAudio.Play();
        }
        else
        {
            if (engineAudio != null && engineAudio.isPlaying) engineAudio.Stop();

            if (isEngineOn)
            {
                if (isGrounded)
                {
                    currentSpeed -= groundBrakeSpeed * Time.fixedDeltaTime;
                    currentSpeed = Mathf.Max(currentSpeed, 0f);
                    if (currentSpeed == 0f) isEngineOn = false;
                }
                else
                {
                    currentSpeed += glideAcceleration * Time.fixedDeltaTime;
                    currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
                }
            }
        }

        if (isEngineOn || currentSpeed > 0f)
        {
            float pitch = Input.GetAxis("Vertical") * pitchSpeed * Time.fixedDeltaTime;
            float yaw = Input.GetAxis("Horizontal") * yawSpeed * Time.fixedDeltaTime;
            float roll = 0f;
            
            if (!Input.GetKey(KeyCode.Space) && !isGrounded)
            {
                pitch += autoNoseDownSpeed * Time.fixedDeltaTime;
            }

            if (Input.GetKey(KeyCode.Q)) roll = rollSpeed * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.E)) roll = -rollSpeed * Time.fixedDeltaTime;

            if (isGrounded)
            {
                Vector3 currentEuler = transform.rotation.eulerAngles;
                float smoothPitch = Mathf.LerpAngle(currentEuler.x, 0f, Time.fixedDeltaTime * 5f);
                float smoothRoll = Mathf.LerpAngle(currentEuler.z, 0f, Time.fixedDeltaTime * 5f);
                rb.MoveRotation(Quaternion.Euler(smoothPitch, currentEuler.y + yaw, smoothRoll));
            }
            else
            {
                Quaternion deltaRotation = Quaternion.Euler(pitch, yaw, roll);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }

            Vector3 movement = transform.forward * currentSpeed;
            movement.y = rb.linearVelocity.y;
            rb.linearVelocity = movement;
        }
    }
}