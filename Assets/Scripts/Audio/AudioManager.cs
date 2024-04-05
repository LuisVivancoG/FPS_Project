using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.spatialBlend = s.SpatialBlend;
            s.Source.priority = s.Priority;
            s.Source.playOnAwake = s.PlayOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Missing sound '" + name + "' in the AudioManager array");
            return;
        }
        s.Source.Play();
    }
}
