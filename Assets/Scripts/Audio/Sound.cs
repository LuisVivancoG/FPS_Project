using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip Clip;
    [Range (0f, 1f)]
    public float Volume;
    [Range (0.1f, 3f)]
    public float Pitch;
    [Range(0, 1f)]
    public float SpatialBlend;
    [Range(0, 256)]
    public int Priority;
    public AudioSource Source {  get; set; }
    public bool Loop;
    public bool PlayOnAwake;
}
