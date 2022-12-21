using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundSFX
{
	public string name;
	public AudioClip clip;
	[Range(0f, 1f)] public float volume;
	public bool loop;
	[HideInInspector] public AudioSource audioSource;
}
