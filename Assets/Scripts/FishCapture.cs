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
        if (fishVisibility != null)
        {
            player.HeldFishEmotion = fishVisibility.EmotionAttached;
            Debug.Log($"Pegou um peixe {player.HeldFishEmotion}!");
        }
        else
        {
            Debug.Log("FishVisibility não encontrado");
        }

        gameObject.SetActive(false);
    }




}
