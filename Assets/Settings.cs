using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] AudioMixer audioMixer;
    private float volume = 1f;
    void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("volume", -20f);
        slider.value = volume;
        audioMixer.SetFloat("Master", volume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("volume", volume);

        PlayerPrefs.Save();
    }

    public void OnSliderValueChanged()
    {
        volume = slider.value;
        audioMixer.SetFloat("Master", volume);
       // Debug.Log(volume);
        SaveSettings();
    }

    
}
