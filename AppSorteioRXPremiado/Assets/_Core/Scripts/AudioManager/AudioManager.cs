using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Space]
    [Range(0f, 1f)] public float _volumeSFX = 1f;
    [Range(0f, 1f)] public float _volumeMusic = 1f;
    [Space]
    public SoundSFX[] soundsSFX;
    [Space]
    public SoundMusic[] soundsMusic;

    void Update()
    {
        VolumeSFX(_volumeSFX);
        //VolumeMusic(_volumeMusic);
    }

    void Awake()
    {
        foreach (SoundSFX sfx in soundsSFX)
        {
            sfx.audioSource = gameObject.AddComponent<AudioSource>();
            sfx.audioSource.clip = sfx.clip;
            sfx.audioSource.volume = sfx.volume;
            sfx.audioSource.loop = sfx.loop;
        }

        foreach (SoundMusic music in soundsMusic)
        {
            music.audioSource = gameObject.AddComponent<AudioSource>();
            music.audioSource.clip = music.clip;
            music.audioSource.volume = music.volume;
            music.audioSource.loop = music.loop;
        }
    }

    public void PlaySFX(string name)
    {
        SoundSFX sfx = Array.Find(soundsSFX, sound => sound.name == name);

        if (sfx == null)
        {
            return;
        }
        sfx.audioSource.Play();
    }

    public void StopSFX(string name)
    {
        SoundSFX sfx = Array.Find(soundsSFX, sound => sound.name == name);
        if (sfx == null)
        {
            return;
        }
        sfx.audioSource.Stop();
    }

    public void PlayMusic(string name)
    {
        SoundMusic music = Array.Find(soundsMusic, sound => sound.name == name);
        if (music == null)
        {
            return;
        }
        music.audioSource.Play();
    }

    public void StopMusic(string name)
    {
        SoundMusic music = Array.Find(soundsMusic, sound => sound.name == name);
        if (music == null)
        {
            return;
        }
        music.audioSource.Stop();
    }

    public void VolumeSFX(float volume)
    {
        for (int i = 0; i < soundsSFX.Length; i++)
        {
            soundsSFX[i].audioSource.volume = volume;
        }
    }

    public void VolumeMusic(float volume)
    {
        for (int i = 0; i < soundsMusic.Length; i++)
        {
            soundsMusic[i].audioSource.volume = volume;
        }
    }
}