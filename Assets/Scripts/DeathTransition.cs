using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DeathTransition : MonoBehaviour
{
    [Header("Controlador de animaçãp")]
    public Animator DeathAnimation;

    [Header("Textos de morte")]
    public bool IsForReal;
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
        if (IsForReal) DeathAnimation.SetTrigger("DiedR");
        else DeathAnimation.SetTrigger("DiedM");
        
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeWriter(DeathHeader, DeathHeaderText, 0.2f));

        yield return StartCoroutine(TypeWriter(
            DeathMessage, DeathMessageText[Random.Range (0, DeathMessageText.Length)], 0.04f));
        
        yield return new WaitForSeconds(1f);

        PlayerWaterMovement MoveScript = GetComponent<PlayerWaterMovement>();
        if (MoveScript != null)
            {
                MoveScript.IsDead = false;
                MoveScript.enabled = true;   
            }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

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
