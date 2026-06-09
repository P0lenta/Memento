using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    public float Speed = 1.5f;
    public AudioSource AudioPlayer;
    public AudioClip DetectSound;
    public AudioClip ChaseSound;
    private float LosePlayerDelay = 0.5f;
    private Transform Player;
    private bool IsChasing = false;
    private Transform Root;
    private Coroutine SoundCoroutine;
    private float FadeOutDuration = 1.5f;
    private Vector3 OriginalPosition;
    private Quaternion OriginalRotation;
    private Coroutine LosePlayerCoroutine;
    private bool IsReturning = false;


    private void Start() 
    {
        Root = transform.root;

        bool DeveAparecer = EmotionManager.Instance != null && EmotionManager.Instance.CurrentMission >= 9;
        gameObject.SetActive(DeveAparecer);    

        OriginalPosition = Root.position;
        OriginalRotation = Root.rotation;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag("Player")) return;

        if (LosePlayerCoroutine != null)
        {
            StopCoroutine(LosePlayerCoroutine);
            LosePlayerCoroutine = null;
        }

        Player = other.transform;    

        SetChasing(true);
    }

    private void OnTriggerExit(Collider other) 
    {
        if (!other.CompareTag("Player")) return;

        LosePlayerCoroutine = StartCoroutine(LosePlayerDelayCo()); 
    }

    private void SetChasing(bool value)
    {
        IsChasing = value;

        if (IsChasing)
        {
            if (SoundCoroutine != null) StopCoroutine(SoundCoroutine);
            SoundCoroutine = StartCoroutine(PlaySounds());
        }
        else
        {
            if (SoundCoroutine != null)
            {
                StopCoroutine(SoundCoroutine);
                SoundCoroutine = null;
            }
            SoundCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator LosePlayerDelayCo()
    {
        yield return new WaitForSeconds(LosePlayerDelay);
        SetChasing (false);
        LosePlayerCoroutine = null;
    }

    private IEnumerator FadeOut()
    {
        float StartVolume = AudioPlayer.volume;
        float TimePassed = 0f;

        while (TimePassed < FadeOutDuration)
        {
            TimePassed += Time.deltaTime;
            AudioPlayer.volume = Mathf.Lerp(StartVolume, 0f, TimePassed / FadeOutDuration);
            yield return null;
        }

        AudioPlayer.Stop();
        AudioPlayer.volume = StartVolume;

        IsReturning = true;

        SoundCoroutine = null;
    }

    private IEnumerator PlaySounds()
    {
        if (DetectSound != null) AudioPlayer.PlayOneShot(DetectSound);

        yield return new WaitForSeconds(8f);

        if (ChaseSound != null)
        {
            AudioPlayer.clip = ChaseSound;
            AudioPlayer.loop = true;
            AudioPlayer.volume = 0.5f;
            AudioPlayer.Play();
        }
    }

    private void Update() 
    {
        if (PlayerInteraction.IsInputLocked) return;

        if (IsChasing && Player != null)
        {
            Vector3 Direction = (Player.position - Root.position).normalized;
            Root.position += Direction * Speed * Time.deltaTime;
            Root.LookAt(Player);
        }   

        else if (IsReturning)
        {
            Root.position = Vector3.MoveTowards(Root.position, OriginalPosition, Speed * Time.deltaTime);
            Root.rotation = Quaternion.Lerp(Root.rotation, OriginalRotation, Speed * Time.deltaTime);

            if (Vector3.Distance(Root.position, OriginalPosition) < 0.01f)
            {
                Root.position = OriginalPosition;
                Root.rotation = OriginalRotation;
                IsReturning = false;
            }
        }
    }
}
