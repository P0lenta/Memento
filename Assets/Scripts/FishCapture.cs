using UnityEngine;

public class FishCapture : MonoBehaviour
{
    public FishVisibility fishVisibility;
    public ParticleSystem CaptureParticle;
    public AudioSource CaptureSound;

    void Start()
    {
        fishVisibility = GetComponent<FishVisibility>();
    }

    public void Interact(PlayerInteraction player)
    {
        if (fishVisibility != null) player.SetHeldFish(fishVisibility.EmotionAttached);

        if (CaptureParticle != null)
        {
            CaptureParticle.transform.SetParent(null);
            CaptureParticle.Play();
            Destroy(CaptureParticle.gameObject, CaptureParticle.main.duration);
        }

        if (CaptureSound != null)
        {
            CaptureSound.transform.SetParent(null);
            CaptureSound.Play();
            Destroy(CaptureSound.gameObject, CaptureSound.clip.length);
        }

        gameObject.SetActive(false);
    }




}
