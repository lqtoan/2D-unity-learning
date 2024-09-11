using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSrc;
    public AudioSource vfxAudioSrc;
    public AudioClip musicClip;
    public AudioClip eatClip;
    public AudioClip jumpClip;
    public AudioClip runClip;

    public void Start() {
        musicAudioSrc.clip = musicClip;
        musicAudioSrc.Play();
    }

    public void PlaySFX(AudioClip vfxClip) {
        vfxAudioSrc.clip = vfxClip;
        vfxAudioSrc.PlayOneShot(vfxClip);
    }
}
