using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OxygenManager : MonoBehaviour
{
    [Header("Configurações de Oxigênio")]
    public float MaxOxygen = 100f;
    public float CurrentOxygen;
    public float DecreaseRate = 1f; 
    public bool IsImortal = false;

    [Header("UI")]
    public Image OxygenFill;
    public GameObject HandsUI;
    private bool IsDead = false;
    public Renderer HandsRenderer;
    public GameObject Hacktext;
    
    [Header("Animação")]
    public DeathTransition deathTransition;


    void Start() 
    {
        CurrentOxygen = MaxOxygen;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsImortal = !IsImortal;
            if (IsImortal) Hacktext.SetActive(true);
            if (!IsImortal) Hacktext.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        float TaxaTotal = DecreaseRate;

        if (IsDead) return;
        if (IsImortal) return;

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
    }

    public void Die()
    {
        if (IsDead) return;
        if (IsImortal) return;

        IsDead = true;

        if (PlayerInteraction.Instance != null) PlayerInteraction.Instance.SetHeldFish(EmotionType.None);

         if(HandsRenderer != null) HandsRenderer.enabled = false;
        
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

        if (deathTransition != null) deathTransition.StartDeath();
    } 
}