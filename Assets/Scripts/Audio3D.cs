using System.Collections.Generic;
using UnityEngine;

public class Audio3D : MonoBehaviour
{
    protected List<Sound> footsteps;
    protected List<Sound> gore;
    protected List<Sound> soundEffects;
    protected AudioManager audioManager;
    private float walkTime = 0;
    private int stepIndex = 0;
    private int goreIndex = 0;

    void Start()
    {
        audioManager = AudioManager.Instance;
        footsteps = InitializeSounds("wetStep");
        gore = InitializeSounds("gore");
        soundEffects = InitializeSounds("soundEffects");
    }

    private List<Sound> InitializeSounds(string arrName)
    {
        List<Sound> soundList = new List<Sound>();

        foreach (Sound step in audioManager.GetSounds(arrName))
        {
            Sound s = new Sound();
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.spatialBlend = 1f;
            s.source.minDistance = 4f;
            s.source.volume = 0.85f;
            s.source.clip = step.clip;
            s.name = step.name;
            soundList.Add(s);
        }
        return soundList;
    }

    public void Walk(float speed)
    {
        if (Time.time > walkTime)
        {
            walkTime = Time.time + 45 / (speed * 60);
            footsteps[stepIndex].source.Play();

            stepIndex++;

            if (stepIndex >= footsteps.Count) stepIndex = 0;
        }
    }
    
    public void Gore()
    {
        gore[goreIndex].source.Play();

        goreIndex++;

        if (goreIndex >= gore.Count) goreIndex = 0;
    }

    public void Explode()
    {
        foreach (Sound s in soundEffects)
        {
            if (s.name == "explode" && !s.source.isPlaying)
            {
               s.source.Play();
            }
        }
    }
}
