using UnityEngine;

public class Bed : MonoBehaviour
{

    public AudioSource audioSource;
    public void Sleep()
    {
        audioSource.Play();

        BedCounter CodeStarter = FadeTransition.Instance.GetComponent<BedCounter>();
        if (CodeStarter != null) CodeStarter.Sleep();
    }
}
