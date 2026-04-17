using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

[Header("Modelo 3D")]
    public GameObject playerModel;

[Header("Valores de velocidade")]
    public float speed = 10;                    
    public float turnSpeedHorizontal = 0.3f; 
    public float turnSpeeVertical = 0.1f; 
    public float MouseSensibility = 1f;

[Header("Valores de câmera")]
    public float minRotX = -30;            
    public float maxRotX = 60;      
    public Transform camTransform;    

[Header("Colisores")]
    public Rigidbody rig;               
    public Collider col;                 
    public LayerMask floorLayers; 

[Header("Flags")]
    public bool IsDead = false;             
    public bool IsInteracting = false;
    public Animator HandsAnimation;

    private Vector3 MoveInput;

    void Start()
    {
        GetSensibility();
    }

    public void GetSensibility()
    {
        MouseSensibility = PlayerPrefs.GetFloat("Sensibilidade", 1f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsDead || PlayerInteraction.IsInputLocked) return;

            Vector2 input = context.ReadValue<Vector2>();
            MoveInput = input.normalized * speed;

            if (MoveInput.sqrMagnitude > 0.1f)
            {
                HandsAnimation.SetBool("IsMoving", true);
            }
            else
            {
                HandsAnimation.SetBool("IsMoving", false);
            }
    }
    

    void FixedUpdate()
    {
        Vector3 vX = MoveInput.x * transform.right;        
        Vector3 vY = rig.linearVelocity.y * transform.up;   
        Vector3 vZ = MoveInput.y * transform.forward;       

        rig.linearVelocity = vX + vY + vZ;
    }

    public void OnLook(InputAction.CallbackContext context)
    {       
        if (IsDead || PlayerInteraction.IsInputLocked) return;

        Vector2 lookInput = context.ReadValue<Vector2>();  
        lookInput *= MouseSensibility;

        float rotY = transform.eulerAngles.y + lookInput.x * turnSpeedHorizontal;
        rotY = ClampAngle(rotY, -360, 360);
        transform.eulerAngles = new Vector3(0, rotY, 0);

        float rotX = camTransform.localEulerAngles.x - lookInput.y * turnSpeeVertical;
        rotX = ClampAngle(rotX, minRotX, maxRotX);
        camTransform.localEulerAngles = new Vector3(rotX, 0, 0);
    }
    public float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }

    public void StopMovement()
    {
        MoveInput = Vector3.zero;
    }

    /*public void OnJump(InputAction.CallbackContext context)
    {
        if (IsDead || IsInteracting) return;

        if (context.started)  
        {
            
            if (Physics.Raycast(col.bounds.center, Vector3.down, col.bounds.extents.y * 1.1f, floorLayers))
            {
                rig.linearVelocity = new Vector3(
                    rig.linearVelocity.x,
                    jumpForce,
                    rig.linearVelocity.z);
            }
        }
    }*/

}