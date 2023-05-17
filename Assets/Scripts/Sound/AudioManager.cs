using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectMixerGroup; 
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        Instance = this;
        foreach(Sound s in sounds)
        {
            if (!s.hasParentObject)
            {
                s.source = gameObject.AddComponent<AudioSource>();


                s.source.clip = s.clip;
                s.source.loop = s.isLoop;
                s.source.volume = s.volume;

                switch (s.audioType)
                {
                    case Sound.AudioTypes.soundEffect:
                        s.source.outputAudioMixerGroup = soundEffectMixerGroup;
                        break;
                    case Sound.AudioTypes.music:
                        s.source.outputAudioMixerGroup = musicMixerGroup;
                        break;
                }

                if (s.playOnAwake)
                {
                    s.source.Play();
                }
            }
        }
    }

    public void PlayOnObject(string clipName, GameObject targetObject, bool play, float maxDist)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.name == clipName);

        if(s == null)
        {
            Debug.Log("Sound: " + clipName + "does NOT exist");
            return;
        }

        AudioSource audioSource = targetObject.AddComponent<AudioSource>();

        audioSource.clip = s.clip;
        audioSource.loop = s.isLoop;
        audioSource.volume = s.volume;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = maxDist;
        audioSource.playOnAwake = s.playOnAwake;
        audioSource.bypassEffects = false;

        switch (s.audioType)
        {
            case Sound.AudioTypes.soundEffect:
                audioSource.outputAudioMixerGroup = soundEffectMixerGroup;
                break;
            case Sound.AudioTypes.music:
                audioSource.outputAudioMixerGroup = musicMixerGroup;
                break;
        }

        if(play)
        {
            audioSource.Play();  
        }
    }

    public void PlayOnObject(string clipName, GameObject targetObject, bool play)
    {
        float defaultMaxDist = 100f; 
        PlayOnObject(clipName, targetObject, play, defaultMaxDist);
    }

    public void Play(string clip)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.name == name);
        if(s == null)
        {
            Debug.LogError("Sound: " + name + "does NOT exist!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string clip)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + "does NOT exist!");
            return;
        }
        s.source.Stop();
    }

    public void UpdateAudioMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10( AudioOptionsManager.musicVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("Sound Effect Volume", Mathf.Log10(AudioOptionsManager.soundEffectVolume) * 20);
    }
}
