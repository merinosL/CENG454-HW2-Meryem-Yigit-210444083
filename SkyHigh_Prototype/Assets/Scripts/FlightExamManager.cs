using System.Collections;
using UnityEngine;
using TMPro;

public class FlightExamManager : MonoBehaviour
{
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private MissileController missile;
    [SerializeField] private Transform playerPlane;

    private bool isThreatActive = false;
    private Coroutine launchCoroutine;

    void Start()
    {
        HideWarning();
        HideCountdown();
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
        HideCountdown();

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
        countdownText.gameObject.SetActive(true);
        int timeLeft = 5;

        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        HideCountdown();

        if (isThreatActive && missile != null && playerPlane != null)
        {
            missile.gameObject.SetActive(true);
            missile.ActivateMissile(playerPlane);
        }
    }

    private void ShowWarning()
    {
        if (warningText != null) warningText.gameObject.SetActive(true);
    }

    private void HideWarning()
    {
        if (warningText != null) warningText.gameObject.SetActive(false);
    }

    private void HideCountdown()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }
}