using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float followDistance = 15f;
    
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

        if (Vector3.Distance(transform.position, target.position) > followDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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