using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager script to govern the affairs of game states and render logic for these individual states.
/// </summary>
[Serializable]
public class GameManager : SingletonDontDestroy<GameManager>
{
    /// <summary>
    /// FPS to be aimed to render at.
    /// </summary>
    [SerializeField]
    private int targetFps = 42;

    /// <summary>
    /// The present state the game is in. If in menu, or in-game or lobby, etc.
    /// </summary>
    public GameState currentGameState;
    /// <summary>
    /// The current mode being played in the game scene.
    /// </summary>
    public GameModes.Modes currentGameMode;
    /// <summary>
    /// The name of the current scene rendered in the game.
    /// </summary>
    protected string currentScene;

    /// <summary>
    /// The main racer object in-game. The runner controlled by our game instance.
    /// </summary>
    public Racer MainPlayer { get; set; }
    /// <summary>
    /// The main stickman object in-game. The runner controlled by our game instance.
    /// </summary>
    public StickmanNet MainStickman { get; set; }

    /// <summary>
    /// A list reference of all racers found in a game scene.
    /// </summary>
    public List<Racer> allRacers { get; private set; } = new List<Racer>();
    /// <summary>
    /// A list reference of all stickmen found in a game scene.
    /// </summary>
    public List<StickmanNet> allStickmenColors { get; private set; } = new List<StickmanNet>();
    /// <summary>
    /// Cache to configure runner positions.
    /// </summary>
    public List<float> playerPositions { get; private set; } = new List<float>();
    /// <summary>
    /// A list reference to all PowerupManagers found in-game.
    /// </summary>
    public List<GameObject> powerupManagers { get; private set; } = new List<GameObject>();
    /// <summary>
    /// A cache of all finished racers.
    /// </summary>
    public List<Racer> finishedRacers = new List<Racer>();

    /// <summary>
    /// A reference to the current game level.
    /// </summary>
    public Level currentLevel { get; set; }
    /// <summary>
    /// A number of all runners found in a game scene.
    /// </summary>
    public int numberOfRunners { get; set; }

    /// <summary>
    /// The start point of a level in-game.
    /// </summary>
    private Vector3 startPoint;
    /// <summary>
    /// The end point of a level in-game.
    /// </summary>
    private Vector3 endPoint;

    /// <summary>
    /// Difference between the start point and end point.
    /// </summary>
    private Vector3 pointsOffset;

    /// <summary>
    /// Reference to the PowerupManager prefab.
    /// </summary>
    public GameObject powerupManager;
    /// <summary>
    /// Reference to the DeathLaser prefab.
    /// </summary>
    public GameObject deathLaser;


    protected bool isGameCompleted = false;

    public static event Action OnGameCompleted;


    public override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = targetFps;
        currentGameMode = GameModes.Modes.Idle;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        deathLaser = Resources.Load<GameObject>("DeathLaser");
        powerupManager = Resources.Load<GameObject>("PowerupManager");

        isGameCompleted = false;
        OnGameCompleted += GameManager_OnGameCompleted;
    }
    private void GameManager_OnGameCompleted()
    {
        Debug.Log("Game has ended.Runner reached the finish line!");
        isGameCompleted = true;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        SetCurrentScene();
        SetCurrentGameState();
        SetLogicForGameState();
        SetConditioning();
        SetGameScoring();
    }

    void SetCurrentScene()
    {
        currentScene = SceneManager.GetActiveScene().path;
    }
    void SetCurrentGameState()
    {
        if (currentScene.Contains("Scene_MainMenu"))
        {
            currentGameState = GameState.MainMenu;
        }
        else if (currentScene.Contains("Scene_Game"))
        {
            currentGameState = GameState.InGame;
        }
        else if (currentScene.Contains("Scene_Lobby"))
        {
            currentGameState = GameState.Lobby;
        }
        else
        {
            Debug.LogError("The current scene does not have a registered name. Try to register the scene for the game to recognise it.");
        }
    }
    void SetLogicForGameState()
    {
        switch (currentGameState)
        {
            case GameState.MainMenu:
                break;
            case GameState.InGame:
                startPoint = GameObject.FindGameObjectWithTag("FlagPoint").transform.Find("StartPoint").transform.position;
                endPoint = GameObject.FindGameObjectWithTag("FlagPoint").transform.Find("EndPoint").transform.position;

                pointsOffset = endPoint - startPoint;
                break;
            case GameState.Lobby:
                break;
            default:
                break;
        }
    }
    public void GameCompleted()
    {
        OnGameCompleted?.Invoke();
    }


    public enum GameState
    {
        MainMenu,
        InGame,
        Lobby
    }
    public void InitInGameObjects()
    {
        // -------------------INSTANTIATE POWERUP MANAGERS-------------------//
        for (int i = 0; i < numberOfRunners; i++)
        {
            var powerupM = Instantiate(powerupManager);
            powerupManagers.Add(powerupM);
        }

        for (int i = 0; i < numberOfRunners; i++)
        {
            PowerupController powerupController = powerupManagers[i].GetComponent<PowerupController>();
            powerupController.racer = allRacers[i];
        }
        // -------------------------------------------------------------------//
    }
    public void SetGameScoring()
    {
        if (currentGameState == GameState.InGame)
        {
            switch (currentGameMode)
            {
                case GameModes.Modes.Idle:
                    // Mode selected when none is.
                    break;
                case GameModes.Modes.QuickPlay:
                    // Set random structure for quick play. Initially locked till level 2 or 3 is reached
                    if (isGameCompleted)
                    {
                        ScoreSystem.QuickplayAndArcadeScore.position = MainPlayer.Rank;
                    }
                    break;
                case GameModes.Modes.ClassicDeathmatch:
                    // Set logic for classic deathmatch. Consider making a laser gameobject to chase players and enable if game mode is set to this. Set score calculations, etc
                    //if (!GameObject.FindGameObjectWithTag("DeathLaser"))
                    //    Instantiate(deathLaser);

                    if (isGameCompleted)
                    {
                        ScoreSystem.ClassicDeathmatchScore.positionScore = MainPlayer.Rank;
                        ScoreSystem.ClassicDeathmatchScore.litPlatformCount = MainPlayer.LitPlatformCount();
                        ScoreSystem.ClassicDeathmatchScore.othersOnLitCount = MainPlayer.highestOtherIsOnLitCount;
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.positionScore + " position");
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.litPlatformCount + " litplatforms");
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.othersOnLitCount + " others on lit");

                    }

                    break;
                case GameModes.Modes.PowerBattle:
                    // Set score calculations, number of powers used, how many runners affected by said powers, etc. Initially locked till x level is reached
                    foreach (var stickman in allStickmenColors)
                    {
                        if (!ScoreSystem.PowerBattleScore.runnerColorBoolsNet.ContainsKey(stickman))
                            ScoreSystem.PowerBattleScore.runnerColorBoolsNet.Add(stickman, 0);
                    }
                    //     Debug.Log(ScoreSystem.PowerBattleScore.runnerColorBoolsNet[stickmenNetInGame[0]] + "score");
                    break;
                case GameModes.Modes.Domination:
                    // Set score system to determine the winner of a domination game. Runner with the most stats wins. Initially locked till x level is reached

                    break;
                case GameModes.Modes.Survival:
                    // Set levels used in the survial gameplay. Increase speed of death laser with every passing level. Initially locked till x level is reached
                    break;
                case GameModes.Modes.Arcade:
                    // Set up a base for a player to set his own stats for a game. Initially locked till x level is reached
                    if (isGameCompleted)
                    {
                        ScoreSystem.QuickplayAndArcadeScore.position = MainPlayer.Rank;
                    }
                    break;
                case GameModes.Modes.Tutorial:
                    // The first mode players are welcomed by. This is the level that teaches a player the basic mechanics of lit!
                    break;
                default:
                    break;
            }
        }
    }
    void SetConditioning()
    {
        switch (NetworkState.instance.currentNetworkState)
        {
            case NetworkState.State.None:
                break;
            case NetworkState.State.Network:
                switch (currentGameState)
                {
                    case GameState.MainMenu:

                        break;
                    case GameState.InGame:
                        SetPositionSystem();
                        CheckGameEnd(MainPlayer);
                        break;
                    case GameState.Lobby:
                        break;
                    default:
                        break;
                }
                break;
            case NetworkState.State.NonNetwork:
                switch (currentGameState)
                {
                    case GameState.MainMenu:

                        break;
                    case GameState.InGame:
                        SetPositionSystem();
                        CheckGameEnd(MainPlayer);
                        break;
                    case GameState.Lobby:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }
    void SetPositionSystem()
    {
        GetRunnerPositions();
        CheckRunnerPositions();
    }
    void CheckGameEnd(Racer player)
    {
        if (player.transform.position.x >= endPoint.x)
        {
            GameCompleted();
        }
    }

    void GetRunnerPositions()
    {
        foreach (var player in allRacers)
        {
            float position;
            position = player.currentPosition;
            player.currentPositionPercentage = (position / pointsOffset.x) * 100;
            playerPositions.Insert(allRacers.IndexOf(player), position);
        }
    }
    void CheckRunnerPositions()
    {
        playerPositions.Sort();
        playerPositions.Reverse();

        foreach (var position in playerPositions)
        {
            foreach (var player in allRacers)
            {
                if (player.currentPosition == position)
                {
                    player.Rank = playerPositions.IndexOf(position) + 1;
                }
            }
        }
        playerPositions.Clear();
    }

    public Racer GetRunnerAtPosition(int position)
    {
        Racer runner = null;
        foreach (var racer in allRacers)
        {
            if (racer.Rank == position)
            {
                runner = racer;
                break;
            }
            else
            {
                continue;
            }
        }
        return runner;
    }
}
