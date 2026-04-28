using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [Header("UI")]
    public TextMeshProUGUI InteractionText;
    public string message = "E";

    [Header("Efeitos sonoros")]
    public AudioSource InteractionSound;

    void Start()
    {
        if (InteractionText != null)
        {
         InteractionText.gameObject.SetActive(false);  
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (!other.CompareTag("Player")) return;

         PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                PlayerInteraction.CanInteract = true;
                playerInteraction.ActualInteractiveObject = gameObject; 
            }
            if (InteractionText != null)
            {
                InteractionText.text = message;
                InteractionText.gameObject.SetActive(true);
            }
            
        }

    private void OnTriggerExit (Collider other)
    {
        if (!other.CompareTag("Player")) return;
             PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                PlayerInteraction.CanInteract = false;
                playerInteraction.ActualInteractiveObject = null;
            }

            if (InteractionText != null)
            {
                InteractionText.gameObject.SetActive(false);
            }
        
    }

    private void OnDisable()
    {
        if (InteractionText != null)
        {
            InteractionText.gameObject.SetActive(false);
        }
    }

}