using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Emoção capturada")]
    public EmotionType HeldFishEmotion = EmotionType.None;

    [Header("Referências")]
    public GameObject ConfigPanel;
    public GameObject ButtonsMenu;
    public GameObject ConfirmBox;
    public MenuManagers MenuManager;


    [Header("flags")]
    public bool CanInteract = false;
    public GameObject ActualInteractiveObject = null;
    public GameObject focusedObject = null;
    public GameObject HandsUI;
    public Animator HandsAnimation;
    public static PlayerInteraction Instance {get; private set;}
    public static bool IsMenuOpen {get; private set;}
    public static bool IsConfirmationOpen { get; set; }
    public static bool IsInDialogue { get; set; }
    public static bool IsSleeping { get; set; }

    public static bool IsInputLocked 
    {
        get
        {
            return IsMenuOpen || IsConfirmationOpen || IsInDialogue || IsSleeping;
        }
    }

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

    void Start()
    {
        if (EmotionManager.Instance != null)
        HeldFishEmotion = EmotionManager.Instance.HeldFish;
        
        UpdateHoldingAnimation();

        ConfigPanel.SetActive(false);
        IsMenuOpen = false;
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.started || ConfigPanel == null || MenuManager == null) return;
        
        if (IsInputLocked && !IsMenuOpen) return;

        if (MenuManager != null)
        {
            MenuManager.ToggleMenu();
            IsMenuOpen = ConfigPanel.activeSelf;
        }
        else if (ConfigPanel != null)
        {
            IsMenuOpen = !ConfigPanel.activeSelf;
            ConfigPanel.SetActive(IsMenuOpen);
        }

        if (ConfigPanel.activeSelf)
        {
            CloseMenu();
            return;
        }

        bool OpenMenu = !ConfigPanel.activeSelf;
        ConfigPanel.SetActive(OpenMenu);
        
        IsMenuOpen = OpenMenu;
        CanInteract = !OpenMenu;

        HandsUI.SetActive(!OpenMenu);

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

        if (OpenMenu)
        {
            if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
            if (ConfirmBox != null) ConfirmBox.SetActive(false);

            if (MusicManager.Instance != null) MusicManager.Instance.LoadSoundMenu();
        }
        else
        {
            if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
            if (ConfirmBox != null) ConfirmBox.SetActive(false);
        }

    }

    public void CloseMenu()
    {
        if (ConfigPanel != null && ConfigPanel.activeSelf)
        {
            if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
            if (ConfirmBox != null) ConfirmBox.SetActive(false);
            ConfigPanel.SetActive(false);
            IsMenuOpen = false;
            CanInteract = true;
            HandsUI.SetActive(true);

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

        if (GetComponent<PlayerWaterMovement>()?.IsDead == true) return;

        if (IsInDialogue)
        {
            DialogueManager.CheckNextLine();
            return;
        }

        if (IsInputLocked) return;

        if (focusedObject != null)
        {
            CameraFocus focus = focusedObject.GetComponent<CameraFocus>();
            if (focus != null)
                focus.EndFocus();
            return;
        }

        if (ActualInteractiveObject != null)
        {
            Interactable InteractSound = ActualInteractiveObject.GetComponent<Interactable>();
            if (InteractSound != null && InteractSound.InteractionSound != null) InteractSound.InteractionSound.Play();
        }

        HandsAnimation.SetTrigger("Grab");
        
        if (!CanInteract || ActualInteractiveObject == null) return;

        DialogueManager Dialogue = ActualInteractiveObject.GetComponent<DialogueManager>();
        if (Dialogue != null) Dialogue.StartDialogue();

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
        if (Focus != null) Focus.StartFocus(this);

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null) SceneChanger.TryChangeScene(this);
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null) Fish.Interact(this);

        Projector Skip = ActualInteractiveObject.GetComponent<Projector>();
        if (Skip != null) Skip.Avancar();

        Trash Lixo = ActualInteractiveObject.GetComponent<Trash>();
        if (Lixo != null) Lixo.Fora(this);

        Bed Sleepy = ActualInteractiveObject.GetComponent<Bed>();
        if (Sleepy != null)
        {
            PlayerMovement Movement = GetComponent<PlayerMovement>();
            if (Movement != null) Movement.StopMovement();
            Sleepy.Sleep();
        }

        Door OpenningDoor = ActualInteractiveObject.GetComponent<Door>();
        if (OpenningDoor != null) OpenningDoor.OpenDoor();
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