    using UnityEngine;
    using TMPro;

    public class DialogueManager : MonoBehaviour
    {

        [System.Serializable]
        public class DialogueOption
        {
            [Tooltip("Frases do diálogo")]
            public string[] lines;

            [Tooltip("Índice da missão necessária")]
            public int RequiredMissionIndex = -1;

            [Tooltip("Emoção necessária do jogador")]
            public EmotionType RequiredEmotion = EmotionType.None;

            [Tooltip("Peixe segurado necessário")]
            public EmotionType RequiredHeldFish = EmotionType.None;

            [Tooltip("Dia necessário")]
            public int RequiredDayIndex = -1;

            [Tooltip("Nenhuma condição anterior é verdadeira")]
            public bool IsExtra = false;    

            [Tooltip("Completa a missão")]
            public bool CompletesMission = false;
        }

        [Header("Diálogos")]
        public DialogueOption[] Dialogues;

        [Header("Configurações de UI")]
        public TextMeshProUGUI DialogueText;
        public GameObject DialogueImage;

        private int CurrentLine = 0;
        private bool IsActive = false;
        private static DialogueOption ActiveDialogue;
        private static DialogueManager CurrentDialogue;

        public void StartDialogue()
        {

            if (CurrentDialogue != null) CurrentDialogue.EndDialogue();

            CurrentDialogue = this;

            DialogueOption Chosen = GetRightDialogue();
            if (Chosen == null ||  Chosen.lines.Length == 0) return;

            ActiveDialogue = Chosen;
            CurrentLine = 0;
            IsActive = true;
            ShowCurrentLine();
            PlayerInteraction.IsInDialogue = true;
        }

        private DialogueOption GetRightDialogue()
        {

            int CurrentMission = EmotionManager.Instance != null ? EmotionManager.Instance.CurrentMission : -1;
            EmotionType PlayerEmotion = EmotionManager.Instance.GetCurrentEmotion();
            EmotionType HeldFish = PlayerInteraction.Instance != null ? PlayerInteraction.Instance.HeldFishEmotion : EmotionType.None;
            int CurrentDay = EmotionManager.Instance != null ? EmotionManager.Instance.CurrentDay : -1;

            for (int i = 0; i < Dialogues.Length; i++)
            {
                DialogueOption Didi = Dialogues[i];
                if (!Didi.IsExtra)
                {
                    bool MissionOk = (Didi.RequiredMissionIndex == -1 || Didi.RequiredMissionIndex == CurrentMission);
                    bool EmotionOk = (Didi.RequiredEmotion == EmotionType.None || Didi.RequiredEmotion == PlayerEmotion);
                    bool FishOk = (Didi.RequiredHeldFish == EmotionType.None || Didi.RequiredHeldFish == HeldFish);
                    bool DayOk = (Didi.RequiredDayIndex == -1 || Didi.RequiredDayIndex == 0 || Didi.RequiredDayIndex == CurrentDay);
                    
                    if (MissionOk && EmotionOk && FishOk && DayOk)
                        return Didi;
                }
            }

            for (int i = 0; i < Dialogues.Length; i++)
            {
                if (Dialogues[i].IsExtra) return Dialogues[i];
            }

            return Dialogues.Length > 0 ? Dialogues[0] : null;

        }

        private void ShowCurrentLine()
        {
            if (DialogueText ==  null) return;
            if (DialogueImage ==  null) return;

            DialogueText.text = ActiveDialogue.lines[CurrentLine];
            DialogueText.gameObject.SetActive(true);
            DialogueImage.gameObject.SetActive(true);
        }

        public void NextLine()
        {
            if (!IsActive) return;
            CurrentLine++;
            if (CurrentLine >= ActiveDialogue.lines.Length) EndDialogue();
            else ShowCurrentLine();
        }

        public static void CheckNextLine()
        {
            if (CurrentDialogue != null) CurrentDialogue.NextLine();
        }

        private void EndDialogue()
        {
            IsActive = false;
            if (DialogueText == null) return;
            if (DialogueImage == null) return;
            DialogueText.gameObject.SetActive(false);
            DialogueImage.gameObject.SetActive(false);

            PlayerInteraction.IsInDialogue = false;

            CameraFocus Camera = GetComponent<CameraFocus>();
            if (Camera != null) Camera.EndFocus();

            Interactable Interaction = GetComponent<Interactable>();
            if (Interaction != null) Interaction.enabled = false;

            EmotionGiver giver = GetComponent<EmotionGiver>();
            if (giver != null)
            {
                EmotionManager.Instance.SetEmotion(giver.EmotionToGive);
            }

            if (ActiveDialogue != null && ActiveDialogue.CompletesMission)
            {
                FishDelivery Delivery = GetComponent<FishDelivery>();
                if (Delivery != null) Delivery.CompleteDelivery();
            }

            CurrentDialogue = null;
        }

    }
