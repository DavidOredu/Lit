using Cinemachine;
using System;
using System.Collections;
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

    public float gameLogicUpdateTime = 1f;

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
    /// A list reference of all racers registered in a lit match.
    /// </summary>
    public List<Racer> allRacers { get; private set; } = new List<Racer>();

    /// <summary>
    /// A list of all racers still alive and racing in a game scene.
    /// </summary>
    public List<Racer> activeRacers { get; set; } = new List<Racer>();

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
    /// Has a race instance begun?
    /// </summary>
    public bool hasRaceStarted { get; set; } = false;

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
    public Vector3 startPoint { get; set; }
    /// <summary>
    /// The end point of a level in-game.
    /// </summary>
    public Vector3 endPoint { get; set; }

    /// <summary>
    /// Difference between the start point and end point.
    /// </summary>
    public Vector3 pointsOffset { get; set; }

    /// <summary>
    /// Reference to the PowerupManager prefab.
    /// </summary>
    private GameObject powerupManager;
    /// <summary>
    /// Reference to the object pooler prefab.
    /// </summary>
    private GameObject objectPooler;

    public Timer raceCountdownTimer { get; private set; }

    public static event Action OnGameStarted;
    public static event Action OnGameUpdate;
    public static event Action OnGameCompleted;

    public CinemachineVirtualCamera cmVcam { get; private set; }
    public override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = targetFps;
        currentGameMode = GameModes.Modes.Idle;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        powerupManager = Resources.Load<GameObject>("PowerupManager");
        objectPooler = Resources.Load<GameObject>("ObjectPooler");

        OnGameUpdate += SetPositionSystem;

        raceCountdownTimer = new Timer(3f);
        raceCountdownTimer.SetTimer();
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        SetCurrentScene();
        SetCurrentGameState();
        SetGameScoring();
    }
    #region Game State Logic
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
    #endregion

    #region In-Game Logic Initializers
    public void GameStarted()
    {
        StartCoroutine(StartGameLogic());
    }
    public void GameUpdate()
    {
        OnGameUpdate?.Invoke();
    }
    IEnumerator StartGameLogic()
    {
        yield return new WaitUntil(CheckInGameScene);
        OnGameStarted?.Invoke();
        StartCoroutine(UpdateGameLogic());
    }
    IEnumerator UpdateGameLogic()
    {
        yield return new WaitForSecondsRealtime(gameLogicUpdateTime);
        OnGameUpdate?.Invoke();
        Debug.Log("Has updated game logic!");
        StartCoroutine(UpdateGameLogic());
    }
    public void GameCompleted()
    {
        OnGameCompleted?.Invoke();
    }
    #endregion

    public enum GameState
    {
        MainMenu,
        InGame,
        Lobby
    }
    public void InitInGameObjects()
    {
        // -------------------INSTANTIATE AND SETUP POWERUP MANAGERS-------------------//
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

        // -------------------INSTANTIATE AND SETUP OBJECT POOLER-------------------//
        var poolerObj = Instantiate(objectPooler);
        var poolerScript = poolerObj.GetComponent<ObjectPooler>();
        var newPool = new ObjectPooler.Pool("ElementOrb", Resources.Load<GameObject>($"{MainPlayer.runner.stickmanNet.currentColor.colorID}/ElementOrb"), 30);
        poolerScript.pools.Add(newPool);

        // -------------------SET VIRTUAL CAMERA-------------------//
        cmVcam = GameObject.FindGameObjectWithTag("CMvcam").GetComponent<CinemachineVirtualCamera>();
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
                    {
                        ScoreSystem.QuickplayAndArcadeScore.position = MainPlayer.Rank;
                    }
                    break;
                case GameModes.Modes.ClassicDeathmatch:
                    // Set logic for classic deathmatch. Consider making a laser gameobject to chase players and enable if game mode is set to this. Set score calculations, etc
                    //if (!GameObject.FindGameObjectWithTag("DeathLaser"))
                    //    Instantiate(deathLaser);

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
                case GameModes.Modes.Elimination:
                    // Set score system to determine the winner of a domination game. Runner with the most stats wins. Initially locked till x level is reached

                    break;
                case GameModes.Modes.Survival:
                    // Set levels used in the survial gameplay. Increase speed of death laser with every passing level. Initially locked till x level is reached
                    break;
                case GameModes.Modes.Arcade:
                    // Set up a base for a player to set his own stats for a game. Initially locked till x level is reached

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

    #region Positioning System
    void SetPositionSystem()
    {
        GetRunnerPositions();
        CheckRunnerPositions();
    }
    void GetRunnerPositions()
    {
        foreach (var player in activeRacers)
        {
            float position;
            position = player.currentPosition;
            player.currentPositionPercentage = (position / pointsOffset.x) * 100;
            playerPositions.Insert(activeRacers.IndexOf(player), position);
        }
    }
    void CheckRunnerPositions()
    {
        playerPositions.Sort();
        playerPositions.Reverse();

        foreach (var position in playerPositions)
        {
            foreach (var player in activeRacers)
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
        foreach (var racer in activeRacers)
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
    #endregion
    /// <summary>
    /// Checks if we are in the game scene yet.
    /// </summary>
    /// <returns>returns true if we are in game scene.</returns>
    private bool CheckInGameScene()
    {
        return currentGameState == GameState.InGame;
    }

    /// <summary>
    /// Gives the percentage of the position of an object in race, relative to the full distance of the race.
    /// </summary>
    /// <param name="currentPosition">The object's current position.</param>
    /// <param name="normalized">If true, return's the percentage as a value between 0 and 1.</param>
    /// <returns>The percentage value: Float.</returns>
    public float GetObjectPercentageInRace(Vector3 currentPosition, bool normalized = false)
    {
        var diffFromStart = currentPosition - startPoint;
        var percentage = diffFromStart.x / pointsOffset.x;
        if (normalized)
            return percentage;
        else
            return percentage * 100;
    }
}
