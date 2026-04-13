using UnityEngine;

public class OxygenGiver : MonoBehaviour
{
    public OxygenManager oxygenManager;
    public float OxygenToAdd = 25f;

     private void OnTriggerEnter (Collider other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (!other.CompareTag("Player")) return;

        if (oxygenManager == null) return;

            oxygenManager.CurrentOxygen = Mathf.Min(
                oxygenManager.CurrentOxygen + OxygenToAdd, oxygenManager.MaxOxygen);

            gameObject.SetActive(false);

    }
}

