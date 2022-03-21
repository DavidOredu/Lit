using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class NetworkRoomPlayerLobby : RoomPlayerLobby
    {
        
        [SerializeField] private List<ColorButtonNetwork> colorButtons = new List<ColorButtonNetwork>();

        private void Awake()
        {
            currentColorCode = 10;
            //this is what sets the ready button active later... set it false on start
            hasChosenColor = false;
        }
        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }
        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public override void OnStartAuthority()
        {
            playerData = Resources.Load<PlayerData>("PlayerData");
            CmdSetDisplayName(playerData.playerName);

            lobbyUI.SetActive(true);
        }

        
        private void LateUpdate()
        {

            SetReadyBtn();

        }

        protected override void UpdateDisplay()
        {
            base.UpdateDisplay();

            for (int i = 0; i < colorButtons.Count; i++)
            {
                colorButtons[i].IsPicked = false;
            }
            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                var num = Room.RoomPlayers[i].currentColorCode;
                if (Room.RoomPlayers[i].hasChosenColor)
                    colorButtons[num - 1].IsPicked = true;
                //   Room.RoomPlayers[i].colorPicker.colorButtonNetworks[currentColorCode - 1].IsPicked = true;
            }
        }
        

        

        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;

            Room.NotifyPlayersOfReadyState();
        }

        
        [Command]
        public void CmdPickColor(int color)
        {

            currentColorCode = color;
            hasChosenColor = true;
        }
        
        private void SetReadyBtn()
        {
            if (hasChosenColor)
                readyButton.interactable = true;
        }
    }
}
