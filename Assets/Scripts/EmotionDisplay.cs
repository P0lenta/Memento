using UnityEngine;
using TMPro;

public class EmotionDisplay : MonoBehaviour
{
    public GameObject HappyIcon;
    public GameObject SadIcon;
    public GameObject AngryIcon;
    public GameObject FearIcon;
    public GameObject SurprisedIcon;

    void Update()
    {
        if (EmotionManager.Instance == null) return;

        EmotionType current = EmotionManager.Instance.GetCurrentEmotion();

        if (HappyIcon != null) HappyIcon.SetActive(false);
        if (SadIcon != null) SadIcon.SetActive(false);
        if (AngryIcon != null) AngryIcon.SetActive(false);
        if (FearIcon != null) FearIcon.SetActive(false);
        if (SurprisedIcon != null) SurprisedIcon.SetActive(false);

        switch(current)
        {
            case EmotionType.Happy:
            if (HappyIcon != null) HappyIcon.SetActive(true);
            break;

            case EmotionType.Sad:
            if (SadIcon != null) SadIcon.SetActive(true);
            break;

            case EmotionType.Angry:
            if (AngryIcon != null) AngryIcon.SetActive(true);
            break;

            case EmotionType.Fear:
            if (FearIcon != null) FearIcon.SetActive(true);
            break;

            case EmotionType.Surprised:
            if (SurprisedIcon != null) SurprisedIcon.SetActive(true);
            break;
        }
    }


}


