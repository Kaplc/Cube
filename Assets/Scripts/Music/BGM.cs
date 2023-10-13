using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Music/BGM");
        audioSource.loop = true;
        // 初始化数据
        Mute(DataManager.Instance.musicData.musicMute);
        SetVolume(DataManager.Instance.musicData.musicVolume);
        
        audioSource.Play();
    }

    public void Mute(bool isMute)
    {
        audioSource.mute = isMute;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
