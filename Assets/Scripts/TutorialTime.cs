using UnityEngine;

public class TutorialTime : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private bool CanWalk = false;

    void Start()
    {
        CanWalk = PlayerPrefs.GetInt("TutorialTime_Walked", 0) == 1;

        if (!CanWalk)
        {
            playerMovement.IsTutorialRunning = true;
        }
        else
        {
            playerMovement.IsTutorialRunning = false;
        }
    }

    public void LiberateWalk()
    {
        if (CanWalk) return;

        CanWalk = true;
        playerMovement.IsTutorialRunning = false;
        PlayerPrefs.SetInt("TutorialTime_Walked", 1);
        PlayerPrefs.Save();
    }
}
