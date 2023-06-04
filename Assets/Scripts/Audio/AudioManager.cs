using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sfx;
    public Sound[] music;
    public Sound[] swordSoundsSFX;

    System.Random rand = new System.Random();

    public static AudioManager instance;
    private void Awake()
    {
        if (AudioManager.instance == null)
            AudioManager.instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sfx)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        foreach (Sound sound in music)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        foreach (Sound sound in swordSoundsSFX)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Sound s = Array.Find(music, sound => sound.name == "IntroMusic");
        if (s == null)
        {
            Debug.LogWarning("Sound with name: " + name + " doesn't exist");
            return;
        }
        s.source.Play();
    }
    public void Play (string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name: " + name + " doesn't exist");
            return;
        }
        s.source.Play();
    }

    public void PlaySwordSound()
    {
        int numOfSounds = swordSoundsSFX.Length;
        swordSoundsSFX[rand.Next(numOfSounds)].source.Play();

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name: " + name + " doesn't exist");
            return;
        }
        s.source.Stop();
    }

    public bool IsSoundPlaying(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name: " + name + " doesn't exist");
            return false;
        }
        return s.source.isPlaying;
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Sound sound in music)
        {
            sound.source.volume = sound.volume * volume;
        }
    }

    public void SetSFX(float volume)
    {
        foreach (Sound sound in sfx)
        {
            sound.source.volume = sound.volume * volume;
        }

        foreach (Sound sound in swordSoundsSFX)
        {
            sound.source.volume = sound.volume * volume;
        }
    }
}
