    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using System.Collections; 

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

            [Tooltip("Emoção que vai ser dada")]
            public EmotionType EmotionToGive = EmotionType.None;

            [Tooltip("Ele vai liberar o mapa")]
            public bool UnlocksMap = false;

            [Tooltip("É o texto para dormir")]
            public bool BedTime = false;
        }

        [Header("Diálogos")]
        public DialogueOption[] Dialogues;

        [Header("Configurações de UI")]
        public TextMeshProUGUI DialogueText;    
        public GameObject DialogueImage;
        public Image SkipProgressImage;

        [Header ("Animação máquina de escrever")]
        public float TypingSpeed = 0.05f;
        public bool CanSkip = true;

        public static DialogueOption CurrentDialogueOption { get; private set; }
        public static int CurrentDialogueLine { get; private set; }
        public static bool IsDialogueActive => CurrentDialogue != null;

        public static event System.Action<DialogueOption> OnDialgueStarted;
        public static event System.Action<DialogueOption, int> OnLineChanged;
        public static event System.Action<DialogueOption> OnDialogueEnded;

        private Coroutine TypingCoroutine;
        private string FullText;
        private bool IsTyping = false;
        public int CurrentLine = 0;
        public bool IsActive = false;
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

            OnDialgueStarted?.Invoke(ActiveDialogue);

            PlayerInteraction.Instance?.RefreshHandsUIVisibility();
            PlayerInteraction.Instance?.SetInteractionImageVisible(false);

            CurrentDialogue.SkipProgressImage.fillAmount = 0f;

            if (Chosen.UnlocksMap && EmotionManager.Instance != null) EmotionManager.Instance.IsMapUnlocked = true;
            if (Chosen.BedTime && EmotionManager.Instance != null) EmotionManager.Instance.IsTimeToBed = true;
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
            if (DialogueText ==  null || DialogueImage ==  null) return;

            CurrentDialogueLine = CurrentLine;
            OnLineChanged?.Invoke(ActiveDialogue, CurrentLine);

            FullText = ActiveDialogue.lines[CurrentLine];

            if (TypingCoroutine != null) StopCoroutine(TypingCoroutine);
            TypingCoroutine = StartCoroutine(TypeWriterEffect());

            DialogueText.text = ActiveDialogue.lines[CurrentLine];
            DialogueText.gameObject.SetActive(true);
            DialogueImage.gameObject.SetActive(true);
        }

        private IEnumerator TypeWriterEffect()
        {
            IsTyping = true;
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.text = FullText;
            DialogueText.gameObject.SetActive(true);
            DialogueImage.SetActive(true);

            for (int i = 1; i <= FullText.Length; i++)
            {
                DialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(TypingSpeed);
            }

            IsTyping = false;
            TypingCoroutine = null;

            if (!CanSkip)
            {
                yield return new WaitForSeconds(1f);
                NextLine(true);
            }
        }

        public void NextLine(bool Auto = false)
        {
            if (!IsActive) return;

            if (IsTyping && CanSkip)
            {
                SkipTyping();
                return;
            }

            if (!CanSkip && !Auto) return;

            CurrentLine++;
            if (CurrentLine >= ActiveDialogue.lines.Length) EndDialogue();
            else ShowCurrentLine();
        }

        private void SkipTyping()
        {
            if (TypingCoroutine != null) StopCoroutine(TypingCoroutine);

            DialogueText.maxVisibleCharacters = FullText.Length;
            IsTyping = false;
            TypingCoroutine = null;
        }

        public static void CheckNextLine()
        {
            if (CurrentDialogue != null) CurrentDialogue.NextLine();
        }

        public DialogueOption GetCurrentDialogueOption()
        {
            return ActiveDialogue;
        }

        private void EndDialogue()
        {
            IsActive = false;
            if (DialogueText != null) DialogueText.gameObject.SetActive(false);
            if (DialogueImage != null) DialogueImage.gameObject.SetActive(false);

            PlayerInteraction.IsInDialogue = false;
            PlayerInteraction.Instance?.RefreshHandsUIVisibility();

            CameraFocus Camera = GetComponent<CameraFocus>();
            if (Camera != null) Camera.EndFocus();

            if (ActiveDialogue != null && ActiveDialogue.EmotionToGive != EmotionType.None) EmotionManager.Instance.SetEmotion(ActiveDialogue.EmotionToGive);

            if (ActiveDialogue != null && ActiveDialogue.CompletesMission)
            {
                FishDelivery Delivery = GetComponent<FishDelivery>();
                if (Delivery != null) Delivery.CompleteDelivery();
            }

            PlayerInteraction.Instance?.StopCurrentInteractionSound();

            CurrentDialogue = null;

            OnDialogueEnded?.Invoke(ActiveDialogue);
            CurrentDialogueOption = null;
            CurrentDialogueLine = -1;
            CurrentDialogue = null;
            ActiveDialogue = null;
        }

        public static void UpdateSkipProgress(float Fill)
        {
            if (CurrentDialogue != null && !CurrentDialogue.CanSkip) return;
            if (CurrentDialogue?.SkipProgressImage != null) CurrentDialogue.SkipProgressImage.fillAmount = Fill;
        }

        public static void CancelSkip()
        {
            if (CurrentDialogue?.SkipProgressImage != null) CurrentDialogue.SkipProgressImage.fillAmount = 0f;
        }

        public static void SkipDialogue()
        {
            if (CurrentDialogue != null && !CurrentDialogue.CanSkip) return;
            if (CurrentDialogue != null) CurrentDialogue.EndDialogue();
        }

    }
