using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    public EmotionType EmotionAttached;
    public bool IsHard = false;

    void Update()
    {
        if (EmotionManager.Instance == null) return;

        if (!IsHard && EmotionManager.Instance.CurrentMission > 4)
        {
            gameObject.SetActive(false);
            return;
        }

        if (IsHard && EmotionManager.Instance.CurrentMission <= 4)
        {
            gameObject.SetActive(false);
            return;
        }


        bool Visibility = (EmotionManager.Instance.GetCurrentEmotion() == EmotionAttached);
        
        gameObject.SetActive(Visibility);
    }
}