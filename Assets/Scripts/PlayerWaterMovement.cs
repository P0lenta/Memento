using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaterMovement : MonoBehaviour
{

[Header("Modelo 3D")]
    public GameObject playerModel;

[Header("Valores de velocidade")]
    public float MoveSpeed = 10f;
    public float VerticalBoostSpeed = 5f;
    public float WaterDrag = 3;
    public float MouseSensibility = 1f;

[Header("Valores de câmera")]
    public float minRotX = -30;            
    public float maxRotX = 60;      
    public Transform CamTransform;    

[Header("Referencias")]
    public Rigidbody rig;               
    public Collider col;
    public Animator HandsAnimation;

[Header("Flags")]
    public bool IsDead = false;             
    public bool IsInteracting = false;

    private Vector3 MoveInput;
    private bool IsJumping;
    private bool IsCrouching;

    void Start()
    {
        GetSensibility();

        rig.useGravity = false;
        rig.linearDamping = WaterDrag;
    }

    public void GetSensibility()
    {
        MouseSensibility = PlayerPrefs.GetFloat("Sensibilidade", 1f);
    }

    public void Update()
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;
        if (HandsAnimation != null) HandsAnimation.SetBool("IsSwiming", MoveInput.sqrMagnitude > 0.1f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        MoveInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        IsJumping = context.performed;
        
        if (context.canceled) IsJumping = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        IsCrouching = context.performed;

        if (context.canceled) IsCrouching = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {       
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        Vector2 lookInput = context.ReadValue<Vector2>() * MouseSensibility;  

        float rotY = transform.eulerAngles.y + lookInput.x * 0.3f;
        rotY = ClampAngle(rotY, -360, 360);
        transform.eulerAngles = new Vector3(0, rotY, 0);

        float rotX = CamTransform.localEulerAngles.x - lookInput.y * 0.3f;
        rotX = ClampAngle(rotX, minRotX, maxRotX);
        CamTransform.localEulerAngles = new Vector3(rotX, 0, 0);
    }
    public float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }


    void FixedUpdate()
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        Vector3 CamFoward = CamTransform.forward;
        Vector3 CamRight = CamTransform.right;

        Vector3 MoveDirection = (CamFoward * MoveInput.y + CamRight * MoveInput.x).normalized;

        Vector3 TargetVelocity = MoveDirection * MoveSpeed;

        if (IsJumping) TargetVelocity.y += VerticalBoostSpeed;
        if (IsCrouching) TargetVelocity.y -= VerticalBoostSpeed;

        rig.linearVelocity = TargetVelocity;
    }

    public void StopWaterMovement()
    {
        rig.linearVelocity = Vector3.zero;
        MoveInput = Vector2.zero;
        IsJumping = false;
        IsCrouching = false;
    }

}
