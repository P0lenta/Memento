using UnityEngine;

public class Trash : MonoBehaviour
{
    public void Fora (PlayerInteraction player)
    {
        if (player != null && player.HeldFishEmotion != EmotionType.None)
        player.SetHeldFish(EmotionType.None);
        Debug.Log("Peixe descartado");
    }

}
