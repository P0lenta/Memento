using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    
    public EmotionType EmotionAttached;
    public bool DisableMatch = false;

    public void Update()
    {
        if (EmotionManager.Instance == null) return;
        EmotionType current = EmotionManager.Instance.GetCurrentEmotion();

        bool ShouldBeActivade = (current == EmotionAttached) ? !DisableMatch : DisableMatch;

        if (gameObject.activeSelf != ShouldBeActivade)
        {
            gameObject.SetActive(ShouldBeActivade);
        }
    }

}
