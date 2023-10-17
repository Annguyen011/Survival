using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channel;
    AudioSource[] sfxPlayer;
    int channelIndex;

    public enum Sfx
    {
        Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win
    }

    private void Awake()
    {
        instance = this;

        Init(GetSfxVolume());
    }

    private float GetSfxVolume()
    {
        return sfxVolume;
    }

    private void Init(float sfxVolume)
    {
        GameObject bgmObject = new GameObject("BgmLayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObj = new GameObject("sfxLayer");
        sfxObj.transform.parent = transform;
        sfxPlayer = new AudioSource[channel];

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i] = sfxObj.AddComponent<AudioSource>();
            sfxPlayer[i].playOnAwake = false;
            sfxPlayer[i].volume = sfxVolume;

        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            int loopIndex = (i + channelIndex) / sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
            {
                continue;
            }

            int randIndex = 0;
            if(sfx == Sfx.Melee ||sfx == Sfx.Hit)
            {
                randIndex = UnityEngine.Random.Range(0, 2);
            }

            channelIndex = loopIndex;

            sfxPlayer[loopIndex].clip = sfxClip[(int) + randIndex];
            sfxPlayer[loopIndex].Play();
            break;
        }
    }
}
