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
    public GameObject ConfigPanel;
    private bool IsInDialogue = false;
    public static bool IsMenuOpen {get; private set;}
    public static PlayerInteraction Instance {get; private set;}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (ConfigPanel == null) return; 

        bool OpenMenu = !ConfigPanel.activeSelf;
        ConfigPanel.SetActive(OpenMenu);
        
        IsMenuOpen = OpenMenu;
        CanInteract = !OpenMenu;

        PlayerMovement Movement = GetComponent<PlayerMovement>();
        if (Movement != null) 
        {
            Movement.StopMovement();
            Movement.GetSensibility();
            Movement.IsInteracting = OpenMenu;
        }

        PlayerWaterMovement WaterMovement = GetComponent<PlayerWaterMovement>();
        if (WaterMovement != null) 
        {
            WaterMovement.StopWaterMovement();
            WaterMovement.GetSensibility();
            WaterMovement.IsInteracting = OpenMenu;
        }

    }

    public void CloseMenu()
    {
        if (ConfigPanel != null && ConfigPanel.activeSelf)
        {
            ConfigPanel.SetActive(false);
            IsMenuOpen = false;
            CanInteract = true;

        PlayerMovement Movement = GetComponent<PlayerMovement>();
        if (Movement != null) 
        {
            Movement.StopMovement();
            Movement.GetSensibility();
            Movement.IsInteracting = false;
        }

        PlayerWaterMovement WaterMovement = GetComponent<PlayerWaterMovement>();
        if (WaterMovement != null) 
        {
            WaterMovement.StopWaterMovement();
            WaterMovement.GetSensibility();
            WaterMovement.IsInteracting = false;
        }
        }
    }

    public void OnTrash(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (HeldFishEmotion != EmotionType.None)
        SetHeldFish(EmotionType.None);
        Debug.Log("Peixe descartado");
    }


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

        if (IsMenuOpen) return;

        if (GetComponent<PlayerWaterMovement>() ? .IsDead == true) return;

        if (IsInDialogue)
        {
            DialogueManager.CheckNextLine();
            return;
        }

        if (focusedObject != null)
        {
            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.EndFocus();
            return;
        }

        HandsAnimation.SetTrigger("Grab");
        
        if (!CanInteract || ActualInteractiveObject == null) return;

        DialogueManager Dialogue = ActualInteractiveObject.GetComponent<DialogueManager>();
        if (Dialogue != null) Dialogue.StartDialogue();

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
            if (Focus != null) Focus.StartFocus(this);

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null) SceneChanger.LoadScene();
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null) Fish.Interact(this);

        Projector Skip = ActualInteractiveObject.GetComponent<Projector>();
        if (Skip != null) Skip.Avancar();

        Trash Lixo = ActualInteractiveObject.GetComponent<Trash>();
        if (Lixo != null) Lixo.Fora(this);
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

    public void SetInDialogue(bool value)
    {
        IsInDialogue = value;
    }
}