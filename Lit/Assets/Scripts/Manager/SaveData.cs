using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData 
{
    public enum SaveType
    {
        PlayerData,
        LevelsData,
        LitGameSettings,
        LitLevelData,
        LitUiFeatures,
    }
    [Tooltip("Always insert a '/' at the beginning of this string")]
    public string fileName;
    [Tooltip("Always insert a '/' at the beginning of this string")]
    public string savePath;
    public ScriptableObject scriptableData;
}
