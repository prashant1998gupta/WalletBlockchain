using UnityEngine;
using System.Collections.Generic;

public class AudioManager : GenericSingleton<AudioManager>
{
    // Singleton instance implementation

    public AudioSource soundEffectSource; // AudioSource for playing sound effects
    public AudioSource musicSource; // AudioSource for playing music

    public AudioClip[] soundEffectClips; // Array of sound effect clips
    public AudioClip[] musicClips; // Array of music clips

    private Dictionary<string, AudioClip> soundEffectClipDictionary; // Dictionary for storing sound effect clips by name
    private Dictionary<string, AudioClip> musicClipDictionary; // Dictionary for storing music clips by name

    private float soundEffectVolume = 1f; // Volume for sound effects
    private float musicVolume = 1f; // Volume for music

 
    private void Start()
    {
        // Create dictionaries and populate them with audio clips by name
        soundEffectClipDictionary = new Dictionary<string, AudioClip>();
        musicClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip soundEffectClip in soundEffectClips)
        {
            soundEffectClipDictionary.Add(soundEffectClip.name, soundEffectClip);
        }

        foreach (AudioClip musicClip in musicClips)
        {
            musicClipDictionary.Add(musicClip.name, musicClip);
        }
    }

    // Sound effect methods

    public void PlaySoundEffect(string clipName)
    {
        if (soundEffectClipDictionary.ContainsKey(clipName))
        {
            soundEffectSource.PlayOneShot(soundEffectClipDictionary[clipName], soundEffectVolume);
        }
        else
        {
            Debug.Log("Sound effect clip not found: " + clipName);
        }
    }

    public void StopSoundEffect()
    {
        soundEffectSource.Stop();
    }

    // Music methods

    public void PlayMusic(string clipName)
    {
        if (musicClipDictionary.ContainsKey(clipName))
        {
            musicSource.clip = musicClipDictionary[clipName];
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        else
        {
            Debug.Log("Music clip not found: " + clipName);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Volume control methods

    public void SetSoundEffectVolume(float volume)
    {
        soundEffectVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
}
