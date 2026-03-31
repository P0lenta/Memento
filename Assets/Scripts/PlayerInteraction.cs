using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Emoção capturada")]
    public EmotionType HeldFishEmotion = EmotionType.None;

    [Header("flags")]
    public bool CanInteract = false;
    public GameObject ActualInteractiveObject = null;
    public GameObject focusedObject = null;
    public GameObject HandsUI;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!context.started) return;

        if (focusedObject != null)
        {
            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.EndFocus();
            return; 
        }
        
        if (!CanInteract || ActualInteractiveObject == null)
        {
            return;
        }

        EmotionGiver giver = ActualInteractiveObject.GetComponent<EmotionGiver>();
        if (giver != null)
        {
            EmotionManager.Instance.SetEmotion(giver.EmotionToGive);
        }

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (Focus != null)
            {
                Focus.StartFocus(this);
                return;
            }

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null)
        {
            SceneChanger.LoadScene();
            return;
        }
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null)
        {
            Fish.Interact(this);
            return;
        }

        FishDelivery Delivery = ActualInteractiveObject.GetComponent<FishDelivery>();
        if (Delivery != null)
        {
            Delivery.TryDeliver(this);
            return;
        }
}


}