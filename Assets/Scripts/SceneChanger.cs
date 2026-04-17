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
                PlayerInteraction.IsConfirmationOpen = true;

                PlayerMovement Move = player.GetComponent<PlayerMovement>();
                if (Move != null) Move.StopMovement();

                PlayerWaterMovement WaterMove = player.GetComponent<PlayerWaterMovement>();
                if (WaterMove != null) WaterMove.StopWaterMovement();

            } 
            else
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }

    public void LoadScene()
    {
        PlayerInteraction.IsConfirmationOpen = false;

        SceneManager.LoadScene(SceneName);

        PlayerInteraction.IsConfirmationOpen = false;
    }
    
    public void CancelConfirmation()
    {
        if (SceneConfirmPanel != null) SceneConfirmPanel.SetActive(false);
        PlayerInteraction.IsConfirmationOpen = false;
        }
}
