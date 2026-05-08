using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public bool Require = false;
    public GameObject SceneConfirmPanel;

    [Header("Cena")]
    public string SceneName;

    public void TryChangeScene(PlayerInteraction player)
    {

        if (!Require)
        {
            LoadScene();
            return;
        }

        if (Require)
        {
            EmotionType CurrentEmotion = EmotionManager.Instance.GetCurrentEmotion();

            if (CurrentEmotion == EmotionType.None)
            {
                if (SceneConfirmPanel != null) SceneConfirmPanel.SetActive(true);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                PlayerInteraction.IsConfirmationOpen = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PlayerMovement Move = player.GetComponent<PlayerMovement>();
                if (Move != null) Move.StopMovement();

                PlayerWaterMovement WaterMove = player.GetComponent<PlayerWaterMovement>();
                if (WaterMove != null) WaterMove.StopWaterMovement();

            } 
            else
            {
                SceneManager.LoadScene(SceneName);
                if (MusicManager.Instance != null) MusicManager.Instance.LoadSoundMenu();
            }
        }
    }

    public void LoadScene()
    {
        PlayerInteraction.IsConfirmationOpen = false;

        if (EmotionManager.Instance != null) EmotionManager.Instance.LastScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(SceneName);

        PlayerInteraction.IsConfirmationOpen = false;

        if (MusicManager.Instance != null) MusicManager.Instance.LoadSoundMenu();

        EmotionManager.Instance.Save();
    }
    
    public void CancelConfirmation()
    {
        if (SceneConfirmPanel != null) SceneConfirmPanel.SetActive(false);
        PlayerInteraction.IsConfirmationOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
