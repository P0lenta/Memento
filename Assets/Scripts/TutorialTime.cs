using UnityEngine;

public class TutorialTime : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float Timer = 2f;
    private bool CanWalk = false;

    void Start()
    {
        CanWalk = PlayerPrefs.GetInt("TutorialTime_Walked", 0) == 1;

        if (!CanWalk)
        {
            playerMovement.IsInteracting = true;
        }
        else
        {
            playerMovement.IsInteracting = false;
        }
    }

    void Update()
    {
        if (CanWalk) return;
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                playerMovement.IsInteracting = false;
                CanWalk = true;
                PlayerPrefs.SetInt("TutorialTime_Walked", 1);
                PlayerPrefs.Save();
            }
    }
}
