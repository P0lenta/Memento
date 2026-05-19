using UnityEngine;

public class Projector : MonoBehaviour
{
    public Sprite[] Frames;
    public float Intervalo = 5f;
    public TutorialTime tutorialTime;
    public SpriteRenderer Renderer;
    private int Index = 0;
    private float Timer;


    void Start()
    {
        if (Frames.Length > 0) Renderer.sprite = Frames[0];
        Timer = Intervalo;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            Avancar();
            Timer = Intervalo;
        }
    }

    public void Avancar()
    {
        if (Frames.Length == 0) return;
        Index = (Index + 1) % Frames.Length;
        Renderer.sprite = Frames[Index];
        Timer = Intervalo;

        if (Index == 0 && tutorialTime != null) tutorialTime.LiberateWalk();
    }
}
