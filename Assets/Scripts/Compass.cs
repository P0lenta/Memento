using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform PlayerTransform;
    public RectTransform CompassRectTransform;

    void Update()
    {
        float Angle = PlayerTransform.eulerAngles.y;
        CompassRectTransform.rotation = Quaternion.Euler(0, 0, (-Angle));    
    }
}
