using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OxygenManager : MonoBehaviour
{
    [Header("Configurações de Oxigênio")]
    public float MaxOxygen = 100f;
    public float CurrentOxygen;
    public float DecreaseRate = 1f; 
    public float Accelerate = 0f;

    [Header("UI")]
    public Image OxygenFill;
    public TextMeshProUGUI ResetText;
    public string DeathMessage = "Morto :(";
    public string ResetMessage = "Aperte R para voltar";
    public GameObject HandsUI;
    private bool IsDead = false;
    private string SceneToChange = "Submarine";
    public Renderer HandsRenderer;


    void Start() 
    {
        CurrentOxygen = MaxOxygen;
        if (ResetText != null) ResetText.text = "";
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Accelerate = 19f;
        }
        else
        {
            Accelerate = 0f;
        }
    }

    void FixedUpdate()
    {
        float TaxaTotal = DecreaseRate + Accelerate;

        if (IsDead) return;

        if (!PlayerInteraction.IsInputLocked)
        {
            CurrentOxygen -= TaxaTotal * Time.deltaTime;

            if (CurrentOxygen <= 0f)
            {
                CurrentOxygen = 0;
                Die();
            }   
        }

        if (OxygenFill != null) OxygenFill.fillAmount = CurrentOxygen / MaxOxygen;

        if (!IsDead && ResetText != null && ResetText.text != "") ResetText.text = "";
    }

    public void Die()
    {
        IsDead = true;

         if(HandsRenderer != null) HandsRenderer.enabled = false;

         if (ResetText != null) ResetText.text = DeathMessage + "\n" + ResetMessage;
        
        PlayerWaterMovement MoveScript = GetComponent<PlayerWaterMovement>();
        if (MoveScript != null)
            {
                MoveScript.IsDead = true;
                MoveScript.enabled = false;   
            }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        PlayerInteraction.Instance?.RefreshHandsUIVisibility();

    } 

   public void OnReset(InputAction.CallbackContext context)
    {
        
        if (context.started && IsDead)
        {
            if (EmotionManager.Instance != null)
            {
                EmotionManager.Instance.SetEmotion(EmotionType.None);
                EmotionManager.Instance.HeldFish = EmotionType.None;
            }

            RestartScene();
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneToChange);
    }   

}