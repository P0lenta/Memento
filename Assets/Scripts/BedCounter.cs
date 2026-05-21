using UnityEngine;
using TMPro;
using UnityEngine.UI;      
using System.Collections;  

public class BedCounter : MonoBehaviour
{
    public Image FadeImage;
    public float FadeSleepDuration = 1.5f;
    public float SleepTextSpeed = 0.06f;
    public float PassDaySpeed = 0.5f;
    public TextMeshProUGUI SleepText;
    public TextMeshProUGUI PassDayText;
    public string[] SleepRawMessages;
    public string[] SleepPassMessages;
    private string PassDayMessage = "DIA";
    private bool DayPassed = false;

    void Start()
    {
        if (SleepText != null) SleepText.text = "";
        if (PassDayText != null) PassDayText.text = "";

        if (FadeImage != null) FadeImage.color = new Color(0, 0, 0, 0);
    }

    public void Sleep()
    {
        DayPassed = false;

        if (EmotionManager.Instance.CurrentMission >= EmotionManager.Instance.CurrentDay * 2) IncreaseDay();

        StartCoroutine(FadeAnimation());
    }

    public void IncreaseDay()
    {
        EmotionManager.Instance.CurrentDay++;
        DayPassed = true;
        Debug.Log("Contador de dias aumentado");
    } 

    private IEnumerator FadeAnimation()
    {
        PlayerInteraction.IsSleeping = true;

        yield return StartCoroutine(Fade(0f, 1f));

        EmotionManager.Instance.HeldFish = EmotionType.None;
        EmotionManager.Instance.SetEmotion(EmotionType.None);

        yield return new WaitForSeconds(0.5f);

        if (DayPassed) 
        {
            yield return StartCoroutine(TypeWriter
            (PassDayText, PassDayMessage + EmotionManager.Instance.CurrentDay, PassDaySpeed));

            yield return new WaitForSeconds (0.5f);

            yield return StartCoroutine(TypeWriter
            (SleepText, SleepPassMessages[Random.Range (0, SleepPassMessages.Length)], SleepTextSpeed));
            
        }
        else
        { 
            yield return StartCoroutine(TypeWriter
            (SleepText, SleepRawMessages[Random.Range (0, SleepRawMessages.Length)], SleepTextSpeed));
        }

        yield return new WaitForSeconds(1.5f);

        if (SleepText != null) SleepText.text = "";
        if (PassDayText != null) PassDayText.text = "";

        yield return StartCoroutine(Fade(1f, 0f));
        
        PlayerInteraction.IsSleeping = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float TimePassed = 0f;
        Color UsedColor = FadeImage.color;
        
        while (TimePassed < FadeSleepDuration)
        {
            TimePassed += Time.deltaTime;
            UsedColor.a = Mathf.Lerp(from, to, TimePassed / FadeSleepDuration);
            FadeImage.color = UsedColor;
            yield return null;
        }

        UsedColor.a = to;
        FadeImage.color = UsedColor;
    } 

    private IEnumerator TypeWriter(TextMeshProUGUI TextObject, string Message, float Speed)
    {
        TextObject.text = "";
        foreach (char T in Message)
        {
            TextObject.text += T;
            yield return new WaitForSeconds(Speed);
        }
    }

}
