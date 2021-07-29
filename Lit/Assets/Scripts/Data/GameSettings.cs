using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Game Settings", menuName = "Data/New Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("SOUND")]
    [Range(0f, 1f)]
    public float musicVolume;
    [Range(0f, 1f)]
    public float sfxVolume;
    [Range(.1f, 3f)]
    public float musicPitch;
    [Range(.1f, 3f)]
    public float sfxPitch;

    
}
