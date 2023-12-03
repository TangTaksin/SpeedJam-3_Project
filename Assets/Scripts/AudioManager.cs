using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer and Sliders")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioSource> sfxSources;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    
    public static AudioManager Instance;

    private void Awake()
    {
        SingletonCheck();
        DontDestroyOnLoad(gameObject);
        InitializeSFXSources(3); // Adjust the number based on your needs
        SetSliderListeners();
    }

    private void Start()
    {
        PlayDefaultMusic();
        LoadVolume();
    }

    #region Audio Control

    public void PlayMusic(int index)
    {
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void PlaySFX(int index)
    {
        if (IsValidSFXIndex(index))
        {
            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.clip = sfxClips[index];
                availableSource.Play();
            }
        }
        else
        {
            Debug.LogError("Invalid SFX index");
        }
    }

    public void StopMusic() => musicSource.Stop();

    public void StopSFX()
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.Stop();
        }
    }

    private void PlayDefaultMusic()
    {
        musicSource.clip = musicClips[0];
        musicSource.Play();
    }

    private void InitializeSFXSources(int count)
    {
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < count; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var sfxSource in sfxSources)
        {
            if (!sfxSource.isPlaying)
            {
                return sfxSource;
            }
        }
        return null;
    }

    #endregion

    #region Volume Control

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        musicSource.volume = volume;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = volume;
        }

        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        SetMusicVolume(musicSlider.value);
        SetSfxVolume(sfxSlider.value);
    }

    #endregion

    #region Helper Methods

    private void SingletonCheck()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetSliderListeners()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    private bool IsValidSFXIndex(int index) => index >= 0 && index < sfxClips.Length;

    #endregion
}
