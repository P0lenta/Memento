using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("flags")]
    public bool CanInteract = false;
    public GameObject ActualInteractiveObject = null;

    [Header("UI")]
    public TextMeshProUGUI InteractionMessage;
    public string message = "MACACOS ME MORDÃO";

    void Start()
    {
          if (InteractionMessage != null)
        {
         InteractionMessage.gameObject.SetActive(false);  
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && CanInteract && ActualInteractiveObject != null)
        {   
            if (InteractionMessage != null) 
            {
                InteractionMessage.text = message;
                InteractionMessage.gameObject.SetActive(true);  
            }
        }
            else
            {
                if (InteractionMessage != null)
                {
                InteractionMessage.text = "";   
                }
        }
    }
}
