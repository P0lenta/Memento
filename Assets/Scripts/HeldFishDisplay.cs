using UnityEngine;
using TMPro;

public class HeldFishDisplay : MonoBehaviour
{
    public TextMeshProUGUI DisplayText;
    private EmotionType ActualFish;

    void Start()
    {
        UpdateText();
    }
    
    void Update()
    {
        if (EmotionManager.Instance == null) return;

        if (EmotionManager.Instance.HeldFish != ActualFish)
        {
            ActualFish = EmotionManager.Instance.HeldFish;
            UpdateText();
        }
    }

    void UpdateText()
    {
        if (DisplayText == null) return;

        if (ActualFish == EmotionType.None)
        {
            DisplayText.text = "";
        }
        else
        {
            DisplayText.text = $"Segurando peixe: {ActualFish}";
        }
    }
}
