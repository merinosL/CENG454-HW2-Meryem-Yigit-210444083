using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    [SerializeField] private float liftPower = 20f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * liftPower, ForceMode.Acceleration);
        }
    }
}