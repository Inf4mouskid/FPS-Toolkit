using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public struct Sound
{
    public AudioClip clip;
    public string Name;
    [Range(0, 1)] public float Volume;
    [Range(0, 1)] public float Pitch;
    [HideInInspector] public AudioSource Source;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    // Use this for initialization
    void Awake()
    {
        foreach (var s in Sounds)
        {
            gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
        }
    }

    public void Play(string Name)
    {
        Array.Find(Sounds, Sound => Sound.Name == Name);
    }
}
