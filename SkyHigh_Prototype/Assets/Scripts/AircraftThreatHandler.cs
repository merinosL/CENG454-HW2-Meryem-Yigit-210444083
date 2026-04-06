using UnityEngine;

public class AircraftThreatHandler : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            FreezePlane();
            if (examManager != null) examManager.HandleTerrainCrash();
        }
    }

    public void MissileHit()
    {
        FreezePlane();
        if (examManager != null) examManager.TriggerGameOver();
    }

    private void FreezePlane()
    {
        AircraftFlightController flightController = GetComponent<AircraftFlightController>();
        if (flightController != null) flightController.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}