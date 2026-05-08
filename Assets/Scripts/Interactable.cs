using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [Header("UI")]
    public TextMeshProUGUI InteractionText;
    public string message = "E";

    [Header("Efeitos sonoros")]
    public AudioSource InteractionSound;

    void Start()
    {
        if (InteractionText != null) InteractionText.gameObject.SetActive(false);  
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!gameObject.activeInHierarchy) return;
        Debug.Log($"OnTriggerEnter chamado em {gameObject.name}");

        if (!other.CompareTag("Player")) return;

         PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                PlayerInteraction.CanInteract = true;
                playerInteraction.ActualInteractiveObject = gameObject; 
            }

            if (!PlayerInteraction.IsInputLocked)
            {
                if (InteractionText != null)
                {
                InteractionText.text = message;
                InteractionText.gameObject.SetActive(true);
                }

                PlayerInteraction.Instance?.SetInteractionImageVisible(true);
            }
            
        }

        private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;
        if (!gameObject.activeInHierarchy) return;
        if (!other.CompareTag("Player")) return;

        PlayerInteraction Player = other.GetComponent<PlayerInteraction>();
        if (Player == null) return;

        if (Player.ActualInteractiveObject == null)
        {
            Player.ActualInteractiveObject = gameObject;
            return;
        }

        float DistanciaAtual = Vector3.Distance(other.transform.position, Player.ActualInteractiveObject.transform.position);
        float DistanciaEsse = Vector3.Distance(other.transform.position, transform.position);

        if (DistanciaEsse < DistanciaAtual)
        {
            Player.ActualInteractiveObject = gameObject;
            if (InteractionText != null) InteractionText.text = message;
        }

        if (!PlayerInteraction.IsInputLocked) PlayerInteraction.Instance?.SetInteractionImageVisible(true);
    }

    private void OnTriggerExit (Collider other)
    {
        if (!other.CompareTag("Player")) return;
             PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                PlayerInteraction.CanInteract = false;
                playerInteraction.ActualInteractiveObject = null;
            }

            if (InteractionText != null) InteractionText.gameObject.SetActive(false);

            PlayerInteraction.Instance?.SetInteractionImageVisible(false);
        
    }

    private void OnDisable()
    {

        if (InteractionText != null) InteractionText.gameObject.SetActive(false);

    }

    public void PlayerInteractionSound()
    {
        if (InteractionSound != null) InteractionSound.Play();
    }

    public void StopInteractionSound()
    {
        if (InteractionSound != null) InteractionSound.Stop();
    }


}