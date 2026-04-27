using UnityEngine;
using UnityEngine.UI;

public class MenuManagers : MonoBehaviour
{
    [Header("Referências de UI")]
    public Slider MusicSlider;
    public Slider SFXSlider;
    public GameObject ConfigPanel;

    void Start()
    {
        if (ConfigPanel != null) ConfigPanel.SetActive(false);

        SetupSlider();
    }

    public void ToggleMenu()
    {
        if (ConfigPanel == null) return;

        bool IsOpening = !ConfigPanel.activeSelf;
        ConfigPanel.SetActive(IsOpening);

        if (IsOpening) SetupSlider();
    }

    public void SetupSlider()
    {
        if (MusicManager.Instance == null) return;

        if (MusicSlider != null) MusicManager.Instance.SetupSlider(MusicSlider, true);

        if (SFXSlider != null) MusicManager.Instance.SetupSlider(SFXSlider, true);
    }

}
