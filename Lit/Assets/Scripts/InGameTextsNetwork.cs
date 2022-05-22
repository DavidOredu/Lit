using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DapperDino.Mirror.Tutorials.Lobby;
using UnityEngine.UI;

public class InGameTextsNetwork : MonoBehaviour
{
    [SerializeField] private Racer racer;
    [SerializeField] private StickmanNet stickman;
    [SerializeField] private TextMeshProUGUI currentPosText;
    [SerializeField] private TextMeshProUGUI maxPosText;
    [SerializeField] private TextMeshProUGUI litCountText;
    [SerializeField] private TextMeshProUGUI litPlatformsText;
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [SerializeField] private GamePlayerLobby gamePlayerLobby;
    [SerializeField] private Slider awakenDurationSlider;
    [SerializeField] private Slider strengthSlider;
    [SerializeField] private Button restartButton;

    protected GameManager gameManager;

    protected GameManager GameManager
    {
        get
        {
            if (gameManager != null) { return gameManager; }
            return gameManager = GameManager.instance;
        }
    }
    private void Start()
    {
        restartButton.onClick.AddListener(() => UIManager.instance.Restart());
    }
    // Update is called once per frame
    void Update()
    {
        if (racer == null)
            racer = gamePlayerLobby.racer;
        if (stickman == null)
            stickman = gamePlayerLobby.stickman;
        maxPosText.text = GameManager.activeRacers.Count.ToString();
        currentPosText.text = racer.Rank.ToString();
        litCountText.text = racer.litPlatforms.Count.ToString();
        litCountText.color = Resources.Load<Material>($"{gamePlayerLobby.colorCode}").color;
        awakenDurationSlider.value = racer.racerAwakening.awakenTimer.CurrentTime() / racer.racerData.awakenTime;
        strengthSlider.value = racer.normalizedStrength;

        if (!GameManager.hasRaceStarted)
        {
            countdownTimerText.gameObject.SetActive(true);
            countdownTimerText.text = GameManager.raceCountdownTimer.CurrentTime().ToString("0");
        }
        else
        {
            // TODO: say "GO" the begin the race
            countdownTimerText.gameObject.SetActive(false);
        }
    }
}
