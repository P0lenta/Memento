using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public RadarWarning AvisoRadio;
    public GameObject ComputerObj;
    private bool TutorialCompleto;

    [Header("Referências GameObject")]
    public GameObject TextoObjAndar;
    public GameObject TextoObjInteragir;
    public GameObject TextoObjMapa;
    public GameObject TextoObjAgua;

    [Header ("Textos dos tutoriais")]
    public TextMeshProUGUI TutorialTextAndar;
    public TextMeshProUGUI TutorialTextInteragir;
    public TextMeshProUGUI TutorialTextMapa;
    public TextMeshProUGUI TutorialTextAgua;

    [Header ("Mensagens dos tutoriais")]
    public string MensagemAndar = "Press [WASD] to walk";
    public string MensagemInteragir = "Press [E] or [Mouse Click] to interact";
    public string MensagemMapa = "Press [M] to open the map";
    public string MensagemAgua = "Go to the water";

    [Header ("Diálogo inicial")]
    public DialogueManager DialogoInicial;

    void Start()
    {
        TutorialCompleto = PlayerPrefs.GetInt("TutorialCompleto", 0) == 1;

        TutorialTextAndar.text = MensagemAndar;
        TutorialTextInteragir.text = MensagemInteragir;
        TutorialTextMapa.text = MensagemMapa;
        TutorialTextAgua.text = MensagemAgua;

        if (!TutorialCompleto)
        {
            if (TextoObjAndar) TextoObjAndar.SetActive(true);
            if (TextoObjInteragir) TextoObjInteragir.SetActive(true);
            if (TextoObjMapa) TextoObjMapa.SetActive(false);
            if (TextoObjAgua) TextoObjAgua.SetActive(false);
            if (DialogoInicial != null) StartCoroutine(IniciarDialogo());
        }
        else
        {
            if (TextoObjAndar) TextoObjAndar.SetActive(false);
            if (TextoObjInteragir) TextoObjInteragir.SetActive(false);
            if (TextoObjMapa) TextoObjMapa.SetActive(false);
            if (TextoObjAgua) TextoObjAgua.SetActive(false);
        }
    }

    private IEnumerator IniciarDialogo()
    {
        yield return null;
        DialogoInicial.StartDialogue();
        CameraFocus focus = DialogoInicial.GetComponent<CameraFocus>();
        if (focus != null) focus.StartFocus(PlayerInteraction.Instance);
    }

    public void OnPlayerMoved()
    {
        if (TutorialCompleto) return;
        if (TextoObjAndar) TextoObjAndar.SetActive(false);
    }

    private bool JaInteragiu = false;

    public void OnPlayerInteracted()
    {
        if (TutorialCompleto) return;
        if (JaInteragiu) return;

        JaInteragiu = true;

        if (TextoObjInteragir) TextoObjInteragir.SetActive(false);
        if (TextoObjMapa) TextoObjMapa.SetActive(true);

        if (AvisoRadio != null) AvisoRadio.AtivarRadio();
        if (ComputerObj != null) ComputerObj.SetActive(false);
    }

    public void OnPlayerOpenedMap()
    {
        if (TutorialCompleto) return;
        if (TextoObjMapa) TextoObjMapa.SetActive(false);
        if (TextoObjAgua) TextoObjAgua.SetActive(true);
    }

    public void OnPlayerEnteredWater()
    {
        if (TutorialCompleto) return;
        if (TextoObjAgua) TextoObjAgua.SetActive(false);
        TutorialCompleto = true;
        PlayerPrefs.SetInt("TutorialCompleto", 1);
        PlayerPrefs.Save();
    }
}
