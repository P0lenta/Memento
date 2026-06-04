using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animator CutsceneAnimator;
    public AudioSource audioSource;
    public AudioSource musicSource;
    private float SFXBefore;
    private float FadeDuration = 5f;

    public void StartMusic()
    {
        StartCoroutine(FadeMusic()); 
    }

    private IEnumerator FadeMusic()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        musicSource.volume = 0f;

        float TimePassed = 0f;
        while (TimePassed < FadeDuration)
        {
            TimePassed += Time.deltaTime;
            float NormalizedTime = TimePassed / FadeDuration;
            audioSource.volume = Mathf.Lerp(0f, 1f, NormalizedTime);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public void MuteAllSFX()
    {
        if (MusicManager.Instance != null)
        {
            SFXBefore = MusicManager.Instance.CurrentSFXVolume;
                
            MusicManager.Instance.ApplySFXVolume(0f);
        }
    }

    public void EndCutscene()
    {
        PlayerInteraction.IsSleeping = false;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        if (MusicManager.Instance != null) MusicManager.Instance.ApplySFXVolume(SFXBefore);
        EmotionManager.Instance.CurrentDay = 1;
        EmotionManager.Instance.CurrentMission = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FadeTransition.Instance.StartFade("Credits");
    }

    

}
