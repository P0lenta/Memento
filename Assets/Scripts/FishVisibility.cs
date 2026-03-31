using UnityEngine;

public class FishVisibility : MonoBehaviour
{
    public EmotionType EmotionAttached;
    public bool DisableMatch = false;

    void Update()
    {
        if (EmotionManager.Instance == null) return;
        
        EmotionType current = EmotionManager.Instance.GetCurrentEmotion();
        bool active = (current == EmotionAttached) ? !DisableMatch : DisableMatch;
        
        // SEMPRE tenta ativar/desativar, mesmo se já estiver no estado correto
        // Isso garante que se o peixe foi desativado por outro motivo, ele volte
        gameObject.SetActive(active);
    }
}