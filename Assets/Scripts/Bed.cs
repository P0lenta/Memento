using UnityEngine;

public class Bed : MonoBehaviour
{

    public void Sleep()
    {
        BedCounter CodeStarter = FadeTransition.Instance.GetComponent<BedCounter>();
        if (CodeStarter != null) CodeStarter.Sleep();
    }
}
