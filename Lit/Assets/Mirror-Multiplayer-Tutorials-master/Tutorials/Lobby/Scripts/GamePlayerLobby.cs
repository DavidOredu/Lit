﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    /// <summary>
    /// Governs inner logic and network behaviour of a runner, while in-game.
    /// </summary>
    public class GamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";
        [SyncVar]
        public int colorCode;
        [SyncVar(hook = nameof(HandleGameSceneBool))]
        public bool inGameScene = false;

        [SerializeField] private List<TargetTrackerNetwork> targetTrackers = new List<TargetTrackerNetwork>(9);

        public Racer.RacerType gamePlayerType;
        public GameObject gameStatsPanel;
        public Racer racer;
        public StickmanNet stickman;
        public Entity.Difficulty difficulty;

        public Powerup powerup;
        public PowerupButton powerupButton;
        public Button awakenedStateButton;
        public EnemyPowerup enemyPowerup;
        public Slider strengthBar;

        protected GameManager gameManager;
        protected NetworkManagerLobby room;
        protected GameManager GameManager
        {
            get
            {
                if (gameManager != null) { return gameManager; }
                return gameManager = GameManager.instance;
            }
        }
        protected NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        private void Start()
        {
            StartCoroutine(CheckIfInGame());
        }

        IEnumerator CheckIfInGame()
        {
            yield return new WaitUntil(CheckScene);
            inGameScene = true;
        }

        bool CheckScene()
        {
            return SceneManager.GetActiveScene().path.Contains("Scene_Game");
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
            foreach (var player in GameManager.allStickmenColors)
            {
                if (player.code == colorCode)
                {
                    GameManager.allStickmenColors.Remove(player);
                    return;
                }

            }
            UpdateDisplay();
        }


        public virtual void UpdateDisplay()
        {
            // Run for a player even if he doesn't have authority. This is done so that a non-server player does not have administrative rights to call this function.
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

            GameManager.numberOfRunners = Room.GamePlayers.Count;

            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                if (Room.GamePlayers[i].GetComponent<NetworkIdentity>().hasAuthority)
                {
                    GameManager.MainPlayer = Room.GamePlayers[i].racer;
                    GameManager.MainStickman = Room.GamePlayers[i].stickman;

                    GameManager.instance.InitInGameObjects();
                    if (gamePlayerType == Racer.RacerType.Player)
                        awakenedStateButton.onClick.AddListener(() => racer.Awaken(racer.runner.stickmanNet.currentColor.colorID));
                }
            }
            GameManager.allRacers.Reverse();
            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                Room.GamePlayers[i].racer = GameManager.allRacers[i].gameObject.GetComponent<Racer>();
                Room.GamePlayers[i].stickman = GameManager.allRacers[i].gameObject.GetComponent<StickmanNet>();
                Room.GamePlayers[i].racer.GamePlayer = Room.GamePlayers[i];
            }
            GameManager.allRacers.Reverse();

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
                targetTrackers[i].player = GameManager.allRacers[i];
                targetTrackers[i].stickman = GameManager.allStickmenColors[i];
            }

            for (int i = 0; i < Room.GamePlayers.Count; i++)
            {
                if (Room.GamePlayers[i].strengthBar != null)
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
