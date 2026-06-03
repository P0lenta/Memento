using UnityEngine;

public class TesteFog : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        Debug.Log($"Fog: {RenderSettings.fog} | Mode: {RenderSettings.fogMode} | Density: {RenderSettings.fogDensity} | Start: {RenderSettings.fogStartDistance} | End: {RenderSettings.fogEndDistance}"
);
    }
}
