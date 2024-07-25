using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    protected Sound[] dialogs, forestStep, woodStep, stoneStep, wetStep, background, soundEffects, attack, gore, swim;

    private int stepIndex = 0;
    private float walkTime = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InitializeSounds(background);
        InitializeSounds(forestStep);
        InitializeSounds(wetStep);
        InitializeSounds(woodStep);
        InitializeSounds(stoneStep);
        InitializeSounds(dialogs);
        InitializeSounds(soundEffects);
        InitializeSounds(attack);
        InitializeSounds(swim);
    }

    private void InitializeSounds(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            if (!s.is3D)
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                s.source = s.objectSource.AddComponent<AudioSource>();
                s.source.spatialBlend = 1f;
                s.source.minDistance = s.minDistance;
            }

            s.source.loop = s.loop;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.initialVolume = s.volume;
        }
    }

    public void SetVolume(float volume)
    {
        changeVolume(volume, dialogs);
        changeVolume(volume, forestStep);
        changeVolume(volume, woodStep);
        changeVolume(volume, stoneStep);
        changeVolume(volume, wetStep);
        changeVolume(volume, background);
        changeVolume(volume, soundEffects);
        changeVolume(volume, attack);
        changeVolume(volume, gore);
        changeVolume(volume, swim);

    }

    private void changeVolume(float volume, Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.initialVolume * volume;
        }
    }

    public void Attack(string name)
    {
        Sound s = Array.Find(attack, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        s.source.Play();
    }

    public void SoundEffect(string name, bool playOnce = false)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        if (playOnce)
        {
            if (s.source.isPlaying)
                return;
        }
        s.source.Play();
    }

    public void Background(string name)
    {
        Sound s = Array.Find(background, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        if(s.source != null)
            s.source.Play();
    }

    public void StopBackground(string name)
    {
        Sound s = Array.Find(background, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        s.source.Stop();
    }

    public void Dialog(string name)
    {
        Sound s = Array.Find(dialogs, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        s.source.Play();
    }

    public void Walk(string terrain, float speed)
    {
        if (Time.time < walkTime) return;

        Sound[] soundArray = new Sound[12];

        if (terrain == "forest") soundArray = forestStep;
        else if (terrain == "wood") soundArray = woodStep;
        else if (terrain == "stone") soundArray = stoneStep;
        else if (terrain == "wet") soundArray = wetStep;
        else if (terrain == "water") soundArray = swim;

        if (stepIndex >= soundArray.Length) stepIndex = 0;

        walkTime = Time.time + 45 / speed;
        soundArray[stepIndex].source.Play();

        stepIndex++;
    }

    public Sound[] GetSounds(string type)
    {
        if (type == "wetStep")
        {
            return wetStep;
        }
        else if (type == "gore")
        {
            return gore;
        }
        else if (type == "soundEffects")
        {
            return soundEffects;
        }
        else
        {
            return forestStep;
        }
    }
}
