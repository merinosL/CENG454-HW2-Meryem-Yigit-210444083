using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 15f; // Normalde yavaş gelsin (kaçabil diye)
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float slowDownDistance = 10f;
    [SerializeField] private float hitDistance = 5f;
    
    private Transform target;
    private bool isActive = false;
    private bool isEnraged = false; // Öfke modu

    void Update()
    {
        if (!isActive || target == null) return;

        transform.LookAt(target);

        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance <= hitDistance)
        {
            AircraftThreatHandler threat = target.GetComponent<AircraftThreatHandler>();
            if (threat != null) threat.MissileHit();
            DeactivateMissile();
            return;
        }

        // Eğer öfke modu aktifse hızı 100 yap, değilse normal hızları kullan
        float currentSpeed = isEnraged ? 100f : (distance > slowDownDistance ? chaseSpeed : followSpeed);
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
    }

    public void Enrage() 
    {
        isEnraged = true;
    }

    public void ActivateMissile(Transform targetPlane)
    {
        target = targetPlane;
        isActive = true;
        isEnraged = false; 
    }

    public void DeactivateMissile()
    {
        isActive = false;
        isEnraged = false;
        gameObject.SetActive(false);
    }
}