using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [Header("UI")]
    public TextMeshProUGUI InteractionText;
    public string message = "E";
    public AudioSource InteractionSound;

    void Start()
    {
        if (InteractionText != null) InteractionText.gameObject.SetActive(false);  
    }

    public void OnGainFocus()
    {
        if (!PlayerInteraction.IsInputLocked)
        {
            if (InteractionText != null)
            {
                InteractionText.text = message;
                InteractionText.gameObject.SetActive(true);
            }
            
            PlayerInteraction.Instance?.SetInteractionImageVisible(true);

        }
    }

    public void OnLostFocus()
    {
        if (InteractionText != null) InteractionText.gameObject.SetActive(false);
        PlayerInteraction.Instance?.SetInteractionImageVisible(false);
    }

    private void OnDisable()
    {

        if (InteractionText != null) InteractionText.gameObject.SetActive(false);

    }
}