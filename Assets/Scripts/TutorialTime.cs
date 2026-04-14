using UnityEngine;

public class TutorialTime : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float Timer = 2f;

    private static bool CanWalk = false;

    void Awake()
    {
        CanWalk = PlayerPrefs.GetInt("TutorialTime_Walked", 0) == 1;
        playerMovement.IsInteracting = true;
    }

    void Update()
    {
        if (!CanWalk && Timer > 0f)
        {
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
}
