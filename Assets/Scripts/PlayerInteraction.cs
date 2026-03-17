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

    public GameObject focusedObject = null;

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
            CameraFocus focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (focus != null)
            {
                focus.StartFocus(this); 
            }
            else
            {
                if (InteractionMessage != null)
                {
                    InteractionMessage.text = message;
                    InteractionMessage.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (InteractionMessage != null)
                InteractionMessage.text = "";
        }
    }
    public void OnExitFocus (InputAction.CallbackContext context)
    {
        if (context.started && focusedObject != null)
        {
            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
            {
                focus.EndFocus();
            }
        }
    }
}
