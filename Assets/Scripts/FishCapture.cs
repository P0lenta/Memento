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
            Debug.Log($"[FishCapture] AudioSource encontrado em {gameObject.name}");
            Debug.Log($"  - AudioSource.clip = {(CaptureSound.clip != null ? CaptureSound.clip.name : "NULL")}");
            Debug.Log($"  - AudioSource.volume = {CaptureSound.volume}");
            Debug.Log($"  - AudioSource.mute = {CaptureSound.mute}");
            Debug.Log($"  - AudioSource.enabled = {CaptureSound.enabled}");
            Debug.Log($"  - AudioSource.gameObject.activeSelf = {CaptureSound.gameObject.activeSelf}");
            Debug.Log($"  - AudioSource.gameObject.name = {CaptureSound.gameObject.name}");


            CaptureSound.transform.SetParent(null);

            Debug.Log($"[FishCapture] AudioSource desanexado, novo pai: {CaptureSound.transform.parent}");
        
            CaptureSound.Play();
            Debug.Log($"[FishCapture] AudioSource.Play() chamado. isPlaying? {CaptureSound.isPlaying}");


            CaptureSound.Play();

            Debug.Log($"[FishCapture] Som deve tocar por {CaptureSound.clip.length} segundos");

            Destroy(CaptureSound.gameObject, CaptureSound.clip.length);

            Debug.Log($"[FishCapture] Desativando peixe: {gameObject.name}");
        }

        gameObject.SetActive(false);
    }




}
