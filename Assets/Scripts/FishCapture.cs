using UnityEngine;

public class FishCapture : MonoBehaviour
{
    public FishVisibility fishVisibility;

    void Start()
    {
        fishVisibility = GetComponent<FishVisibility>();
    }

    public void Interact(PlayerInteraction player)
    {
        if (fishVisibility != null) player.SetHeldFish(fishVisibility.EmotionAttached);

        gameObject.SetActive(false);
    }




}
