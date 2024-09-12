using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicAudioSrc;
    public AudioSource vfxAudioSrc;
    public AudioClip musicClip;
    public AudioClip eatClip;
    public AudioClip jumpClip;
    public AudioClip runClip;
    public AudioClip swordClip;
    public AudioClip castClip;

    private void Awake()
    {
        if(Instance != null && !Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.musicAudioSrc.clip = musicClip;
        this.musicAudioSrc.Play();
    }

    public void PlaySFX(AudioClip vfxClip)
    {
        // this.vfxAudioSrc.clip = vfxClip;
        this.vfxAudioSrc.PlayOneShot(vfxClip);
    }
}
