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
    public Animator HandsAnimation;


    void Start()
    {
        if (EmotionManager.Instance != null)
        HeldFishEmotion = EmotionManager.Instance.HeldFish;
        
        UpdateHoldingAnimation();
    }

    public void SetHeldFish(EmotionType Fish)
    {
        HeldFishEmotion = Fish;
        if (EmotionManager.Instance != null)
        {
            EmotionManager.Instance.HeldFish = Fish;
        }

        UpdateHoldingAnimation();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        HandsAnimation.SetTrigger("Grab");

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

        EmotionGiver Giver = ActualInteractiveObject.GetComponent<EmotionGiver>();
        if (Giver != null)
        {
            EmotionManager.Instance.SetEmotion(Giver.EmotionToGive);
        }

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (Focus != null)
            {
                Focus.StartFocus(this);
            }

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null)
        {
            SceneChanger.LoadScene();
        }
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null)
        {
            Fish.Interact(this);
        }

        FishDelivery Delivery = ActualInteractiveObject.GetComponent<FishDelivery>();
        if (Delivery != null)
        {
            Delivery.TryDeliver(this);
        }
    }

    void UpdateHoldingAnimation()
    {
        if (HeldFishEmotion == EmotionType.None)
        {
         HandsAnimation.SetBool("IsHolding", false);   
        }
        else
        {
            HandsAnimation.SetBool("IsHolding", true);
        }
    }

}