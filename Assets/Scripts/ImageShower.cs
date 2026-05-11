using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ImageShower : MonoBehaviour
{
    [Header("Sprites de diálogo")]
    public Sprite[] DialogueSprites;
    public Image DialogueImage;
    public GameObject ImageObject;

    [Header("Referência Dialogue Manager")]
    public DialogueManager Dialogue;

    void Start()
    {
        ImageObject.SetActive(false);
    }

    void Update()
    {
        if (Dialogue == null) return;

        if (Dialogue.IsActive) 
        {
            StartImage();
        }
        else
        {
            ImageObject.SetActive(false);
        }
        
    }

    public void StartImage()
    {
        int ImageIndex = Dialogue.CurrentLine;

        ImageObject.SetActive(true);

        DialogueImage.sprite = DialogueSprites[ImageIndex];
    }



}
