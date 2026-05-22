using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    private void LateUpdate()
    {
        Vector3 CameraPosition = MainCamera.transform.position;

        CameraPosition.y = transform.position.y;

        transform.LookAt(CameraPosition);
        transform.Rotate(0f, 180f, 0f);
    }
}
