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


    [Header("flags")]
    public static PlayerInteraction Instance { get; private set; } 
    public static bool IsMenuOpen;
    public static bool IsConfirmationOpen { get; set; }
    public static bool IsInDialogue { get; set; }
    public static bool IsSleeping { get; set; }
    public static bool IsInMap { get; set; }
    public static bool CanInteract = false;
    private Renderer[] HandsRenderers;

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

        if (EmotionManager.Instance != null) 
        {
            HeldFishEmotion = EmotionManager.Instance.HeldFish;

            if (HeldFishEmotion != EmotionType.None)
            {
                bool isHard = EmotionManager.Instance.CurrentMission > 4;
                 Debug.Log($"Tentando restaurar peixe: {HeldFishEmotion}, isHard: {isHard}");
                foreach (var entry in FishModels)
                {
                     Debug.Log($"Entry: {entry.Emotion}, {entry.IsHard}, Model: {entry.Model?.name}");
                    if (entry.Emotion == HeldFishEmotion && entry.IsHard == isHard)
                    {
                        entry.Model.SetActive(true);
                        Debug.Log($"Modelo restaurado: {entry.Model.name}");
                        break;
                    }
                }
            }

        }
        HandsRenderers = HandsUI.GetComponentsInChildren<Renderer>();
        
        RefreshHandsUIVisibility();
        
        UpdateHoldingAnimation();

        SetInteractionImageVisible(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        Debug.Log($"OnHoldingAnimationEvent chamado! Emoção: {HeldFishEmotion}");
        
        if (HeldFishEmotion == EmotionType.None)
        {
            Debug.Log("Emoção é None, saindo...");
            return;
        }

        bool isHard = EmotionManager.Instance != null && EmotionManager.Instance.CurrentMission > 4;
        Debug.Log($"isHard: {isHard}");

        foreach (var entry in FishModels)
        {
            Debug.Log($"Checando entry: {entry.Emotion}, {entry.IsHard}, match: {entry.Emotion == HeldFishEmotion && entry.IsHard == isHard}");
            if (entry.Emotion == HeldFishEmotion && entry.IsHard == isHard)
            {
                entry.Model.SetActive(true);
                Debug.Log($"Modelo ativado: {entry.Model.name}");
            }
        }

        UpdateHoldingAnimation();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            Debug.Log("Disparando trigger Grab!");
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
    bool visible = !IsInputLocked;
    foreach (var rend in HandsRenderers)
    {
        if (rend != null)
            rend.enabled = visible;
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
}