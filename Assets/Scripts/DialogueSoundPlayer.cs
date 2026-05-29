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
    }

    void OnDisable() 
    {
        DialogueManager.OnLineChanged -= PlayLineSound;        
    }

    void PlayLineSound(DialogueManager.DialogueOption Option, int LineIndex)
    {
        if (Dialogue.GetCurrentDialogueOption() != Option) return;

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

}
