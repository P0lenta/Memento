using UnityEngine;

public class RadarWarning : MonoBehaviour
{
    [SerializeField]
    private GameObject RadioObj;

    private bool RadioLiberado = false;

    public void AtivarRadio()
    {
        RadioLiberado = true;
    }

    void Start() 
    {
        if (RadioObj != null) RadioObj.SetActive(false);
    }

    void Update()
    {
        if (RadioObj == null) return;

        if (!RadioLiberado)
        {
            RadioObj.SetActive(false);
            return;
        }

        bool MapaTaBloqueado = !EmotionManager.Instance.IsMapUnlocked;
        bool NumDormeNao = !EmotionManager.Instance.IsTimeToBed;
        bool CapturouCerto = EmotionManager.Instance.HeldFish == EmotionManager.Instance.CurrentMissionFish;

        bool PrecisaPegarQuest = MapaTaBloqueado && NumDormeNao;

        bool PrecisaEntregarQuest = !MapaTaBloqueado && NumDormeNao && CapturouCerto;

        RadioObj.SetActive(PrecisaPegarQuest || PrecisaEntregarQuest);

    }

}
