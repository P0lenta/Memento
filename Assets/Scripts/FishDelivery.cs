using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FishDelivery : MonoBehaviour
{

    [Header("Requisitos")]
    public EmotionType RequiredEmotion;
    public EmotionType[] PossibleEmotion;

    private static int CurrentEmotionIndex = 0;

    void Start()
    {   

        CurrentEmotionIndex = EmotionManager.Instance.CurrentMission;

        if (PossibleEmotion == null || PossibleEmotion.Length == 0)
        {
            PossibleEmotion = new EmotionType[]
            {
                EmotionType.Felicidade,
                EmotionType.Tristeza,
                EmotionType.Raiva,
                EmotionType.Medo,
                EmotionType.Surpresa
            };
        }

        if (EmotionManager.Instance.CurrentMissionFish == EmotionType.None)
        {
            StartNextMission();
        }
        else
        {
            RequiredEmotion = EmotionManager.Instance.CurrentMissionFish;
        }
    }

    public void CompleteDelivery()
    {   

        PlayerInteraction Player = FindFirstObjectByType<PlayerInteraction>();
        if (Player == null) return;

        Player.SetHeldFish(EmotionType.None);
        EmotionManager.Instance.SetEmotion(EmotionType.None);
        EmotionManager.Instance.CurrentMissionFish = EmotionType.None;

        EmotionManager.Instance.CurrentMission++;
        CurrentEmotionIndex = EmotionManager.Instance.CurrentMission;
        EmotionManager.Instance.IsMapUnlocked = false;
        Debug.Log("Map is locked");

        if (CurrentEmotionIndex < PossibleEmotion.Length) StartNextMission();
    }

        public void StartNextMission()
    {
        if (PossibleEmotion.Length == 0) return;
        RequiredEmotion = PossibleEmotion[CurrentEmotionIndex];
        EmotionManager.Instance.CurrentMissionFish = RequiredEmotion;
        Debug.Log("Map is unlocked");
    }

    
}
