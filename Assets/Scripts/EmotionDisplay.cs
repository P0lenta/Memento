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
            case EmotionType.Felicidade:
            if (HappyIcon != null) HappyIcon.SetActive(true);
            break;

            case EmotionType.Tristeza:
            if (SadIcon != null) SadIcon.SetActive(true);
            break;

            case EmotionType.Raiva:
            if (AngryIcon != null) AngryIcon.SetActive(true);
            break;

            case EmotionType.Medo:
            if (FearIcon != null) FearIcon.SetActive(true);
            break;

            case EmotionType.Surpresa:
            if (SurprisedIcon != null) SurprisedIcon.SetActive(true);
            break;
        }
    }


}


