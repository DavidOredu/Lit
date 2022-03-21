using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DapperDino.Mirror.Tutorials.Lobby;

public class InGameTextsNetwork : MonoBehaviour
{
    [SerializeField] private Racer racer;
    [SerializeField] private StickmanNet stickman;
    [SerializeField] private TextMeshProUGUI currentPosText;
    [SerializeField] private TextMeshProUGUI maxPosText;
    [SerializeField] private TextMeshProUGUI litCountText;
    [SerializeField] private TextMeshProUGUI litPlatformsText;
    [SerializeField] private GamePlayerLobby gamePlayerLobby;

    protected GameManager gameManager;

    protected GameManager GameManager
    {
        get
        {
            if (gameManager != null) { return gameManager; }
            return gameManager = GameManager.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (racer == null)
            racer = gamePlayerLobby.racer;
        if (stickman == null)
            stickman = gamePlayerLobby.stickman;
        maxPosText.text = GameManager.numberOfRunners.ToString();
        currentPosText.text = racer.Rank.ToString();
        litCountText.text = racer.litPlatforms.Count.ToString();
        litCountText.color = Resources.Load<Material>($"{gamePlayerLobby.colorCode}").color;
        if(racer.litPlatforms.Count == 1)
        {
            litPlatformsText.text = "Litplatform";
        }
        else
        {
            litPlatformsText.text = "Litplatforms";
        }

    }
}
