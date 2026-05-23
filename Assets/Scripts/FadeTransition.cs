using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public Image FadeImage;
    public float FadeDuration = 1.5f;
    private string SceneToChange;
    public static FadeTransition Instance {get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy (gameObject);
            return;
        }
        Instance = this;
        {
            DontDestroyOnLoad (gameObject);
        }
    }

    void Start()
    {
        if (FadeImage != null) FadeImage.color = new Color(0, 0, 0, 0);
    }

    public void StartFade(string SceneName)
    {
        SceneToChange = SceneName;
        StartCoroutine(FadeLoad());
    }

    IEnumerator FadeLoad()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        
        if (EmotionManager.Instance != null)
        {
            EmotionManager.Instance.LastScene = SceneManager.GetActiveScene().name;
            EmotionManager.Instance.Save();
        }

        SceneManager.LoadScene(SceneToChange);
        SceneToChange = null;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float From, float To)
    {
        float TimePassed = 0f;
        Color ColorUsed = FadeImage.color;

        while (TimePassed < FadeDuration)
        {
            TimePassed += Time.deltaTime;
            ColorUsed.a = Mathf.Lerp(From, To, TimePassed / FadeDuration);
            FadeImage.color = ColorUsed;
            yield return null;
        }

        ColorUsed.a = To;
        FadeImage.color = ColorUsed;
    }
}
