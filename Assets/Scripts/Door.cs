using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider DoorRealCollider;
    public Collider DoorInteractionCollider;
    public Animator DoorAnimator;

    public void OpenDoor()
    {
        DoorAnimator.SetTrigger("OpenDoor");
    }

    public void MiddleDoor()
    {
        DoorRealCollider.enabled = false;
        DoorInteractionCollider.enabled = false;
    }

    public void CloseDoor()
    {
        DoorRealCollider.enabled = true;
        DoorInteractionCollider.enabled = true;
    }
}
