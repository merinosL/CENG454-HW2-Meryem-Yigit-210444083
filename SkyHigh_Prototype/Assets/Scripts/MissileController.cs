using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 50f;
    [SerializeField] private float followSpeed = 25f;
    [SerializeField] private float slowDownDistance = 10f;
    [SerializeField] private float followDistance = 0.5f;
    
    private Transform target;
    private bool isActive = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!isActive || target == null) return;

        transform.LookAt(target);

        float distance = Vector3.Distance(transform.position, target.position);
        float currentSpeed = distance > slowDownDistance ? chaseSpeed : followSpeed;

        if (distance > followDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }
    }

    public void ActivateMissile(Transform targetPlane)
    {
        target = targetPlane;
        isActive = true;
    }

    public void DeactivateMissile()
    {
        isActive = false;
        gameObject.SetActive(false);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}