using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("flags")]
    public bool CanInteract = false;
    public GameObject ActualInteractiveObject = null;
    public GameObject focusedObject = null;
    public GameObject HandsUI;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && CanInteract && ActualInteractiveObject != null)
        {
            //CÓDIGO DE INTERAÇÃO

            CameraFocus focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (focus != null)
            {
                focus.StartFocus(this); 
            }
        }
    }
    public void OnExitFocus (InputAction.CallbackContext context)
    {
        if (context.started && focusedObject != null)
        {
            //CÓDIGO SAÍDA DE INTERAÇÃO

            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
            {
                focus.EndFocus();
            }
        }
    }
}
