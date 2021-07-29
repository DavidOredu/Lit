using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private GameSettings gameSettings;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider SFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        gameSettings = Resources.Load<GameSettings>("LitGameSettings");
        musicVolume.value = gameSettings.musicVolume;
        SFXVolume.value = gameSettings.sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        gameSettings.musicVolume = musicVolume.value;
        gameSettings.sfxVolume = SFXVolume.value;
    }
}
