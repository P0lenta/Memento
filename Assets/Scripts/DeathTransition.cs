using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DeathTransition : MonoBehaviour
{
    [Header("Controlador de animaçãp")]
    public Animator DeathAnimation;

    [Header("Textos de morte")]
    public string DeathHeaderText = "Você Morreu";
    public string[] DeathMessageText;

    [Header("Referências de texto")]
    public TextMeshProUGUI DeathHeader;
    public TextMeshProUGUI DeathMessage;

    private void Start() 
    {
        DeathHeader.text = "";
        DeathMessage.text = "";    
    }

    public void StartDeath()
    {
        StartCoroutine(PlayerDied());
    }

    private IEnumerator PlayerDied()
    {
        DeathAnimation.SetTrigger("Died");
        
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeWriter(DeathHeader, DeathHeaderText, 0.2f));

        yield return StartCoroutine(TypeWriter(
            DeathMessage, DeathMessageText[Random.Range (0, DeathMessageText.Length)], 0.04f));
        
        yield return new WaitForSeconds(1f);

        if (FadeTransition.Instance != null) FadeTransition.Instance.StartFade("Submarine");

    } 

    private IEnumerator TypeWriter(TextMeshProUGUI TextObject, string Message, float Speed)
    {
        TextObject.text = "";
        foreach (char T in Message)
        {
            TextObject.text += T;
            yield return new WaitForSeconds(Speed);
        }
    }
}
