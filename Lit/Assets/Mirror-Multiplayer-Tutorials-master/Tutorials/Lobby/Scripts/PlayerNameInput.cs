using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class PlayerNameInput : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI playerNameText = null;
        [SerializeField] private TMP_InputField nameInputField = null;
        public Button continueButton = null;

        public static string DisplayName { get; private set; }

        private PlayerData playerData;

        private void Start() {
            GameSaveManager.OnResetGame += GameSaveManager_OnResetGame;
            playerData = Resources.Load<PlayerData>("PlayerData");
            SetUpInputField();
            SetPlayerNameText(); 
        }

        private void GameSaveManager_OnResetGame()
        {
            DisplayName = playerData.playerName;
            nameInputField.text = playerData.playerName;
        }

        private void SetPlayerNameText()
        {
            playerNameText.text = playerData.playerName;
        }
        private void SetUpInputField()
        {
            string defaultName = playerData.playerName;

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            if (!continueButton.interactable) { return; }
            DisplayName = nameInputField.text;
            playerNameText.text = DisplayName;
            playerData.playerName = DisplayName;
        }
    }
}
