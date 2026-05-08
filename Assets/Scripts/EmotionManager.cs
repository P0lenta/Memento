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
    public bool IsMapUnlocked = false;
    public string LastScene = "";


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

        Load();
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

    public void Save()
    {
        PlayerPrefs.SetInt("CurrentMission", CurrentMission);
        PlayerPrefs.SetInt("CurrentDay", CurrentDay);
        PlayerPrefs.SetInt("IsMapUnlocked", IsMapUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        CurrentMission = PlayerPrefs.GetInt("CurrentMission", 0);
        CurrentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        IsMapUnlocked = PlayerPrefs.GetInt("IsMapUnlocked", 0) == 1;
    }

}