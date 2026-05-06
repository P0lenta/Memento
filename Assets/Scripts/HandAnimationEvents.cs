using UnityEngine;

public class HandAnimationEvents : MonoBehaviour
{
   public PlayerInteraction playerInteraction;

   public void OnGrabAnimation()
    {
        Debug.Log("OnGrabAnimation chamado!");
        playerInteraction.OnHoldingAnimationEvent();
    }
}
