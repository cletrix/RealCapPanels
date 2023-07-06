using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBallsController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private List<AudioClip> lightSounds;
    [SerializeField] private List<AudioClip> mediumSounds;
    [SerializeField] private List<AudioClip> intenseSounds;

    public void PlaySoundBall(int _number)
    {
        audioSource.clip = GetSoundSelectedBall(_number);
        audioSource.Play();
    }
    public AudioClip GetSoundSelectedBall(int _number)
    {
        AudioClip clip = null;
        //if (_number >= 41)
        //{
        //    clip = intenseSounds[_number - 1];
        //}
        //else if (_number >= 21 &&_number <= 40)
        //{
        //    clip = mediumSounds[_number - 1];
        //}
        //else if (_number >= 1 && _number <= 20)
        //{
        //    clip = lightSounds[_number - 1];
        //}
        clip = lightSounds[_number - 1];
        return clip;
    }

   
}
