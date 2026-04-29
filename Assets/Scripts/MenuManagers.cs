using UnityEngine;
using UnityEngine.UI;

public class MenuManagers : MonoBehaviour
{
    [Header("Painéis de UI")]
    public GameObject ConfigPanel;
    public GameObject ConfirmExitPanel;
    public GameObject ButtonsMenu;

    [Header("Slider de Áudio")]
    public Slider MusicSlider;
    public Slider SFXSlider;

    void Start()
    {
        if (ConfigPanel != null) ConfigPanel.SetActive(false);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
        RefreshSlider();
    }

    public void ToggleMenu()
    {
        if (ConfigPanel == null)
        {
            return;
        }

        if (ConfigPanel.activeSelf)
        {
            CloseConfigPanel();
            return;
        }

        if (PlayerInteraction.IsInputLocked) return;

        OpenConfigPanel();
    }

    public void OpenConfigPanel()
    {
        if (ConfigPanel == null) return;

        RefreshSlider();

        ConfigPanel.SetActive(true);
        
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);

        PlayerInteraction.IsMenuOpen = true;
        PlayerInteraction.CanInteract = false;

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.GetSensibility();
            movement.IsInteracting = true;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.GetSensibility();
            watermovement.IsInteracting = true;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    }

    public void CloseConfigPanel()
    {
        if (ConfigPanel == null) return;
        ConfigPanel.SetActive(false);

        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);

        PlayerInteraction.IsMenuOpen = false;
        PlayerInteraction.CanInteract = true;

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.GetSensibility();
            movement.IsInteracting = false;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.GetSensibility();
            watermovement.IsInteracting = false;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    }

    public void OpenConfirmExitPanel()
    {
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(true);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(false);

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    }

    public void CloseConfirmExitPanel()
    {
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    }

    public void RefreshSlider()
    {
        if (MusicManager.Instance == null) return;

        if (MusicSlider != null)
            MusicSlider.SetValueWithoutNotify(MusicManager.Instance.CurrentMusicVolume);

        if (SFXSlider != null)
            SFXSlider.SetValueWithoutNotify(MusicManager.Instance.CurrentSFXVolume);
    }

    public void OnMusicSlider(float Value)
    {
        if (MusicManager.Instance != null) MusicManager.Instance.OnSliderMusic(Value);
    }

        public void OnSFXSlider(float Value)
    {
        if (MusicManager.Instance != null) MusicManager.Instance.OnSliderSFX(Value);
    }

}
