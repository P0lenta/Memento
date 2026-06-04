using UnityEngine;

public class DialogueSoundPlayer : MonoBehaviour
{
    [Header("Referências")]
    public AudioSource AudioPlayer;
    public DialogueManager Dialogue;
    public AudioClip DefaultLineSound;

    public DialogueSoundSet[] SoundSets;

    [System.Serializable]
    public class DialogueSoundSet
    {
        public AudioClip[] LineSounds;
    }

    void OnEnable() 
    {
        DialogueManager.OnLineChanged += PlayLineSound;
        DialogueManager.OnDialogueEnded += StopLineSound;
    }

    void OnDisable() 
    {
        DialogueManager.OnLineChanged -= PlayLineSound;
        DialogueManager.OnDialogueEnded -= StopLineSound;   
    }

    void PlayLineSound(DialogueManager.DialogueOption Option, int LineIndex)
    {
        if (Dialogue == null || !Dialogue.IsActive) return;

        bool ThisDialogue = System.Array.IndexOf(Dialogue.Dialogues, Option) >= 0;

        if (!ThisDialogue) return;

            int OptionIndex = System.Array.IndexOf(Dialogue.Dialogues, Option);
            if (OptionIndex >= 0 && OptionIndex < SoundSets.Length)
            {
                DialogueSoundSet set = SoundSets[OptionIndex];
                if (LineIndex < set.LineSounds.Length && set.LineSounds[LineIndex] != null)
                {
                    AudioPlayer.PlayOneShot(set.LineSounds[LineIndex]);
                    return;
                }
            }

        if (DefaultLineSound != null) AudioPlayer.PlayOneShot(DefaultLineSound);
    }   

    void StopLineSound(DialogueManager.DialogueOption Option)
    {
        if (AudioPlayer != null) AudioPlayer.Stop();
    }

}
