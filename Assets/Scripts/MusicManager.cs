using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour

{

    public AudioMixer MasterMixer;
    public static MusicManager Instance { get; private set; }

    private const float DefaultVolume = 0.5f;
    private float CurrentMusicVolume;
    private float CurrentSFXVolume;

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

       LoadSoundMenu();
    }

    public void LoadSoundMenu()
    {
            CurrentMusicVolume = PlayerPrefs.GetFloat("VolumeMusic", DefaultVolume);
            ApplyMusicVolume(CurrentMusicVolume);
            CurrentSFXVolume = PlayerPrefs.GetFloat("VolumeSFX", DefaultVolume);
            ApplySFXVolume(CurrentSFXVolume);
    }

    public void SetupSlider(Slider VisualSlider, bool IsMusicSlider)
    {
        if (VisualSlider == null) return;
        VisualSlider.value = IsMusicSlider ? CurrentMusicVolume : CurrentSFXVolume;

        if (IsMusicSlider)
        {
            VisualSlider.onValueChanged.AddListener(OnSliderMusic);
        }
        else
        {
            VisualSlider.onValueChanged.AddListener(OnSliderSFX);
        }
    }

    public void OnSliderMusic(float Volume)
    {
        CurrentMusicVolume = Volume;
        ApplyMusicVolume(Volume);
        PlayerPrefs.SetFloat("VolumeMusic", Volume);
        PlayerPrefs.Save();
        LoadSoundMenu();
    }

    public void OnSliderSFX(float Volume)
    {
        CurrentSFXVolume = Volume;
        ApplySFXVolume(Volume);
        PlayerPrefs.SetFloat("VolumeSFX", Volume);
        PlayerPrefs.Save();
        LoadSoundMenu();
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
