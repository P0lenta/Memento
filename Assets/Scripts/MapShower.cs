using UnityEngine;

public class MapShower : MonoBehaviour
{
    [Header("Referências")]
    public GameObject MapBackground;
    public GameObject MapRadius;
    public RectTransform MapRadiusLocation;

    private bool IsMapOpen = false;

    [Header("Valores de X, Y e Z")]
    public Vector3[] FishLocations;
    
    void Start()
    {
        MapBackground.SetActive(false);
        MapRadius.SetActive(false);
    }

    public void OpenMap()
    {
        if (IsMapOpen) 
        {
            CloseMap();
            return;
        }

        if (PlayerInteraction.IsInputLocked) return;

        IsMapOpen = true;

        PlayerInteraction.IsInMap = true;
        
        MapBackground.SetActive(true);
        MapRadius.SetActive(true);

        if (!EmotionManager.Instance.IsMapUnlocked)
        {
            MapRadiusLocation.localPosition = FishLocations[0];
        }
        else
        {
            int Index = EmotionManager.Instance.CurrentMission + 1;

            MapRadiusLocation.localPosition = FishLocations[Index];
        }

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.IsInteracting = true;
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
        MapRadius.SetActive(false);

        PlayerInteraction.IsInMap = false;

        IsMapOpen = false;

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.IsInteracting = false;
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
