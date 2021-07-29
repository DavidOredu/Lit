using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class AudioManager : SingletonDontDestroy<AudioManager>, IPointerEnterHandler, IPointerClickHandler
{
    public Sound[] sounds;
    private GameSettings gameSettings;
 
    // Start is called before the first frame update
   public override void Awake()
    {
        base.Awake();
        gameSettings = Resources.Load<GameSettings>("LitGameSettings");
        foreach (var sound in sounds)
        {
            switch (sound.soundType)
            {
                case UiFeatures.SoundType.Music:
                    sound.volume = gameSettings.musicVolume;
                    sound.pitch = gameSettings.musicPitch;
                    sound.loop = true;
                    break;
                case UiFeatures.SoundType.SFX:
                    sound.volume = gameSettings.sfxVolume;
                    sound.pitch = gameSettings.sfxPitch;
                    break;
                default:
                    break;
            }
        }
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

       }

    private void Start()
    {
        PlaySound("Automatav");
    }
    private void Update()
    {
            foreach (var sound in sounds)
            {
                switch (sound.soundType)
                {
                    case UiFeatures.SoundType.Music:
                     sound.volume = gameSettings.musicVolume;
                    sound.pitch = gameSettings.musicPitch;
                        break;
                    case UiFeatures.SoundType.SFX:
                    sound.volume = gameSettings.sfxVolume;
                    sound.pitch = gameSettings.sfxPitch;
                        break;
                   default:
                       break;
                }
            }
        foreach (Sound sound in sounds)
        {
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    // Update is called once per frame

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

            //if (UIManager.GameIsPaused)
            //{
            //    // play pause sound
            //    s.source.Pause();
            //}
        
         
        s.source.Play();
    }
    public void PlayDelayedSound(string name, float delayTime)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (UIManager.GameIsPaused)
        {
            // play pause sound
            s.source.Pause();
        }
        s.source.PlayDelayed(delayTime);
    }
    public void PlayOneShotSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (UIManager.GameIsPaused)
        {
            // play pause sound
            s.source.Pause();
        }
        s.source.PlayOneShot(s.clip);
    }
    public Sound FindSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.SoundName == name)
            {
                return s;
            }
            else
            {
                continue;
            }
        }
        Debug.LogError("Given name does not correspond to any sound name existing in the sound array. Make sure the spelling corresponds to the required sound name or add a sound to fit that name.");
        return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
