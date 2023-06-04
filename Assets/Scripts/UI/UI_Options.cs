using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_Options : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    AudioManager audioManager;

    private void Awake()
    {
    }
    private void Start()
    {
        audioManager = AudioManager.instance;

        musicSlider.value = 1f;
        sfxSlider.value = 1f;
        musicSlider.onValueChanged.AddListener((value) =>
        {
            SetMusicVolume(value);
        });

        sfxSlider.onValueChanged.AddListener((value) =>
        {
            SetSFXVolume(value);
        });
    }

    public void SetMusicVolume (float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioManager.SetSFX(volume);
    }

    public void ToggleMVMute(bool isMuted)
    {
        musicSlider.interactable = !isMuted;
        if (isMuted)
            audioManager.SetMusicVolume(0);
        else
            audioManager.SetMusicVolume(sfxSlider.value);
    }

    public void ToggleSFXMute(bool isMuted)
    {
        sfxSlider.interactable = !isMuted;
        if (isMuted)
            audioManager.SetSFX(0);
        else
            audioManager.SetSFX(sfxSlider.value);
    }
}
