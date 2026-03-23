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
        if (!context.started) return;

        if (focusedObject != null)
        {
            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.EndFocus();
            return; 
        }
        
        if (CanInteract && ActualInteractiveObject != null)
        {
            CameraFocus focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.StartFocus(this);
        }

        EmotionGiver emotionGiver = ActualInteractiveObject.GetComponent<EmotionGiver>();
        if (emotionGiver != null)
        {
            EmotionManager.Instance.SetEmotion(emotionGiver.emotionToGive);
            return;
        }

        SceneChanger sceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (sceneChanger != null)
        {
            sceneChanger.LoadScene();
            return;
        }
            
}

}