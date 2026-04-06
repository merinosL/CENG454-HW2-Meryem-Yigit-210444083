using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlightExamManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text missionCompleteText;

    [Header("Game Objects")]
    [SerializeField] private MissileController missile;
    [SerializeField] private Transform playerPlane;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource warningAudio;
    [SerializeField] private AudioSource crashAudio;
    [SerializeField] private AudioSource successAudio;

    private bool isThreatActive = false;
    private bool threatCleared = false;
    private bool isGameOver = false;
    private bool isWaitingForMissile = false;
    private Coroutine launchCoroutine;

    void Start()
    {
        HideWarning();
        HideCountdown();
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (missionCompleteText != null) missionCompleteText.gameObject.SetActive(false);
        
        if (missile != null)
        {
            missile.gameObject.SetActive(false);
        }
    }

    public void EnterDangerZone()
    {
        if (isGameOver || isWaitingForMissile) return;

        isThreatActive = true;
        ShowWarning();

        if (warningAudio != null) warningAudio.Play();

        if (launchCoroutine != null) StopCoroutine(launchCoroutine);
        launchCoroutine = StartCoroutine(LaunchSequence());
    }

    public void ExitDangerZone()
    {
        if (isGameOver || isWaitingForMissile) return;

        isThreatActive = false;
        threatCleared = true;
        HideWarning();
        HideCountdown();

        if (warningAudio != null && warningAudio.isPlaying) warningAudio.Stop();

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

    public void AttemptLanding()
    {
        if (isGameOver || isWaitingForMissile) return;

        if (threatCleared)
        {
            if (missionCompleteText != null) missionCompleteText.gameObject.SetActive(true);
            if (successAudio != null && !successAudio.isPlaying) successAudio.Play();

            if (playerPlane != null)
            {
                AircraftFlightController flightController = playerPlane.GetComponent<AircraftFlightController>();
                if (flightController != null) flightController.enabled = false;

                Rigidbody rb = playerPlane.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
            }
        }
    }

    public void HandleTerrainCrash()
    {
        if (isGameOver) return;

        // Uyarıları hemen gizle
        HideWarning();
        HideCountdown();
        if (warningAudio != null && warningAudio.isPlaying) warningAudio.Stop();
        if (launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
            launchCoroutine = null;
        }

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);

        // Füze aktifse hiçbir şey yapma (ses çalma, baştan başlatma), bekle!
        if (missile != null && missile.gameObject.activeInHierarchy)
        {
            isWaitingForMissile = true;
        }
        else
        {
            // Füze yoksa normal Game Over sürecini işlet
            isGameOver = true;
            if (crashAudio != null) crashAudio.Play();
            StartCoroutine(RestartLevel());
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        isWaitingForMissile = false;

        HideWarning();
        HideCountdown();
        if (warningAudio != null && warningAudio.isPlaying) warningAudio.Stop();

        if (launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
            launchCoroutine = null;
        }

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        
        if (crashAudio != null) crashAudio.Play();
        if (missile != null) missile.gameObject.SetActive(false);

        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        if (isThreatActive && missile != null && playerPlane != null && !isGameOver && !isWaitingForMissile)
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