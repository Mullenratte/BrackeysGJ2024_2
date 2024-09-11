using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    [Range(.1f, 1)]
    public float volume;
    

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
