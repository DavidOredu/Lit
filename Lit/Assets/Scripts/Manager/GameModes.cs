using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameModes : MonoBehaviour
{
    public GameManager GameManager;
    public GameModeData gameModeData;
    /// <summary>
    /// Reference to the DeathLaser prefab.
    /// </summary>
    private GameObject deathLaserGO;
    private Rigidbody2D deathLaserRB;

    public static event Action OnGameModeStart;
    public static event Action OnGameModeUpdate;
    public static event Action OnGameModeEnd;
    private void Start()
    {
        GameManager.OnGameStarted += GameLogicStart;
        GameManager.OnGameUpdate += GameLogicUpdate;
        GameManager.OnGameCompleted += GameLogicEnd;

        OnGameModeStart += SetGameModeLogic;
    }

    #region Logic Invocations
    private void GameLogicStart()
    {
        OnGameModeStart?.Invoke();

        GameManager.startPoint = GameObject.FindGameObjectWithTag("FlagPoint").transform.Find("StartPoint").transform.position;
        GameManager.endPoint = GameObject.FindGameObjectWithTag("FlagPoint").transform.Find("EndPoint").transform.position;

        GameManager.pointsOffset = GameManager.endPoint - GameManager.startPoint;
    }
    private void GameLogicUpdate()
    {
        OnGameModeUpdate?.Invoke();
        GameManager.gameLogicUpdateTime = gameModeData.logicUpdateTime;
    }
    private void GameLogicEnd()
    {
        OnGameModeEnd?.Invoke();
    }
    #endregion

    #region Logic Initializers
    private void SetGameModeLogic()
    {
        switch (GameManager.currentGameMode)
        {
            case Modes.QuickPlay:
                break;
            case Modes.ClassicDeathmatch:
                DeathMatchStart();
                OnGameModeUpdate += DeathMatchUpdate;
                OnGameModeEnd += DeathMatchEnd;
                break;
            case Modes.PowerBattle:
                PowerBattleStart();
                OnGameModeUpdate += PowerBattleUpdate;
                OnGameModeEnd += PowerBattleEnd;
                break;
            case Modes.Elimination:
                EliminationStart();
                OnGameModeUpdate += EliminationUpdate;
                OnGameModeEnd += EliminationEnd;
                break;
            case Modes.Survival:
                break;
            case Modes.Arcade:
                ArcadeStart();
                OnGameModeUpdate += ArcadeUpdate;
                OnGameModeEnd += ArcadeEnd;
                break;
            case Modes.Tutorial:
                break;
            default:
                break;
        }
    }
    private void RemoveGameLogic()
    {
        switch (GameManager.currentGameMode)
        {
            case Modes.QuickPlay:
                break;
            case Modes.ClassicDeathmatch:
                OnGameModeUpdate -= DeathMatchUpdate;
                OnGameModeEnd -= DeathMatchEnd;
                break;
            case Modes.PowerBattle:
                OnGameModeUpdate -= PowerBattleUpdate;
                OnGameModeEnd -= PowerBattleEnd;
                break;
            case Modes.Elimination:
                OnGameModeUpdate -= EliminationUpdate;
                OnGameModeEnd -= EliminationEnd;
                break;
            case Modes.Survival:
                break;
            case Modes.Arcade:
                OnGameModeUpdate -= ArcadeUpdate;
                OnGameModeEnd -= ArcadeEnd;
                break;
            case Modes.Tutorial:
                break;
            default:
                break;
        }
    }
    #endregion

    
    #region Classic DeathMatch
    public void DeathMatchStart()
    {
        // Instantiate death laser
        deathLaserGO = Instantiate(gameModeData.deathLaserPrefab, gameModeData.deathLaserSpawnPoint, gameModeData.deathLaserPrefab.transform.rotation);
        deathLaserRB = deathLaserGO.GetComponent<Rigidbody2D>();
        // initialize scoring
    }
    public void DeathMatchUpdate()
    {
        if (NoActiveRacers())
        {
            StopDeathLaser();
        }
        else
        {
            MoveDeathLaser();
        }
        
        if (GameManager.activeRacers.Count > 0)
        {
            var lastRacer = GameManager.GetRunnerAtPosition(GameManager.activeRacers.Count);
            var yPosition = Mathf.Lerp(deathLaserGO.transform.position.y, lastRacer.transform.position.y, Time.deltaTime);
            deathLaserGO.transform.position = new Vector2(deathLaserGO.transform.position.x, yPosition);
        }
        StopDeathLaser();
        // update scores
    }
    public void DeathMatchEnd()
    {
        // produce final scores
        if(GameManager.finishedRacers.Count >= 1)
        {
            for (int i = 0; i < GameManager.activeRacers.Count; i++)
            {
                if (GameManager.finishedRacers.Contains(GameManager.activeRacers[i])) { continue; }

                switch (GameManager.activeRacers[i].currentRacerType)
                {
                    case Racer.RacerType.Player:
                        GameManager.activeRacers[i].StateMachine.ChangeState(GameManager.activeRacers[i].playerLoseState);
                        break;
                    case Racer.RacerType.Opponent:
                        GameManager.activeRacers[i].StateMachine.ChangeState(GameManager.activeRacers[i].opponentLoseState);
                        break;
                }
            }
            StartCoroutine(DeathMatchEndCameraEvent(GameManager.cmVcam));
        }
    }
    #endregion

    #region Power Battle
    public void PowerBattleStart()
    {
        // no death laser exists here
        // initialize scoring
    }
    public void PowerBattleUpdate()
    {
        // update scores
    }
    public void PowerBattleEnd()
    {
        // produce final scores
    }
    #endregion

    #region Elimination
    public void EliminationStart()
    {
        // Instantiate death laser
        // set death laser logic
        // set elimination timer
        // initialize scoring
    }
    public void EliminationUpdate()
    {
        // update scores
    }
    public void EliminationEnd()
    {
        // produce final scores
    }
    #endregion

    #region Arcade
    public void ArcadeStart()
    {
        // Instantiate death laser
        // initialize scoring
    }
    public void ArcadeUpdate()
    {
        // update scores
    }
    public void ArcadeEnd()
    {
        // produce final scores
    }
    #endregion

    #region Other Functions
   
    #endregion

    public enum Modes
    {
        Idle,
        QuickPlay,
        ClassicDeathmatch,
        PowerBattle,
        Elimination,
        Survival,
        Arcade,
        Tutorial
    }

    public IEnumerator DeathMatchEndCameraEvent(CinemachineVirtualCamera cmVcam)
    {
        // For the death match
        // if player won
        if (GameManager.finishedRacers[0] == GameManager.MainPlayer)
        {
            yield return new WaitForSeconds(1f);
            cmVcam.Follow = deathLaserGO.transform;
            yield return new WaitUntil(NoActiveRacers);
        }
        else
        {
            // if player lost
            cmVcam.Follow = GameManager.finishedRacers[0].transform;
            yield return new WaitForSeconds(1f);
            cmVcam.Follow = deathLaserGO.transform;
            yield return new WaitUntil(IsMainPlayerDead);
            cmVcam.Follow = GameManager.MainPlayer.transform;
        }

        DetermineGameWinner();
        yield return new WaitForSeconds(2f);
        cmVcam.Follow = GameManager.finishedRacers[0].transform;
    }
    private bool IsMainPlayerDead()
    {
        return GameManager.MainPlayer.StateMachine.DamagedState == GameManager.MainPlayer.playerDeadState;
    }
    private bool NoActiveRacers()
    {
        for (int i = 0; i < GameManager.activeRacers.Count; i++)
        {
            if (GameManager.finishedRacers.Contains(GameManager.activeRacers[i])) { continue; }
            else { return false; }
        }
        return true;
    }
    private void MoveDeathLaser()
    {
        var deathLaserPercentage = GameManager.GetObjectPercentageInRace(deathLaserGO.transform.position);
        var xVelocity = gameModeData.deathLaserVelocityCurve.Evaluate(deathLaserPercentage);
        deathLaserRB.velocity = new Vector2(xVelocity, deathLaserRB.velocity.y);
    }
    private void StopDeathLaser()
    {
        var movementVelocity = deathLaserRB.velocity.x;
        movementVelocity -= 15f * Time.deltaTime;
        movementVelocity = Mathf.Max(movementVelocity, 0);

        deathLaserRB.velocity = new Vector2(movementVelocity, deathLaserRB.velocity.y); 
    }
    private void DetermineGameWinner()
    {
        // tell lit game objects and game manager that the player has won
    }
}
