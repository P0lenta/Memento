using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (EmotionManager.Instance.CurrentDay == 2)
        {
            EndBuild();
            return;
        }

        EmotionManager.Instance.CurrentDay++;
        Debug.Log("Contador de dias aumentado");
    } 

    public void OnFadeComplete()
    {
        PlayerInteraction.IsSleeping = false;
        EmotionManager.Instance.HeldFish = EmotionType.None;
        EmotionManager.Instance.SetEmotion(EmotionType.None);
    }

    public void EndBuild()
    {
        Debug.Log("EndBuild chamado");
        SceneManager.LoadScene("Credits");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
