using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DapperDino.Mirror.Tutorials.Lobby;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText = null;
    [SerializeField] private PlayerNameInput playerNameInput = null;
    [SerializeField] private Animator anim = null;


    private bool isOpened = false;
    private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        GameSaveManager.OnResetGame += GameSaveManager_OnResetGame;
        playerData = Resources.Load<PlayerData>("PlayerData");
        playerNameText.text = playerData.playerName;
    }

    private void GameSaveManager_OnResetGame()
    {
        playerNameText.text = playerData.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleOpenAndClose()
    {
        if (!isOpened)
        {
            UIManager.instance.UpdatePopUp(1);
            isOpened = true;
        }
        else if(playerNameInput.continueButton.interactable && isOpened)
        {
            UIManager.instance.ClosePopUp();
            isOpened = false;
        }
            
    }
    public void Close()
    {
        if (isOpened)
        {
            anim.SetTrigger("close");
            isOpened = false;
        }
        else
        {
            return;
        }
        

    }
}
