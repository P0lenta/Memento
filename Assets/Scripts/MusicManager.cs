using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour

{

    public AudioMixer MasterMixer;
    public static MusicManager Instance { get; private set; }

    private const float DefaultVolume = 0.5f;
    public float CurrentMusicVolume { get; private set; }
    public float CurrentSFXVolume { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSoundMenu();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSoundMenu()
    {
        Debug.Log("=== LoadSoundMenu ===");
        CurrentMusicVolume = PlayerPrefs.GetFloat("VolumeMusic", DefaultVolume);
        CurrentSFXVolume = PlayerPrefs.GetFloat("VolumeSFX", DefaultVolume);
        Debug.Log($"Carregou MusicVolume = {CurrentMusicVolume}, SFXVolume = {CurrentSFXVolume}");
        ApplyMusicVolume(CurrentMusicVolume);
        ApplySFXVolume(CurrentSFXVolume);
    }

    public void SetupSlider(UnityEngine.UI.Slider VisualSlider, bool IsMusicSlider)
    {
        if (VisualSlider == null) return;
        VisualSlider.onValueChanged.RemoveAllListeners();
        VisualSlider.value = IsMusicSlider ? CurrentMusicVolume : CurrentSFXVolume;
        if (IsMusicSlider) VisualSlider.onValueChanged.AddListener(OnSliderMusic);
        else VisualSlider.onValueChanged.AddListener(OnSliderSFX);
    }

    public void OnSliderMusic(float Volume)
    {
        Debug.Log($"OnSliderMusic recebido: {Volume}");
        CurrentMusicVolume = Volume;
        ApplyMusicVolume(Volume);
        PlayerPrefs.SetFloat("VolumeMusic", Volume);
        PlayerPrefs.Save();
    }

    public void OnSliderSFX(float Volume)
    {
        CurrentSFXVolume = Volume;
        ApplySFXVolume(Volume);
        PlayerPrefs.SetFloat("VolumeSFX", Volume);
        PlayerPrefs.Save();
    }
    
    public void ApplyMusicVolume(float LinearVolume)
    {
        float dB = Mathf.Log10(Mathf.Max(LinearVolume, 0.0001f)) * 20f;
        MasterMixer.SetFloat("VolumeMusic", dB);
    }

    public void ApplySFXVolume(float LinearVolume)
    {
        float dB = Mathf.Log10(Mathf.Max(LinearVolume, 0.0001f)) * 20f;
        MasterMixer.SetFloat("VolumeSFX", dB);
    }

}
