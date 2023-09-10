using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    void Start()
    {
        LoadSettingss();
    }

    private void LoadSettingss()
    {
        float volume = PlayerPrefs.GetFloat("volume", 0f);
        audioMixer.SetFloat("Master", volume);

    }
}
