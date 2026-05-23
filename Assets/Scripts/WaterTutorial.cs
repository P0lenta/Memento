using UnityEngine;
using TMPro;

public class WaterTutorial : MonoBehaviour
{
    [Header("Texto")]
    public string TextoDoTutorial;

    [Header("Referências")]
    public GameObject TextObj; 
    public TextMeshProUGUI TutorialText;

    void Start()
    {
        TutorialText.text = TextoDoTutorial;

        OnWaterAcessed();
    }

    public void OnWaterAcessed()
    {
        if (TextObj != null) TextObj.SetActive(!EmotionManager.Instance.WasWaterAcessed);
    }
}
