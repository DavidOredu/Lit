using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameSaveManager : SingletonDontDestroy<GameSaveManager>
{
    [Tooltip("Always insert a '/' at the beginning of this string")]
    public string allSaveDataPath;
    [Tooltip("Always insert a '.' at the beginning of this string")]
    public string saveFilesExtension;
    public List<SaveData> saveData = new List<SaveData>();
    public List<SaveData> defaultData = new List<SaveData>();

    public Button saveButton;
    public Button loadButton;
    public Button resetButton;

    public static event Action OnResetGame;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        // Initialize the debugging buttons
        saveButton.onClick.AddListener(() => SaveAll());
        loadButton.onClick.AddListener(() => LoadAll());
        resetButton.onClick.AddListener(() => ResetGame());
        // Save the default data before overwriting with the saved data
        SaveDefaultData();
        // Overwrite the default data with the saved data
        LoadAll();
    }
    public bool IsSaveFile()
    {
        // does the save directory exist?
        return Directory.Exists(Application.persistentDataPath + allSaveDataPath);
    }

    public void SaveGame(SaveData saveData)
    {
        // If the save directory doesn't exist, then create it
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + allSaveDataPath);
        }

        // If the specific save directory doesn't exist, then create it
        if (!Directory.Exists(Application.persistentDataPath + allSaveDataPath + saveData.savePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + allSaveDataPath + saveData.savePath);
        }

        /*----------------SERIALIZATION----------------*/

        // Initialize the binary formatter object
        BinaryFormatter bf = new BinaryFormatter();
        // Create the file
        FileStream file = File.Create(Application.persistentDataPath + allSaveDataPath + saveData.savePath + saveData.fileName + saveFilesExtension);
        // initialize the serializer
        var json = JsonUtility.ToJson(saveData.scriptableData);
        // Serialize the data
        bf.Serialize(file, json);
        // close the file to avoid pesky bugs :(
        file.Close();
    }
    public void LoadGame(SaveData saveData)
    {
        // if the save directory doesn't exist, return out of the function... it means that the file has never been saved so no need for loading
        if (!Directory.Exists(Application.persistentDataPath + allSaveDataPath + saveData.savePath))
        {
            return;//TODO: Make it return instead of create a new directory
        }

        /*--------------DESERIALIZATION----------------*/

        // Initialize the binary formatter object
        BinaryFormatter bf = new BinaryFormatter();
        // if the file exists, then we deserialize and overwrite the data on the scriptable object
        if (File.Exists(Application.persistentDataPath + allSaveDataPath + saveData.savePath + saveData.fileName + saveFilesExtension))
        {
            // open the file 
            FileStream file = File.Open(Application.persistentDataPath + allSaveDataPath + saveData.savePath + saveData.fileName + saveFilesExtension, FileMode.Open);
            // derialize the data with the serializer
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), saveData.scriptableData);
            // close the file 
            file.Close();
        }
    }
    // FUNCTION: Used to delete an existing save data
    public void DeleteSaveGame(SaveData saveData)
    {
        // if the directory doesn't exist, return out of the function... it means that the file has never been saved so no need for loading
        if (!Directory.Exists(Application.persistentDataPath + allSaveDataPath + saveData.savePath))
        {
            return;//TODO: Make it return instead of create a new directory
        }
        // if the file exists, then delete it
        if (File.Exists(Application.persistentDataPath + allSaveDataPath + saveData.savePath + saveData.fileName + saveFilesExtension))
        {
            File.Delete(Application.persistentDataPath + allSaveDataPath + saveData.savePath + saveData.fileName + saveFilesExtension);
        }
    }
    // FUNCTION: runs the "SaveGame" function on all save data in the "saveData" list
    public void SaveAll()
    {
        foreach (var saveDatum in saveData)
        {
            SaveGame(saveDatum);
        }
    }
    // FUNCTION: runs the "LoadGame" function on all save data in the "saveData" list
    public void LoadAll()
    {
        foreach (var saveDatum in saveData)
        {
            LoadGame(saveDatum);
        }
    }
    // FUNCTION: runs the "DeleteSaveGame" function on all save data in the "saveData" list
    public void DeleteAllSaves()
    {
        foreach (var saveDatum in saveData)
        {
            DeleteSaveGame(saveDatum);
        }
    }
    // FUNCTION: runs the "SaveGame" function on all save data in the "defaultData" list
    public void SaveDefaultData()
    {
        //    if(PlayerPrefs.GetInt(PlayerPrefKeys.firstTimeSave) == 1) { return; }
        //    if (!PlayerPrefs.HasKey(PlayerPrefKeys.firstTimeSave))
        //  {
        foreach (var saveDatum in defaultData)
        {
            SaveGame(saveDatum);
        }
        //   }
        //   PlayerPrefs.SetInt(PlayerPrefKeys.firstTimeSave, 1);
    }
    // FUNCTION: runs the "LoadGame" function on all save data in the "defaultData" list
    public void LoadDefaultData()
    {
        foreach (var saveDatum in defaultData)
        {
            LoadGame(saveDatum);
        }
    }
    // FUNCTION: makes game reseting possible 
    public void ResetGame()
    {
        // first of all, delete all existing game save files, so as to not conflict with the new save data to come
        DeleteAllSaves();
        // then overwrite the scriptable objects with the data residing in the default saves... this is so the game data in the scriptable objects get affected by the reset
        LoadDefaultData();
        // then with the new data loaded, save it the directories
        SaveAll();
        // lastly, tell all listeners subscribed to the "OnResetGame" event that the game has been reset
        OnResetGame?.Invoke();
    }
    //UNITY_CALLBACK_FUNCTION, FUNCTION: saves the game data before the app closes
    private void OnApplicationQuit()
    {
        // Save the game before the game closes
        SaveAll();
    }
    
}
