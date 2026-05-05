using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    public EmotionType EmotionAttached;
    public bool IsHard = false;

    void Update()
    {
        if (EmotionManager.Instance == null) return;

        Debug.Log($"[FishVisibility] Nome: {gameObject.name}, Emoção: {EmotionManager.Instance.GetCurrentEmotion()}, Emoção exigida: {EmotionAttached}");

        if (!IsHard && EmotionManager.Instance.CurrentMission > 4)
        {
            gameObject.SetActive(false);
            return;
        }

        if (IsHard && EmotionManager.Instance.CurrentMission <= 4)
        {
            gameObject.SetActive(false);
            return;
        }


        bool Visibility = (EmotionManager.Instance.GetCurrentEmotion() == EmotionAttached);
        Debug.Log($"[FishVisibility] Visibilidade calculada: {Visibility} (emoção atual == {EmotionAttached})");
        
        gameObject.SetActive(Visibility);
    }
}