using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class GamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";
        [SyncVar]
        public int colorCode;
        [SyncVar(hook = nameof(HandleGameSceneBool))]
        public bool inGameScene = false;
        [SerializeField] private List<TargetTrackerNetwork> targetTrackers = new List<TargetTrackerNetwork>(9);
        protected List<StickmanNet> players;

        public PlayerData playerData;
        public Racer.RacerType gamePlayerType;
        public GameObject gameStatsPanel;
        public Racer racer;
        public StickmanNet stickman;
        public Entity.Difficulty difficulty;

       bool hasInit = false;

        public Powerup powerup;
        public PowerupButton powerupButton;
        public Button awakenedStateButton;
        public EnemyPowerup enemyPowerup;
        public Slider strengthBar;
        private GameManager gameManager;
        protected NetworkManagerLobby room;
        protected NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        IEnumerator ChangeBool()
        {
            yield return new WaitUntil(CheckScene);
          //  yield return new WaitForSeconds(3f);
            inGameScene = true;
        }
        private void Awake()
        {
           
        }
        bool CheckScene()
        {
            if (SceneManager.GetActiveScene().path.Contains("Scene_Game"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [Client]
        public virtual void Update()
        {

            if (gameManager == null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }
            //if (Room.GamePlayers.Count == Room.players.Count)
            // {
            //     Debug.Log($"Trying to run update function in {ToString()}");
            //     CmdUpdateDisplay();
            //     Debug.Log("Has run update display in the update function!");
            // }
            if (SceneManager.GetActiveScene().name.Contains("Scene_Game") && gameManager.numberOfRunners == Room.numberOfRoomPlayers)
            {
                StartCoroutine(ChangeBool());
                var players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < Room.GamePlayers.Count; i++)
                {
                    if (Room.GamePlayers[i].GetComponent<NetworkIdentity>().hasAuthority && (gameManager.netPlayer == null || gameManager.netStickman == null))
                    {
                        gameManager.netPlayer = Room.GamePlayers[i].racer;
                        gameManager.netStickman = Room.GamePlayers[i].stickman;
                    }
                    if (Room.GamePlayers[i].GetComponent<NetworkIdentity>().hasAuthority && !hasInit && Room.GamePlayers[i].gamePlayerType == Racer.RacerType.Player)
                    {
                        GameManager.instance.InitInGameObjects();
                        if(gamePlayerType == Racer.RacerType.Player)
                        awakenedStateButton.onClick.AddListener(() => racer.Awaken(racer.runner.stickmanNet.currentColor.colorID));
                        hasInit = true;
                    }
                   else { continue; }
                }

            }
            
            //Debug.Log($"{Room.GamePlayers.Count} Game Players exist!");
            //Debug.Log($"{Room.RoomPlayers.Count} Room player exist!");
            //Debug.Log($"{Room.players.Count} Stickman Players exist!");

            UpdateDisplay();
        }
        public override void OnStartAuthority()
        {
            gameStatsPanel.SetActive(true);
        }
        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
            
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
            Room.gamePlayerConnect.Remove(this);
            foreach(var player in Room.players)
            {
                if(player.code == colorCode)
                {
                    Room.players.Remove(player);
                    return;
                }

            }
            UpdateDisplay();
        }
       
       
        public virtual void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var player in Room.GamePlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }

                return;
            }
            //var myIndex = Room.GamePlayers.IndexOf(this);
            //Room.gamePlayerConnect[this] = Room.players[myIndex].gameObject;
            //player = Room.gamePlayerConnect[this].GetComponent<PlayerNN>();
            //stickman = Room.gamePlayerConnect[this].GetComponent<StickmanNet>();
            //var obj = GameObject.FindGameObjectsWithTag("Player");
            //foreach (var item in obj)
            //{
            //    var i = item.GetComponent<Stickman>();
            //    players.Add(i);
            //}
            //for (int i = 0; i < Room.GamePlayers.Count; i++)
            //{
            //    players[i].code = Room.GamePlayers[i].colorCode;
            //}
            //players.Clear();

            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                if(Room.GamePlayers[i].racer != players[i].gameObject.GetComponent<Racer>())
                    Room.GamePlayers[i].racer = players[i].gameObject.GetComponent<Racer>();
                if (Room.GamePlayers[i].stickman != players[i].gameObject.GetComponent<StickmanNet>())
                    Room.GamePlayers[i].stickman = players[i].gameObject.GetComponent<StickmanNet>();
                if (Room.GamePlayers[i].racer.GamePlayer != Room.GamePlayers[i])
                    Room.GamePlayers[i].racer.GamePlayer = Room.GamePlayers[i];

                if (Room.GamePlayers[i].gamePlayerType == Racer.RacerType.Player)
                {
                    awakenedStateButton.interactable = Room.GamePlayers[i].racer.canAwaken;
                }
            }
            
            //var myPosition = Room.GamePlayers.IndexOf(this);
            //foreach (var player in players)
            //{
            //    if(players[myPosition] == player)
            //    {
            //        player.gameObject.GetComponent<PlayerNN>().inputHandler.enabled = true;
            //    }
            //    else
            //    {
            //        player.gameObject.GetComponent<PlayerNN>().inputHandler.enabled = false;
            //    }
            //}
            
            for (int i = 0; i < targetTrackers.Count; i++)
            {
                targetTrackers[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                targetTrackers[i].gameObject.SetActive(true);  
            }
            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                targetTrackers[i].player = players[i].gameObject.GetComponent<Racer>();//Room.gamePlayerConnect[Room.GamePlayers[i]].gameObject.GetComponent<PlayerNN>();
                targetTrackers[i].stickman = players[i].gameObject.GetComponent<StickmanNet>();//Room.gamePlayerConnect[Room.GamePlayers[i]].gameObject.GetComponent<StickmanNet>();
            }

            //player = Room.gamePlayerConnect[this].GetComponent<PlayerNN>();
            //stickman = Room.gamePlayerConnect[this].GetComponent<StickmanNet>();

            //foreach (var stickman in Room.players)
            //{
            //    foreach (var player in Room.GamePlayers)
            //    {
            //        if(stickman.displayName == player.displayName)
            //        {
            //            stickman.code = player.colorCode
            //        }
            //    }
            //}
            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                if(Room.GamePlayers[i].strengthBar != null)
                    Room.GamePlayers[i].strengthBar.value = Room.GamePlayers[i].racer.normalizedStrength;
            }
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
        [Server]
        public void SetColor(int code)
        {
            colorCode = code;
        }
        public void HandleGameSceneBool(bool oldValue, bool newValue)
        {
            UpdateDisplay();
        }
    }
}
