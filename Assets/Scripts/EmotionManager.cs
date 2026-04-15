using UnityEngine;

public class EmotionManager : MonoBehaviour
{
   public static EmotionManager Instance { get; private set; }

    [SerializeField] private EmotionType currentEmotion = EmotionType.None;
    public EmotionType CurrentMissionFish = EmotionType.None;
    public EmotionType HeldFish = EmotionType.None;

    public System.Action<EmotionType> OnEmotionChanged;
    public int CurrentMission = 0;
    public int CurrentDay = 1;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy (gameObject);
            return;
        }
        Instance = this;
        {
            DontDestroyOnLoad (gameObject);
        }
    }

    public EmotionType GetCurrentEmotion()
    {
        return currentEmotion;
    }
    
    public void SetEmotion (EmotionType newEmotion)
    {
        if (currentEmotion == newEmotion) return;
        currentEmotion = newEmotion;
        OnEmotionChanged?.Invoke(currentEmotion);
    }

}