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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log("OnInteract chamado");

        if (focusedObject != null)
        {

            Debug.Log("Tem focusedObject, tentando sair do foco");

            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.EndFocus();
            return; 
        }
        
        if (!CanInteract || ActualInteractiveObject == null)
        {
            Debug.Log($"Não pode interagir: CanInteract={CanInteract}, ActualInteractiveObject={ActualInteractiveObject}");
            return;
        }

            

            Debug.Log($"Pode interagir com: {ActualInteractiveObject.name}");

        EmotionGiver giver = ActualInteractiveObject.GetComponent<EmotionGiver>();
        if (giver != null)
        {

            Debug.Log("Encontrou EmotionGiver");

            EmotionManager.Instance.SetEmotion(giver.EmotionToGive);
        }

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null)
        {

            Debug.Log("Encontrou SceneChanger");

            SceneChanger.LoadScene();
        }
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null)
        {

            Debug.Log("Encontrou FishCapture");

            Fish.Interact(this);
        }

        FishDelivery Delivery = ActualInteractiveObject.GetComponent<FishDelivery>();
        if (Delivery != null)
        {

            Debug.Log("Encontrou FishDelivery");

            Delivery.TryDeliver(this);
        }

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (Focus != null)
            {

                Debug.Log("Encontrou CameraFocus");

                Focus.StartFocus(this);
            }

        Debug.Log("Nenhum componente de interação encontrado");

}


}