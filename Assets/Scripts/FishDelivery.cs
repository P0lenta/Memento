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

    void Start()
    {
        if (MessageText != null) MessageText.gameObject.SetActive(false);
        if (EmotionIcon != null) EmotionIcon.gameObject.SetActive(false);

        if (PossibleEmotion == null || PossibleEmotion.Length == 0)
        {
            PossibleEmotion = new EmotionType[]
            {
                EmotionType.Happy,
                EmotionType.Sad,
                EmotionType.Angry,
                EmotionType.Fear,
                EmotionType.Surprised
            };
        }

        if (EmotionManager.Instance.CurrentMissionFish == EmotionType.None)
        {
            SetRandomRequiredEmotion();
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
        if (RequiredEmotion == EmotionType.None)
        {
            SetRandomRequiredEmotion();
            EmotionManager.Instance.CurrentMissionFish = RequiredEmotion;
            UpdateIcon();
        }

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

    void SetRandomRequiredEmotion()
    {
        if (PossibleEmotion.Length == 0) return;
        int index = Random.Range(0, PossibleEmotion.Length);
        RequiredEmotion = PossibleEmotion[index];
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
            SetRandomRequiredEmotion();
            EmotionManager.Instance.CurrentMissionFish = RequiredEmotion;
            UpdateIcon();
            ShowMessage($"Preciso do peixe {RequiredEmotion}");
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
        else
        {
            ShowMessage($"Preciso do peixe {EmotionManager.Instance.CurrentMissionFish}");
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
