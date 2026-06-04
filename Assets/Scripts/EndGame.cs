using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animator CutsceneAnimator;
    public void EndCutscene()
    {
        PlayerInteraction.IsSleeping = false;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FadeTransition.Instance.StartFade("Credits");
    }

}
