using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class AudioClipPair
{
    public AudioClip a;
    public AudioClip b;
}

[Serializable]
public class AudioClipEntry
{
    public string name;
    public AudioClip audio;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource musicPlayer;
    public AudioSource soundFXPlayer;

    public AudioClipPair[] levelMusics;
    public AudioClipEntry[] otherMusics;
    public AudioClipEntry[] soundFXs;

    private Dictionary<string, AudioClip> fxDict = new Dictionary<string, AudioClip>();

    private void Start()
    {
        foreach (AudioClipEntry entry in soundFXs)
        {
            fxDict.Add(entry.name, entry.audio);
        }
    }

    public void PlayLevelMusic(int level, bool isASide, bool restart)
    {
        level -= 1;
        if (level < 0 || level > levelMusics.Length)
        {
            Debug.LogError("wrong music index");
            return;
        }

        musicPlayer.clip = isASide ? levelMusics[level].a : levelMusics[level].b;
        if (restart) musicPlayer.time = 0;
        musicPlayer.Play();
    }

    public void PlayMusic(string name)
    {
        foreach (AudioClipEntry entry in otherMusics)
        {
            if (entry.name == name)
            {
                musicPlayer.clip = entry.audio;
                musicPlayer.time = 0;
                musicPlayer.Play();
                break;
            }
        }
    }

    public void PlayFX(string name)
    {
        soundFXPlayer.clip = fxDict[name];
        soundFXPlayer.time = 0;
        soundFXPlayer.Play();
    }
}
