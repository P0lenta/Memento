using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Tipo de peixe capturado")]
    public EmotionType HeldFishEmotion = EmotionType.None;

    [Header("Referências")]
    public GameObject ActualInteractiveObject = null;
    public GameObject focusedObject = null;
    public FishModelEntry[] FishModels;
    public GameObject HandsUI;
    public Animator HandsAnimation;
    public MenuManagers MenuManager;
    public MapShower MapController;
    public GameObject InteractionImage;
    private AudioSource CurrentInteractionSound;


    [Header("flags")]
    public static PlayerInteraction Instance { get; private set; } 
    public static bool IsMenuOpen;
    public static bool IsConfirmationOpen { get; set; }
    public static bool IsInDialogue { get; set; }
    public static bool IsSleeping { get; set; }
    public static bool IsInMap { get; set; }
    public static bool CanInteract = false;
    private Renderer[] HandsRenderers;

    [Header("Distância para interagir")]
    public float InteractionDistance = 1.5f;

    public static bool IsInputLocked 
    {
        get
        {
            return IsMenuOpen || IsConfirmationOpen || IsInDialogue || IsSleeping || IsInMap;
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
        IsMenuOpen = false;
        IsConfirmationOpen = false;
        IsInDialogue = false;
        IsSleeping = false;
        CanInteract = true;

        if (EmotionManager.Instance != null) HeldFishEmotion = EmotionManager.Instance.HeldFish;

        HandsRenderers = HandsUI.GetComponentsInChildren<Renderer>();
        
        RefreshHandsUIVisibility();
        
        UpdateHoldingAnimation();

        SetInteractionImageVisible(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (HeldFishEmotion != EmotionType.None) CheckActualFish();

        if (!IsInputLocked) CheckInteractable();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (MenuManager != null) MenuManager.ToggleMenu();

        if (MapController != null) MapController.CloseMapWithEsc();
    }

    public void OnTrash(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (HeldFishEmotion != EmotionType.None)
        SetHeldFish(EmotionType.None);
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (MapController != null) MapController.OpenMap();
    }

    public void SetHeldFish(EmotionType Fish)
    {

        HeldFishEmotion = Fish;

        if (EmotionManager.Instance != null) EmotionManager.Instance.HeldFish = Fish;

        foreach (var entry in FishModels) entry.Model.SetActive(false);

        if (Fish == EmotionType.None) UpdateHoldingAnimation();
    }

    public void OnHoldingAnimationEvent()
    {
        if (HeldFishEmotion == EmotionType.None) return;

        bool isHard = EmotionManager.Instance != null && EmotionManager.Instance.CurrentMission > 4;

        foreach (var entry in FishModels)
        {
            if (entry.Emotion == HeldFishEmotion && entry.IsHard == isHard) entry.Model.SetActive(true);
        }

        UpdateHoldingAnimation();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
            if (InteractSound != null && InteractSound.InteractionSound != null)
            {
                CurrentInteractionSound = InteractSound.InteractionSound;
                CurrentInteractionSound.Play();
            }
        }
        
        if (!CanInteract || ActualInteractiveObject == null) return;

        SetInteractionImageVisible(false);

        DialogueManager Dialogue = ActualInteractiveObject.GetComponent<DialogueManager>();
        if (Dialogue != null) Dialogue.StartDialogue();

        CameraFocus Focus = ActualInteractiveObject.GetComponent<CameraFocus>();
        if (Focus != null) Focus.StartFocus(this);

        SceneChanger SceneChanger = ActualInteractiveObject.GetComponent<SceneChanger>();
        if (SceneChanger != null) SceneChanger.TryChangeScene(this);
            
        FishCapture Fish = ActualInteractiveObject.GetComponent<FishCapture>();
        if (Fish != null) 
        {
            Fish.Interact(this);
            HandsAnimation.SetTrigger("Grab");
        }

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

    public void RefreshHandsUIVisibility()
    {
        foreach (var Rend in HandsRenderers)
        {
            if (Rend != null) Rend.enabled = !IsInputLocked;
        }
    }

    public void SetInDialogue(bool value)
    {
        IsInDialogue = value;
    }

    public void SetInteractionImageVisible(bool visible)
    {
        if (InteractionImage != null) InteractionImage.SetActive(visible);
    }

    public void StopCurrentInteractionSound()
    {
        if (CurrentInteractionSound != null && CurrentInteractionSound.isPlaying) CurrentInteractionSound.Stop();
        CurrentInteractionSound = null;
    }

    void CheckActualFish()
    {
        bool isHard = EmotionManager.Instance != null && EmotionManager.Instance.CurrentMission > 4;

        foreach (var Entry in FishModels)
        {
            if (Entry.Emotion == HeldFishEmotion && Entry.IsHard == isHard)
            {
                Entry.Model.SetActive(true);
                return;
            }
        }
    }

    void CheckInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, InteractionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null && interactable.enabled && interactable.gameObject.activeInHierarchy)
            {
                if (ActualInteractiveObject != interactable.gameObject && ActualInteractiveObject != null)
                {
                    Interactable interactable_anterior = ActualInteractiveObject.GetComponent<Interactable>();
                    if (interactable_anterior != null) interactable_anterior.OnLostFocus();    
                }
                ActualInteractiveObject = interactable.gameObject;
                CanInteract = true;
                interactable.OnGainFocus();

                return;
            }

        }

        if (ActualInteractiveObject != null)
        {
            Interactable interactable_anterior = ActualInteractiveObject.GetComponent<Interactable>();
            if (interactable_anterior != null) interactable_anterior.OnLostFocus();   
            ActualInteractiveObject = null;
            CanInteract = false;
        }
    }
}