using System.Collections;
using UnityEngine;
using TMPro;

public class FlightExamManager : MonoBehaviour
{
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private MissileController missile;
    [SerializeField] private Transform playerPlane;
    [SerializeField] private float missileDelay = 5f;

    private bool isThreatActive = false;
    private Coroutine launchCoroutine;

    void Start()
    {
        HideWarning();
        if (missile != null)
        {
            missile.gameObject.SetActive(false);
        }
    }

    public void EnterDangerZone()
    {
        isThreatActive = true;
        ShowWarning();

        if (launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
        }
        launchCoroutine = StartCoroutine(LaunchSequence());
    }

    public void ExitDangerZone()
    {
        isThreatActive = false;
        HideWarning();

        if (launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
            launchCoroutine = null;
        }

        if (missile != null)
        {
            missile.DeactivateMissile();
        }
    }

    private IEnumerator LaunchSequence()
    {
        yield return new WaitForSeconds(missileDelay);

        if (isThreatActive && missile != null && playerPlane != null)
        {
            missile.gameObject.SetActive(true);
            missile.ActivateMissile(playerPlane);
        }
    }

    private void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
        }
    }

    private void HideWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }
}