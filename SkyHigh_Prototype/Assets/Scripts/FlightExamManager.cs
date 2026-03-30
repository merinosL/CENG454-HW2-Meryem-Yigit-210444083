using UnityEngine;
using TMPro;

public class FlightExamManager : MonoBehaviour
{
    [SerializeField] private TMP_Text warningText;
    
    private bool isThreatActive = false;

    void Start()
    {
        HideWarning();
    }

    public void EnterDangerZone()
    {
        isThreatActive = true;
        ShowWarning();
        Debug.Log("Danger Zone Entered!");
    }

    public void ExitDangerZone()
    {
        isThreatActive = false;
        HideWarning();
        Debug.Log("Danger Zone Exited!");
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