using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public Transform AfterWaterSpawn;

    void Start()
    {
        if (EmotionManager.Instance == null) return;

        if (EmotionManager.Instance.LastScene == "Water") transform.position = AfterWaterSpawn.position;
    }
}
