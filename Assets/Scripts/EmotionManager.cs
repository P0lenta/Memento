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
    public bool IsTimeToBed = false;
    public string LastScene = "";
    public bool WasMapOpened = false;
    public bool WasWaterAcessed = false;


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
        PlayerPrefs.SetInt("WasMapOpened", WasMapOpened ? 1 : 0);
        PlayerPrefs.SetInt("WasWaterAcessed", WasWaterAcessed ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        CurrentMission = PlayerPrefs.GetInt("CurrentMission", 0);
        CurrentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        WasMapOpened = PlayerPrefs.GetInt("WasMapOpened", 0) == 1;
        WasWaterAcessed = PlayerPrefs.GetInt("WasWaterAcessed", 0) == 1;
    }

}