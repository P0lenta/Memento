using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    public EmotionType EmotionAttached;
    public bool DisableMatch = false;

    void Update()
    {
        if (EmotionManager.Instance == null) return;
        
        EmotionType current = EmotionManager.Instance.GetCurrentEmotion();
        bool active = (current == EmotionAttached) ? !DisableMatch : DisableMatch;
        
        gameObject.SetActive(active);
    }
}