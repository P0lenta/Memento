using UnityEngine;

public class Bomb : MonoBehaviour
{
    
     private void OnTriggerEnter (Collider other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (!other.CompareTag("Player")) return;

        OxygenManager die = other.GetComponent<OxygenManager>();
        die.Die();

    }


}
