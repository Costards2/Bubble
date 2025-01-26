using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX;

    public float fadeDuration = 1f;

    public List<AudioClip> audioClips = new List<AudioClip>();

    private bool fadingInMusic, fadingOutMusic;

    void Awake()
    {

        audioSourceMusic = transform.GetChild(0).GetComponent<AudioSource>();
        audioSourceSFX = transform.GetChild(1).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        PlayMusic("Candy Rush");
    }

    public void PlayMusic(string nome)
    {
        AudioClip clipEncontrado = audioClips.Find(clip => clip.name == nome);
        if (clipEncontrado != null)
        {
            audioSourceMusic.clip = clipEncontrado;
            StartCoroutine(FadeIn(audioSourceMusic));
        }
        else
        {
            Debug.LogWarning("Clip não encontrado!");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        audioSourceSFX.clip = clip;
        audioSourceSFX.Play();
    }

    public void FadeInMusic()
    {
        StartCoroutine(FadeIn(audioSourceMusic));
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOut(audioSourceMusic));
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }

    public void ChangeMusic(string nome)
    {
        StartCoroutine(FadeOutAndIn(audioSourceMusic, nome));
    }

    #region Methods to Control the Audio in the Menu

    public void ToggleMusic()
    {
        audioSourceMusic.mute = !audioSourceMusic.mute;
    }

    public void ToggleSFX()
    {
        audioSourceSFX.mute = !audioSourceSFX.mute;
    }

    public void MusicVolume(float volume)
    {
        audioSourceMusic.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        audioSourceSFX.volume = volume;
    }

    #endregion

    #region IEnums

    //try to find a way to only use the Fade out and Fade in Method
    private IEnumerator FadeOutAndIn(AudioSource source, string nome)
    {
        fadingOutMusic = true;

        float startVolume = source.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        source.volume = 0;

        source.Stop();

        fadingOutMusic = false;

        AudioClip clipEncontrado = audioClips.Find(clip => clip.name == nome);
        if (clipEncontrado != null)
        {
            audioSourceMusic.clip = clipEncontrado;
        }

        fadingInMusic = true;

        source.volume = 0.1f;
        source.Play();

        startVolume = 0;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 1f, t / fadeDuration);
            yield return null;
        }

        source.volume = 1f;

        fadingInMusic = false;
    }

    private IEnumerator FadeIn(AudioSource source)
    {
        fadingInMusic = true;

        source.Play();

        source.volume = 0.1f;

        float startVolume = 0;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 1f, t / fadeDuration);
            yield return null;
        }

        source.volume = 1f;

        fadingInMusic = false;
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        fadingOutMusic = true;

        float startVolume = source.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        source.volume = 0;

        //source.Stop();

        fadingOutMusic = false;
    }
    #endregion
}
