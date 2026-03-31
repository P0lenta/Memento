using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour

{
    public Slider MusicSlider;
    public Slider SFXSlider;
    public AudioMixer MasterMixer;

    private float CurrentMusicVolume = 0.5f;
    private float CurrentSFXVolume = 0.5f;

    void Awake()
    {
        if (PlayerPrefs.HasKey("VolumeMusic"))
        CurrentMusicVolume = PlayerPrefs.GetFloat("VolumeMusic");
        if (PlayerPrefs.HasKey("VolumeSFX"))
        CurrentSFXVolume = PlayerPrefs.GetFloat("VolumeSFX");

        MusicSlider.value = CurrentMusicVolume;
        SFXSlider.value = CurrentSFXVolume;

        ApplyMusicVolume(CurrentMusicVolume);
        ApplySFXVolume(CurrentSFXVolume);
    }

    public void OnSliderMusic(float Volume)
    {
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
    
    private void ApplyMusicVolume(float LinearVolume)
    {
        float dB = Mathf.Log10(Mathf.Max(LinearVolume, 0.0001f)) * 20f;
        MasterMixer.SetFloat("VolumeMusic", dB);
    }

    private void ApplySFXVolume(float LinearVolume)
    {
        float dB = Mathf.Log10(Mathf.Max(LinearVolume, 0.0001f)) * 20f;
        MasterMixer.SetFloat("VolumeSFX", dB);
    }

}
