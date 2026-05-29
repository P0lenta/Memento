using UnityEngine;

public class DialogueCameraControl : MonoBehaviour
{
    [Header("Referências")]
    public CameraFocus Camera;
    public DialogueManager Dialogue;

    private Vector3 OriginalCameraPos;
    private Vector3 OriginalCameraRot;

    public CameraFocusSet[] CameraSet;

    [System.Serializable]
    public class CameraFocusSet
    {
        public CameraPoint[] Point;
    }

    [System.Serializable]
    public class CameraPoint
    {
        public Vector3 CameraPosition;
        public Vector3 CameraRotation;
    }

    void Start() 
    {
        OriginalCameraPos = Camera.cameraPosition;
        OriginalCameraRot = Camera.cameraRotation;
    }

    void OnEnable() 
    {
        DialogueManager.OnLineChanged += PlayCameraPosition;
        DialogueManager.OnDialogueEnded += ResetCamera;
    }

    void OnDisable() 
    {
        DialogueManager.OnLineChanged -= PlayCameraPosition;
        DialogueManager.OnDialogueEnded -= ResetCamera; 
    }

    void PlayCameraPosition(DialogueManager.DialogueOption Option, int LineIndex)
    {
        if (Dialogue.GetCurrentDialogueOption() != Option) return;

        int OptionIndex = System.Array.IndexOf(Dialogue.Dialogues, Option);
        if (OptionIndex < 0 || OptionIndex >= CameraSet.Length) return;

        CameraFocusSet set = CameraSet[OptionIndex];

        if (LineIndex < set.Point.Length)
        {
            CameraPoint PointSet = set.Point[LineIndex];

            if (PointSet.CameraPosition == Vector3.zero && PointSet.CameraRotation == Vector3.zero) return;

            Camera.cameraPosition = PointSet.CameraPosition;
            Camera.cameraRotation = PointSet.CameraRotation;

            Camera.UpdateFocusPosition();
        }
    }

    void ResetCamera(DialogueManager.DialogueOption Option)
    {
        Camera.cameraPosition = OriginalCameraPos;
        Camera.cameraRotation = OriginalCameraRot;
    }
}
