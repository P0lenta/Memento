using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    public EmotionType EmotionAttached;
    public bool DisableMatch = false;
    public int MinDay = 1;
    public int MaxDay = 10;

    void Start()
    {
        int Day = EmotionManager.Instance.CurrentDay;
        if (Day < MinDay || Day > MaxDay) gameObject.SetActive(false);
    }

    void Update()
    {
        if (EmotionManager.Instance == null) return;
        
        EmotionType current = EmotionManager.Instance.GetCurrentEmotion();
        bool active = (current == EmotionAttached) ? !DisableMatch : DisableMatch;
        
        gameObject.SetActive(active);
    }
}