using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> bgmClips;
    public List<AudioClip> sfxClips;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    // Cooldown management
    private Dictionary<string, float> sfxCooldowns = new Dictionary<string, float>();
    public float defaultSfxCooldown = 0.5f; // Default cooldown for all SFX in seconds

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitAudioDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitAudioDictionaries()
    {
        foreach (var clip in bgmClips)
        {
            if (!bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }

        foreach (var clip in sfxClips)
        {
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    // Play background music
    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmDict.ContainsKey(clipName))
        {
            bgmSource.clip = bgmDict[clipName];
            bgmSource.loop = loop;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{clipName}' not found!");
        }
    }

    // Play a one-shot sound effect with cooldown check, and ensure it completes before allowing another play
    public void PlaySFX(string clipName)
    {
        if (sfxDict.ContainsKey(clipName))
        {
            // Check if the sound is already playing
            if (sfxSource.isPlaying)
            {
                return; // Don't play the sound again until the current one finishes
            }

            // Check for cooldown
            if (sfxCooldowns.ContainsKey(clipName) && Time.time - sfxCooldowns[clipName] < defaultSfxCooldown)
            {
                return; // Cooldown is still active, don't play the sound
            }

            // Play the sound effect
            sfxSource.PlayOneShot(sfxDict[clipName], sfxVolume);

            // Update the cooldown timestamp
            sfxCooldowns[clipName] = Time.time;
        }
        else
        {
            Debug.LogWarning($"SFX '{clipName}' not found!");
        }
    }

    // Toggle mute
    public void ToggleBGM(bool isOn)
    {
        bgmSource.mute = !isOn;
    }

    public void ToggleSFX(bool isOn)
    {
        sfxSource.mute = !isOn;
    }

    // Adjust volume
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
