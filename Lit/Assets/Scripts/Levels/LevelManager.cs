using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DapperDino.Tutorials.Lobby;

public class LevelManager : Singleton<LevelManager>
{
    private GameLevelsData levelsData;
    private LevelLoader levelLoader;

    public LevelButton selectedLevel { get; set; }

    public Transform map_1_Parent;
    public Transform map_2_Parent;
    public Transform map_3_Parent;
    public Transform map_4_Parent;
    public Transform map_5_Parent;
    public Transform map_6_Parent;
    public Transform map_7_Parent;

    public GameObject buttonPrefab;
    public Vector3 buttonScale;

    public List<Level> map1Levels = new List<Level>();
    public List<Level> map2Levels = new List<Level>();
    public List<Level> map3Levels = new List<Level>();
    public List<Level> map4Levels = new List<Level>();
    public List<Level> map5Levels = new List<Level>();
    public List<Level> map6Levels = new List<Level>();
    public List<Level> map7Levels = new List<Level>();

    public List<List<Level>> mapsList { get; set; } = new List<List<Level>>();
    public override void Awake()
    {
        base.Awake();
        levelsData = Resources.Load<GameLevelsData>("LevelsData");
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        buttonPrefab = Resources.Load<GameObject>("LevelButton");
    }
    // Start is called before the first frame update
    void Start()
    {
        mapsList.Add(map1Levels);
        mapsList.Add(map2Levels);
        mapsList.Add(map3Levels);
        mapsList.Add(map4Levels);
        mapsList.Add(map5Levels);
        mapsList.Add(map6Levels);
        mapsList.Add(map7Levels);

        SetLevels();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelsToUnlock();
    }
   
    public int GetStarsForMap(int mapNumber)
    {
        int stars = 0;
        foreach (var level in mapsList[mapNumber - 1])
        {
            stars += level.stars;
        }
        return stars;
    }

    private void CheckLevelsToUnlock()
    {
        //foreach(var map in mapsList)
        //{
        //    foreach (var level in map)
        //    {
        //        if(GetStarsForMap(mapsList.IndexOf(map) + 1) >= level.starsToUnlock)
        //        {
        //            level.isUnlocked = true;
        //            Save();
        //        }
        //    }
        //}
        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach(var button in allButtons)
        {
            var levelButton = button.GetComponent<LevelButton>();
            if(GetStarsForMap(levelButton.level.buttonMap) >= levelButton.level.starsToUnlock)
            {
                levelButton.level.isUnlocked = true;
                Save(button);
            }
            else
            {
                levelButton.level.isUnlocked = false;
                Save(button);
            }
        }
    }
    void SetLevels()
    {
        // Before we start the iteration, we need to set the level number all the levels in level data so they correspond to the target level... This is done to make naming more dynamic and automatic
        AdjustLevelsDataLevelNumbers();

        for (int i = 0; i < mapsList.Count; i++)
        {
            for (int x = 0; x < mapsList[i].Count; x++)
            {
                // simple variables to use instead of saying "mapsList[i][x]" or "mapsList[i]" every time
                var currentLevel = mapsList[i][x];
                var currentMap = mapsList[i];

                //-------LOADING THE LEVEL AND OVERWRITING-------//

                //Get the position of the level to overwrite with the saved level data
                var factor = currentMap.Count;
                var mapPos = mapsList.IndexOf(currentMap);
                var pos = currentMap.IndexOf(currentLevel);

                // The position of this level in the levels save data
                int positionInData = MapsCounter(mapsList, mapPos) + pos;
                Debug.Log(positionInData + " is the level position!");
                // Get the level stats from the saved level list
                ChangeLevelAttributes(currentLevel, levelsData.levels[positionInData]);

                //-----------------------------------------------//

                // The map that this current level belongs to
                int currentButtonMap = mapsList.IndexOf(currentMap) + 1;
                // The position of the level in its map will serve as the scene number to load
                currentLevel.levelPos = pos + 1;
                // Set the the current map the level belongs to in the level instance... Used to get the number of stars available in that current map : list of levels
                currentLevel.buttonMap = currentButtonMap;
                // Sets the scene to load when button is clicked
                currentLevel.levelScene = Resources.Load<MapSet>("Map" + currentButtonMap.ToString());

                //Create the button in the game
                GameObject levelButton = Instantiate(buttonPrefab);

                // Get neccessary components and set them 
                var levelButtonComponent = levelButton.GetComponent<LevelButton>();
                levelButtonComponent.levelButton = levelButton.GetComponent<Button>();
                levelButtonComponent.levelText = levelButton.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
                levelButtonComponent.gameModeImage = levelButton.transform.Find("GameModeImage").GetComponent<Image>();
                levelButtonComponent.level = currentLevel;


                // Set the values in the grabbed components
                levelButtonComponent.levelText.text = currentLevel.levelNumber.ToString();
                
                Debug.Log(levelButtonComponent.level.levelGameMode.ToString());
                levelButtonComponent.gameModeImage.sprite = Resources.Load<GameObject>(levelButtonComponent.level.levelGameMode.ToString() + "Icon").GetComponent<Image>().sprite;
                //           if (levelsData.levels[ levelButtonComponent.level.levelNumber - 1].isUnlocked == true)
                //           {
                //               currentLevel.isUnlocked = true;
                //           }
                //           else
                //           {
                //               currentLevel.isUnlocked = false;
                //           }
                ////           levelButtonComponent.levelButton.interactable = currentLevel.isUnlocked;
                ///

                // Set the level the button directs to
                EnterLevel(levelButtonComponent);

                // Structure the button
                switch (levelButtonComponent.level.buttonMap)
                {
                    case 1:
                        levelButton.transform.SetParent(map_1_Parent);
                        break;
                    case 2:
                        levelButton.transform.SetParent(map_2_Parent);
                        break;
                    case 3:
                        levelButton.transform.SetParent(map_3_Parent);
                        break;
                    case 4:
                        levelButton.transform.SetParent(map_4_Parent);
                        break;
                    case 5:
                        levelButton.transform.SetParent(map_5_Parent);
                        break;
                    case 6:
                        levelButton.transform.SetParent(map_6_Parent);
                        break;
                    case 7:
                        levelButton.transform.SetParent(map_7_Parent);
                        break;
                    default:
                        break;
                }
                
                levelButton.SetActive(true);
                levelButton.transform.localPosition = new Vector3(levelButton.transform.localPosition.x, levelButton.transform.localPosition.y, 1);
                levelButton.transform.localScale = new Vector3(buttonScale.x, buttonScale.y, buttonScale.z);
            }
        }
    
        
    }
    private void ChangeLevelAttributes(Level changing, Level changer)
    {
        changing.levelPos = changer.levelPos;
        changing.buttonMap = changer.buttonMap;
        changing.isUnlocked = changer.isUnlocked;
        changing.levelGameMode = changer.levelGameMode;
        changing.levelNumber = changer.levelNumber;
        changing.levelScene = changer.levelScene;
        changing.numberOfRounds = changer.numberOfRounds;
        changing.stars = changer.stars;
        changing.starsToUnlock = changer.starsToUnlock;
        changing.OnClick = changer.OnClick;
        changing.numberOfOpponentsInLevel = changer.numberOfOpponentsInLevel;
    }
    private void AdjustLevelsDataLevelNumbers() 
    {
        foreach(var level in levelsData.levels)
        {
            var pos = levelsData.levels.IndexOf(level);
            level.levelNumber = pos + 1;
        }
    } 
    public void Save(GameObject button)
    {
      //  GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
      //  foreach (var button in allButtons)
       // {
            LevelButton levelButton = button.GetComponent<LevelButton>();
        //    levelsData.levels[ levelButton.level.levelNumber - 1] = levelButton.level;
        ChangeLevelAttributes(levelsData.levels[levelButton.level.levelNumber - 1], levelButton.level);
      //  }
    }
    int MapsCounter(List<List<Level>> mapList, int posOfMap)
    {
        int counter = 0;
        if(posOfMap == 0) { return 0; }
        posOfMap--;
        while (posOfMap >= 0) 
        {
            counter += mapList[posOfMap].Count;
            posOfMap--;
        }

        return counter;
    }
    public void EnterLevel(LevelButton levelButtonComponent)
    {
        MapHandler mapHandler = new MapHandler(levelButtonComponent.level.levelScene, levelButtonComponent.level.numberOfRounds);
        levelButtonComponent.levelButton.onClick.AddListener(() => levelLoader.LoadLevel(mapHandler.MapLevel(levelButtonComponent.level.levelPos)));
        levelButtonComponent.levelButton.onClick.AddListener(() => levelButtonComponent.level.ChangeGameMode());
        levelButtonComponent.levelButton.onClick.AddListener(() => UIManager.instance.UpdateUI(100));
    }
}
