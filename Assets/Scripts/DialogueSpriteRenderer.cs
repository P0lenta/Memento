using UnityEngine;

public class DialogueSpriteRenderer : MonoBehaviour
{
    [Header("Referências")]
    public SpriteRenderer Renderer;
    public DialogueManager Dialogue;

    public SpriteRendererSet[] SpriteSet;

    private Sprite OriginalSprite;

    [System.Serializable]
    public class SpriteRendererSet
    {
        public Sprite[] LineSprites;
    }

    void OnEnable() 
    {
        DialogueManager.OnLineChanged += PlayLineSprite;
        DialogueManager.OnDialogueEnded += ResetSprite;
    }

    void OnDisable() 
    {
        DialogueManager.OnLineChanged -= PlayLineSprite;
        DialogueManager.OnDialogueEnded -= ResetSprite; 
    }

    void PlayLineSprite(DialogueManager.DialogueOption Option, int LineIndex)
    {
        if (Dialogue.GetCurrentDialogueOption() != Option) return;

        if (OriginalSprite == null) OriginalSprite = Renderer.sprite;

        int OptionIndex = System.Array.IndexOf(Dialogue.Dialogues, Option);
        if (OptionIndex < 0 || OptionIndex >= SpriteSet.Length) return;

        SpriteRendererSet set = SpriteSet[OptionIndex];

        if (LineIndex < set.LineSprites.Length && set.LineSprites[LineIndex] != null)
        {
            Renderer.sprite = set.LineSprites[LineIndex];
        }
    }

    void ResetSprite(DialogueManager.DialogueOption Option)
    {
        if (OriginalSprite != null)
        {
            Renderer.sprite = OriginalSprite;
            OriginalSprite = null;
        }
        else
        {
            Renderer.sprite = null;
        }
    }

}
