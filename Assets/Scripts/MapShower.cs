using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapShower : MonoBehaviour
{
    [Header("Referências")]
    public GameObject MapBackground;
    public GameObject MapRadius;
    public GameObject MapHelper;
    public GameObject MapCompass;
    public RectTransform MapRadiusLocation;

    private bool IsMapOpen = false;

    [Header("Valores de X, Y e Z")]
    public Vector3[] FishLocations;

    [Header ("Ajudantes")]
    public TextMeshProUGUI HelperText;
    public string[] CollectFishMessages;
    public Sprite[] CollectImages;
    public Image HelperImage;

    void Start()
    {
        MapBackground.SetActive(false);
        if (MapRadius != null) MapRadius.SetActive(false);
        MapHelper.SetActive(false);
    }

    private void UpdateMapHelpers()
    {
        if (HelperImage == null || CollectImages.Length == 0) return;

        if (EmotionManager.Instance.IsTimeToBed)
        {
            HelperImage.sprite = CollectImages[1];
            if (HelperText != null) HelperText.text = CollectFishMessages[1];
            return;
        }


        if (!EmotionManager.Instance.IsMapUnlocked)
        {
            HelperImage.sprite = CollectImages[0];
            if (HelperText != null) HelperText.text = CollectFishMessages[0];
            return;
        }

        int MissionIndex = EmotionManager.Instance.CurrentMission;

        int HelperMissionIndex = MissionIndex + 2;

        if (HelperMissionIndex >= 0 && HelperMissionIndex < CollectImages.Length) 
        HelperImage.sprite = CollectImages[HelperMissionIndex];

        if (HelperText != null && 
        HelperMissionIndex >=0 && 
        MissionIndex < CollectFishMessages.Length) 
        HelperText.text = CollectFishMessages[HelperMissionIndex];
    }

    public void OpenMap()
    {
        if (IsMapOpen) 
        {
            CloseMap();
            return;
        }

        if (PlayerInteraction.IsInputLocked) return;

        UpdateMapHelpers();

        EmotionManager.Instance.WasMapOpened = true;

        MapTutorial mapTutorial = FindFirstObjectByType<MapTutorial>();
        if (mapTutorial != null) mapTutorial.OnMapOpened();

        IsMapOpen = true;

        PlayerInteraction.IsInMap = true;
        
        MapBackground.SetActive(true);
        if (MapRadius != null) MapRadius.SetActive(true);
        MapHelper.SetActive(true);
        if (MapCompass != null) MapCompass.SetActive(true);

        if (!EmotionManager.Instance.IsMapUnlocked)
        {
            if (MapRadius != null) MapRadiusLocation.localPosition = FishLocations[0];
        }
        else
        {
            int Index = EmotionManager.Instance.CurrentMission + 1;

            if (MapRadius != null) MapRadiusLocation.localPosition = FishLocations[Index];
        }

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.IsMovementLocked = true;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.IsInteracting = true;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    }

    public void CloseMap()
    {
        MapBackground.SetActive(false);
        if (MapRadius != null) MapRadius.SetActive(false);
        MapHelper.SetActive(false);
        if (MapCompass != null) MapCompass.SetActive(false);

        PlayerInteraction.IsInMap = false;

        IsMapOpen = false;

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.IsMovementLocked = false;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.IsInteracting = false;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();
    }

    public void CloseMapWithEsc()
    {
        if (IsMapOpen) CloseMap();
    }

}
