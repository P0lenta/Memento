using UnityEngine;
using UnityEngine.UI;

public class MenuManagers : MonoBehaviour
{
    [Header("Painéis de UI")]
    public GameObject ConfigPanel;
    public GameObject ConfirmExitPanel;
    public GameObject AudioPanel;
    public GameObject ControlPanel;
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
        
        if (EmotionManager.Instance != null) EmotionManager.Instance.Save();

        BackToMain();
        RefreshSlider();

        ConfigPanel.SetActive(true);

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);

        PlayerInteraction.IsMenuOpen = true;
        PlayerInteraction.CanInteract = false;

        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovement();
            movement.GetSensibility();
            movement.IsMovementLocked = true;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.GetSensibility();
            watermovement.IsInteracting = true;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

        RefreshCursor();
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
            movement.IsMovementLocked = false;
        }

        PlayerWaterMovement watermovement = FindFirstObjectByType<PlayerWaterMovement>();
        if (watermovement != null)
        {
            watermovement.StopWaterMovement();
            watermovement.GetSensibility();
            watermovement.IsInteracting = false;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

        RefreshCursor();

    }

    public void OpenConfirmExitPanel()
    {
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(true);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(false);

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

        RefreshCursor();

    }

    public void CloseConfirmExitPanel()
    {
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

        RefreshCursor();

    }

    public void OpenAudioPanel()
    {
        if (AudioPanel != null) AudioPanel.SetActive(true);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(false);
        if (ControlPanel != null) ControlPanel.SetActive(false);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
    }

        public void OpenControlPanel()
    {
        if (AudioPanel != null) AudioPanel.SetActive(false);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(false);
        if (ControlPanel != null) ControlPanel.SetActive(true);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
    }

    public void BackToMain()
    {
        if (AudioPanel != null) AudioPanel.SetActive(false);
        if (ButtonsMenu != null) ButtonsMenu.SetActive(true);
        if (ControlPanel != null) ControlPanel.SetActive(false);
        if (ConfirmExitPanel != null) ConfirmExitPanel.SetActive(false);
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

    private void RefreshCursor()
    {
        if (PlayerInteraction.Instance == null) return;

        bool MenuAberto = PlayerInteraction.IsMenuOpen || (ConfirmExitPanel != null && ConfirmExitPanel.activeSelf);

        Cursor.lockState = MenuAberto ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = MenuAberto;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        if (EmotionManager.Instance != null) EmotionManager.Instance.Load();

        CloseConfigPanel();
    }

}
