using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFocus : MonoBehaviour
{

    [Header("Config de Câmera")]
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    private bool IsFocused = false;
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;
    private Transform playerCamera;
    private Transform originalParent;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Renderer PlayerRenderer;
    private Renderer HandRenderer;
    

    public void StartFocus(PlayerInteraction interaction)
    {
        if (IsFocused) return;


            playerInteraction = interaction;
            playerMovement = interaction.GetComponent<PlayerMovement>();
            if (playerMovement == null) return;

            playerCamera = playerMovement.camTransform;
            if (playerCamera == null) return;

            if (playerMovement.playerModel != null)
            {
                PlayerRenderer = playerMovement.playerModel.GetComponent<Renderer>();
                PlayerRenderer.enabled = false;
            }


            if(playerInteraction.HandsUI != null)
            {   
                HandRenderer = playerInteraction.HandsUI.GetComponent<Renderer>();
                HandRenderer.enabled = false;
            }

            
        playerMovement.IsInteracting = true;
        playerInteraction.enabled = false;

        playerInteraction.focusedObject = gameObject;

        originalParent = playerCamera.parent;
        originalLocalPos = playerCamera.localPosition;
        originalLocalRot = playerCamera.localRotation;

        playerCamera.SetParent(null);
        playerCamera.position = cameraPosition;
        playerCamera.rotation = Quaternion.Euler(cameraRotation);

        Interactable interactable = GetComponent<Interactable>();
        if (interactable != null && interactable.InteractionText != null)
        {
            interactable.InteractionText.gameObject.SetActive(false);
        }

        IsFocused = true;



    }

    public void EndFocus()
    {
        if (!IsFocused) return;

        playerCamera.SetParent(originalParent);
        playerCamera.localPosition = originalLocalPos;
        playerCamera.localRotation = originalLocalRot;

        PlayerRenderer.enabled = true;
        HandRenderer.enabled = true;
        playerMovement.IsInteracting = false;
        playerInteraction.enabled = true;
        playerInteraction.HandsUI.SetActive(true);


        if (playerInteraction != null)
        playerInteraction.focusedObject = null;

        IsFocused = false;
    }
}
