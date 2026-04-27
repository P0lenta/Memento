using UnityEngine;

public class Bed : MonoBehaviour
{
    public BedCounter bedCounter;

    public void Sleep()
    {
        bedCounter.Sleep();
    }
}
