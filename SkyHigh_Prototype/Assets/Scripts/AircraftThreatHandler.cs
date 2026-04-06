using UnityEngine;

public class AircraftThreatHandler : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            // Dağa çarpınca uçağı dondur ama fiziksel varlığını yok etme!
            FreezePlane();
            if (examManager != null) examManager.HandleTerrainCrash();
        }
    }

    // FÜZENİN ÇARPMASINI GARANTİLEYEN KISIM BURASI
    public void MissileHit()
    {
        FreezePlane();
        if (examManager != null) examManager.TriggerGameOver();
    }

    private void FreezePlane()
    {
        // Uçuş kontrolünü kapat
        AircraftFlightController flightController = GetComponent<AircraftFlightController>();
        if (flightController != null) flightController.enabled = false;

        // Hızı sıfırla ama KINEMATIC YAPMA (Füze uçağı bulabilsin)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false; // Yerçekimini kapat ki dağdan aşağı kaymasın
            rb.constraints = RigidbodyConstraints.FreezeAll; // Her şeyi kilitle
        }
    }
}