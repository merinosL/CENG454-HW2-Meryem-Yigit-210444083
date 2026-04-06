using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlightExamManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text warningText;
    public TMP_Text countdownText;
    public TMP_Text gameOverText;
    public TMP_Text missionCompleteText;

    [Header("Game Objects & References")]
    [SerializeField] private MissileController missile;
    [SerializeField] private Transform playerPlane;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource warningAudio;
    [SerializeField] private AudioSource crashAudio;
    [SerializeField] private AudioSource successAudio;

    private bool playerInZone = false;
    private bool threatCleared = false;
    private bool isGameOver = false;
    private bool waitingForCrash = false; 
    
    private Coroutine missileTimerRoutine;

    void Start()
    {
        if (warningText != null) warningText.gameObject.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (missionCompleteText != null) missionCompleteText.gameObject.SetActive(false);
        
        if (missile != null)
        {
            missile.gameObject.SetActive(false);
        }

        Debug.Log("Flight Exam Manager initialized. Ready for takeoff.");
    }

    public void EnterDangerZone()
    {
        if (isGameOver || waitingForCrash) return;

        Debug.Log("Player entered Danger Zone!");
        playerInZone = true;
        
        if (warningText != null) warningText.gameObject.SetActive(true);

        if (warningAudio != null) 
            warningAudio.Play();

        if (missileTimerRoutine != null) 
            StopCoroutine(missileTimerRoutine);
            
        missileTimerRoutine = StartCoroutine(LaunchSequenceTimer());
    }

    public void ExitDangerZone()
    {
        if (isGameOver || waitingForCrash) return;

        Debug.Log("Player escaped the Danger Zone!");
        playerInZone = false;
        threatCleared = true; 
        
        if (warningText != null) warningText.gameObject.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);

        if (warningAudio != null && warningAudio.isPlaying) 
            warningAudio.Stop();

        if (missileTimerRoutine != null)
        {
            StopCoroutine(missileTimerRoutine);
            missileTimerRoutine = null;
        }

        if (missile != null)
        {
            missile.DeactivateMissile();
        }
    }

    public void AttemptLanding()
    {
        if (isGameOver || waitingForCrash) return;

        if (threatCleared == true)
        {
            Debug.Log("Mission Complete! Safe landing.");
            if (missionCompleteText != null) missionCompleteText.gameObject.SetActive(true);
            
            if (successAudio != null && !successAudio.isPlaying) 
                successAudio.Play();

            if (playerPlane != null)
            {
                playerPlane.GetComponent<AircraftFlightController>().enabled = false;

                Rigidbody rb = playerPlane.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true; 
                }
            }

            StartCoroutine(RestartSceneAfterDelay(5f));
        }
        else
        {
            Debug.Log("Cannot complete mission yet. Threat is not cleared.");
        }
    }

    public void HandleTerrainCrash()
    {
        if (isGameOver) return;

        Debug.Log("Aircraft crashed into terrain!");
        
        if (warningText != null) warningText.gameObject.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (warningAudio != null && warningAudio.isPlaying) warningAudio.Stop();
        
        if (missileTimerRoutine != null)
        {
            StopCoroutine(missileTimerRoutine);
            missileTimerRoutine = null;
        }

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);

        if (missile != null && missile.gameObject.activeInHierarchy)
        {
            waitingForCrash = true;
            missile.Enrage(); 
        }
        else
        {
            isGameOver = true;
            StartCoroutine(RestartSceneAfterDelay(3f));
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        
        Debug.Log("Game Over triggered by missile hit.");
        isGameOver = true;
        waitingForCrash = false;

        if (warningText != null) warningText.gameObject.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (warningAudio != null && warningAudio.isPlaying) warningAudio.Stop();

        if (missileTimerRoutine != null)
        {
            StopCoroutine(missileTimerRoutine);
            missileTimerRoutine = null;
        }

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        
        if (crashAudio != null) crashAudio.Play();
        
        if (missile != null) missile.gameObject.SetActive(false);

        StartCoroutine(RestartSceneAfterDelay(3f));
    }

    private IEnumerator RestartSceneAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LaunchSequenceTimer()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(true);
        int timeLeft = 5;

        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        if (countdownText != null) countdownText.gameObject.SetActive(false);

        if (playerInZone && !isGameOver && !waitingForCrash)
        {
            Debug.Log("Deploying Missile!");
            if (missile != null && playerPlane != null)
            {
                missile.gameObject.SetActive(true);
                missile.ActivateMissile(playerPlane);
            }
        }
    }
}