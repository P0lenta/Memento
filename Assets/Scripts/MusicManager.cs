using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour

{
    public Slider MusicSlider;
    public Slider SFXSlider;
    public float VolumeMusic = 0.5f;
    public float VolumeSFX = 0.5f;
    public AudioMixer MasterMixer;

    void Awake() => Invoke("Setup", 0.1f);

    void Setup()
    {
        if(PlayerPrefs.HasKey("MusicVolume")) VolumeMusic = PlayerPrefs.GetFloat("MusicVolume");
        OnSliderMusic(VolumeMusic);
        MusicSlider.value = VolumeMusic;

        if(PlayerPrefs.HasKey("SFXVolume")) VolumeSFX = PlayerPrefs.GetFloat("SFXVolume");
        OnSliderSFX(VolumeSFX);
        SFXSlider.value = VolumeSFX;
    }

    public void OnSliderMusic(float Volume) => OnSetVolume("MusicVolume", ref VolumeMusic, ref Volume);
    public void OnSliderSFX(float Volume) => OnSetVolume("SFXVolume", ref VolumeSFX, ref Volume);

    public void OnSetVolume(string MixerGroup, ref float CurrentVolume, ref float NewVolume)
    {
        CurrentVolume = NewVolume;
        MasterMixer.SetFloat(MixerGroup, Mathf.Log10(Mathf.Max(CurrentVolume, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(MixerGroup, CurrentVolume);
        PlayerPrefs.Save();
    }

}
