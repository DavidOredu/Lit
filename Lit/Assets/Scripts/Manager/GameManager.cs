using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int targetFps = 42;

    public GameState currentGameState { get; private set; }
    public GameModes.Modes currentGameMode { get; set; }

    protected string currentScene;

    public Racer netPlayer { get; set; }
    public Racer nonNetPlayer { get; set; }

    // used in getting player positions
    public List<Racer> allRacers { get; private set; } = new List<Racer>();
    public List<Racer> playersList { get; private set; } = new List<Racer>();
    public List<float> playerPositions { get; private set; } = new List<float>();

    public StickmanNet[] stickmenNetInGame { get; set; }
    public int numberOfRunners { get; set; }
    public PlayerEvents playerEvents { get; set; }

    private Vector3 startPoint;
    private Vector3 endPoint;

    private Vector3 pointsOffset;


    protected Player playerNN { get; private set; }

    public GameObject powerupManager;
    public GameObject deathLaser;

    public static event Action OnGameCompleted;
    protected bool isGameCompleted = false;

    // used when in a network game
    public List<Player> allNetworkPlayers = new List<Player>();
    public StickmanNet netStickman { get; set; }
    // used when in a non network game
    public List<GameObject> opponents { get; set; } = new List<GameObject>();
    public List<GameObject> powerupManagers { get; private set; } = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = targetFps;
        currentGameMode = GameModes.Modes.Idle;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        playerNN = Resources.Load<Player>("SpawnablePrefabs/Runner(Network)");
        deathLaser = Resources.Load<GameObject>("DeathLaser");
        powerupManager = Resources.Load<GameObject>("PowerupManager");

        isGameCompleted = false;
        OnGameCompleted += GameManagerNon_OnGameCompleted;



        //  AddPlayer();
        //   numberOfRunners = players.Count;

    }
    private void GameManagerNon_OnGameCompleted()
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
                numberOfRunners = GameObject.FindGameObjectsWithTag("Player").Count() + GameObject.FindGameObjectsWithTag("Opponent").Count();

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
        switch (NetworkState.instance.currentNetworkState)
        {
            case NetworkState.State.None:
                break;
                //  for network state behaviour
            case NetworkState.State.Network:
                var players = GameObject.FindGameObjectsWithTag("Player");
                // -------------------INSTANTIATE POWERUP MANAGERS-------------------//
                for (int i = 0; i < players.Count(); i++)
                {
                    var powerupM = Instantiate(powerupManager);
                    powerupManagers.Add(powerupM);
                }

                for (int i = 0; i < players.Count(); i++)
                {
                    Debug.Log($"i is: {i}");
                    powerupManagers[i].GetComponent<PowerupController>().racer = players[i].GetComponent<Player>();
                }
                // -------------------------------------------------------------------//
                break;
                // for non network state behaviour
            case NetworkState.State.NonNetwork:
                var player2 = GameObject.FindGameObjectWithTag("Player");
                opponents = GameObject.FindGameObjectsWithTag("Opponent").ToList();
                // -------------------INSTANTIATE POWERUP MANAGERS-------------------//
                for (int i = 0; i < numberOfRunners; i++)
                {
                    var powerupM = Instantiate(powerupManager);
                    powerupManagers.Add(powerupM);
                }
                powerupManagers[0].GetComponent<PowerupController>().racer = player2.GetComponent<Player>();

                for (int i = 0; i < opponents.Count; i++)
                {
                    Debug.Log($"i is: {i}");
                    powerupManagers[i + 1].GetComponent<PowerupController>().racer = opponents[i].GetComponent<Opponent>();
                }
                // -------------------------------------------------------------------//
                break;
            default:
                break;
        }
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
                        ScoreSystem.QuickplayAndArcadeScore.position = netPlayer.Rank;
                    }
                    break;
                case GameModes.Modes.ClassicDeathmatch:
                    // Set logic for classic deathmatch. Consider making a laser gameobject to chase players and enable if game mode is set to this. Set score calculations, etc
                    //if (!GameObject.FindGameObjectWithTag("DeathLaser"))
                    //    Instantiate(deathLaser);

                    if (isGameCompleted)
                    {
                        ScoreSystem.ClassicDeathmatchScore.positionScore = netPlayer.Rank;
                        ScoreSystem.ClassicDeathmatchScore.litPlatformCount = netPlayer.LitPlatformCount();
                        ScoreSystem.ClassicDeathmatchScore.othersOnLitCount = netPlayer.highestOtherIsOnLitCount;
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.positionScore + " position");
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.litPlatformCount + " litplatforms");
                        Debug.Log(ScoreSystem.ClassicDeathmatchScore.othersOnLitCount + " others on lit");

                    }

                    break;
                case GameModes.Modes.PowerBattle:
                    // Set score calculations, number of powers used, how many runners affected by said powers, etc. Initially locked till x level is reached
                    foreach (var stickman in stickmenNetInGame)
                    {
                        if (!ScoreSystem.PowerBattleScore.runnerColorBoolsNet.ContainsKey(stickman))
                            ScoreSystem.PowerBattleScore.runnerColorBoolsNet.Add(stickman, 0);
                    }
                    Debug.Log(ScoreSystem.PowerBattleScore.runnerColorBoolsNet[stickmenNetInGame[0]] + "score");
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
                        ScoreSystem.QuickplayAndArcadeScore.position = netPlayer.Rank;
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
                        stickmenNetInGame = FindObjectsOfType<StickmanNet>();
                        if (playerEvents == null)
                            playerEvents = netPlayer.GetComponent<PlayerEvents>();
                        SetGame();
                        CheckGameEnd(netPlayer);
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
                        stickmenNetInGame = FindObjectsOfType<StickmanNet>();
                        if (nonNetPlayer == null)
                            nonNetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                        if (playerEvents == null)
                            playerEvents = nonNetPlayer.GetComponent<PlayerEvents>();
                        SetGame();
                        CheckGameEnd(nonNetPlayer);
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
    void SetGame()
    {
        AddPlayer(NetworkState.instance.currentNetworkState);
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
    void AddPlayer(NetworkState.State state)
    {
        List<GameObject> playersnew;
        switch (state)
        {
            case NetworkState.State.None:
                break;
            case NetworkState.State.Network:
                playersnew = GameObject.FindGameObjectsWithTag("Player").ToList();
                for (int i = 0; i < numberOfRunners; i++)
                {
                    Debug.Log($"{i} iteration");
                    var player = playersnew[i].GetComponent<Player>();

                    playersList.Insert(playersList.Count, player);
                    allNetworkPlayers.Insert(allNetworkPlayers.Count, player);
                }
                allRacers = new List<Racer>(playersList);
                playersnew.Clear();
                break;
            case NetworkState.State.NonNetwork:
                playersnew = GameObject.FindGameObjectsWithTag("Opponent").ToList();
                var player2 = nonNetPlayer.gameObject;
                playersnew.Add(player2);
                for (int i = 0; i < numberOfRunners; i++)
                {
                    Debug.Log($"{i} iteration");
                    var player = playersnew[i].GetComponent<Racer>();

                    playersList.Insert(playersList.Count, player);
                }
                allRacers = new List<Racer>(playersList);
                playersnew.Clear();
                break;
            default:
                break;
        }


        //List<Player> playersList = new List<Player>(numberOfRunners);


    }
    void GetRunnerPositions()
    {
        foreach (var player in playersList)
        {
            float position;
            position = player.currentPosition;
            player.currentPositionPercentage = (position / pointsOffset.x) * 100;
            playerPositions.Insert(playersList.IndexOf(player), position);
        }


    }
    void CheckRunnerPositions()
    {
        playerPositions.Sort();
        playerPositions.Reverse();



        foreach (var position in playerPositions)
        {
            foreach (var player in playersList)
            {
                if (player.currentPosition == position)
                {
                    player.Rank = playerPositions.IndexOf(position) + 1;
                }
            }
        }
        playersList.Clear();
        playerPositions.Clear();
        Debug.Log($"playerslist count is: { playersList.Count }");
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
