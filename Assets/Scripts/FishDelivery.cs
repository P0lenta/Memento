using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishDelivery : MonoBehaviour
{

    [Header("Requisitos")]
    public EmotionType RequiredEmotion;
    public EmotionType[] PossibleEmotion;

    [Header("Aparência")]
    public Renderer PedestalRenderer;
    public Material DefaultMaterial;
    public Material SuccessMaterial;
    public Image EmotionIcon;
    public Sprite[] EmotionSprite;

    [Header("Mensagens")]
    public TextMeshProUGUI MessageText;
    public float MessageDuration = 2f;
    private float MessageTimer = 0f;

    [Header("Mensagens das missões")]
    public string[] MissionsMessage = new string[]
    {
        "Sou hetero",
        "Sou gay",
        "Sou bi",
        "Sou brksedu"
    };

    [Header ("Mensagens das entregas")]
    public string[] CompleteMessage = new string[]
    {
        "Cura hetero",
        "Cura gay",
        "Cura bi",
        "Cura brksedu"
    };    

    private static int CurrentEmotionIndex = 0;

    void Start()
    {
        if (MessageText != null) MessageText.gameObject.SetActive(false);
        if (EmotionIcon != null) EmotionIcon.gameObject.SetActive(false);

        if (PossibleEmotion == null || PossibleEmotion.Length == 0)
        {
            PossibleEmotion = new EmotionType[]
            {
                EmotionType.Felicidade,
                EmotionType.Tristeza,
                EmotionType.Raiva,
                EmotionType.Medo,
                EmotionType.Surpresa
            };
        }

        if (EmotionManager.Instance.CurrentMissionFish == EmotionType.None)
        {
            SetRequiredEmotion();
            EmotionManager.Instance.CurrentMissionFish = RequiredEmotion;
        }
        else
        {
            RequiredEmotion = EmotionManager.Instance.CurrentMissionFish;
        }

        UpdateIcon();

    }

    void Update()
    {
        if (MessageTimer > 0)
        {
            MessageTimer -= Time.deltaTime;
            if(MessageTimer <= 0f)
            {
                if (MessageText != null) MessageText.gameObject.SetActive(false);
                if (EmotionIcon != null) EmotionIcon.gameObject.SetActive(false);
            }
        }
    }

    void SetRequiredEmotion()
    {
        if (PossibleEmotion.Length == 0) return;

        RequiredEmotion = PossibleEmotion[CurrentEmotionIndex];
        CurrentEmotionIndex = (CurrentEmotionIndex + 1) % PossibleEmotion.Length;
    }

    void UpdateIcon()
    {
        if (EmotionIcon != null && EmotionSprite != null && (int)RequiredEmotion < EmotionSprite.Length)
        {
            EmotionIcon.sprite = EmotionSprite[(int)RequiredEmotion];
        }
    }

    public void TryDeliver(PlayerInteraction player)
    {
        if (EmotionManager.Instance.CurrentMissionFish == EmotionType.None)
        {   
            if (CurrentEmotionIndex >= 0 && CurrentEmotionIndex < MissionsMessage.Length)
            {
                ShowMessage(MissionsMessage[CurrentEmotionIndex]);
            }
            else 
            {
                ShowMessage(CompleteMessage[CurrentEmotionIndex]);
            }
            
            SetRequiredEmotion();
            EmotionManager.Instance.CurrentMissionFish = RequiredEmotion;
            UpdateIcon();
    
            return;
        }

        if (player.HeldFishEmotion == EmotionManager.Instance.CurrentMissionFish)
        {
            player.SetHeldFish(EmotionType.None);
            EmotionManager.Instance.SetEmotion(EmotionType.None);
            
            ShowMessage("Obrigado pela entrega");

            if (PedestalRenderer != null && SuccessMaterial != null)
            PedestalRenderer.material = SuccessMaterial;

            EmotionManager.Instance.CurrentMissionFish = EmotionType.None;
            
            if (DefaultMaterial != null)
            {
                Invoke(nameof(RevertMaterial), 2f);
            }
        }
    
    }

    void RevertMaterial()
    {
        if (PedestalRenderer != null && DefaultMaterial != null)
        {
            PedestalRenderer.material = DefaultMaterial;
        }
    }

    void ShowMessage(string msg)
    {
        if (MessageText == null) return;

        MessageText.gameObject.SetActive(true);
        MessageText.text = msg;
        MessageTimer = MessageDuration;

        if (EmotionIcon == null) return;
        EmotionIcon.gameObject.SetActive(true);
        UpdateIcon();
    }

}
