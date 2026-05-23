using UnityEngine;
using TMPro;

public class MapTutorial : MonoBehaviour
{
    [Header("Texto")]
    public string TextoDoTutorial;

    [Header("Referências")]
    public GameObject TextObj; 
    public TextMeshProUGUI TutorialText;

    void Start()
    {
        TutorialText.text = TextoDoTutorial;

        OnMapOpened();
    }

    public void OnMapOpened()
    {
        if (TextObj != null) TextObj.SetActive(!EmotionManager.Instance.WasMapOpened);
    }
}
