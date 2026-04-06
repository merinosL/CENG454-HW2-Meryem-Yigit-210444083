using UnityEngine;

public class AircraftThreatHandler : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            CrashSequence();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            CrashSequence();
        }
    }

    private void CrashSequence()
    {
        AircraftFlightController flightController = GetComponent<AircraftFlightController>();
        if (flightController != null)
        {
            flightController.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (examManager != null)
        {
            examManager.TriggerGameOver();
        }
    }
}