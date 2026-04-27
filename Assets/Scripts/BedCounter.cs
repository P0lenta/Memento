using UnityEngine;

public class BedCounter : MonoBehaviour
{
    public Animator FadeAnimation;
    public void Sleep()
    {
        if (EmotionManager.Instance.CurrentMission >= EmotionManager.Instance.CurrentDay * 2) IncreaseDay();

        FadeAnimation.SetTrigger("Slept");

        PlayerInteraction.IsSleeping = true;
    }

    public void IncreaseDay()
    {
        EmotionManager.Instance.CurrentDay++;
        Debug.Log("Contador de dias aumentado");
    } 

    public void OnFadeComplete()
    {
        PlayerInteraction.IsSleeping = false;
        EmotionManager.Instance.HeldFish = EmotionType.None;
        EmotionManager.Instance.SetEmotion(EmotionType.None);
    }
}
