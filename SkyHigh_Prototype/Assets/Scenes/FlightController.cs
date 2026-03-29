using UnityEngine;

public class FlightController : MonoBehaviour
{
    [SerializeField] private float pitchSpeed = 45f;
    [SerializeField] private float yawSpeed = 45f;
    [SerializeField] private float rollSpeed = 45f;
    [SerializeField] private float thrustSpeed = 5f;

    private Rigidbody _rb;
    private AudioSource engineAudio;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();

        if (_rb != null)
        {
            _rb.freezeRotation = true;
        }
    }

    void Update()
    {
        HandleRotationStuff();
        HandleThrust(); 
    }

    private void HandleRotationStuff()
    {
        float pitch = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.right * pitch * pitchSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * yaw * yawSpeed * Time.deltaTime);

        float roll = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            roll = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            roll = -1f;
        }

        transform.Rotate(Vector3.forward * roll * rollSpeed * Time.deltaTime);
    }

    private void HandleThrust()
    {
        bool pressingSpace = Input.GetKey(KeyCode.Space);

        if (pressingSpace)
        {
            Vector3 forwardMove = Vector3.forward * thrustSpeed * Time.deltaTime;
            transform.Translate(forwardMove);

            if (engineAudio != null && !engineAudio.isPlaying)
            {
                engineAudio.Play();
            }
        }
        else
        {
            if (engineAudio != null && engineAudio.isPlaying)
            {
                engineAudio.Stop();
            }
        }
    }
}
