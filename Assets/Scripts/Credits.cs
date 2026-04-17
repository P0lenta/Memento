using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float ScrollSpeed = 100f;
    private RectTransform rectTransform;
    public float MaxY = 1100f;
    private string NextScene = "Menu";

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnSkip (InputAction.CallbackContext context)
    {
        if (!context.started) return;
        SceneManager.LoadScene(NextScene);
    }

    void Update()
    {   
        rectTransform.anchoredPosition += new Vector2 (0, ScrollSpeed * Time.deltaTime);

        if (rectTransform.anchoredPosition.y >= MaxY) SceneManager.LoadScene(NextScene);
    }
}
