using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.Mirror.Tutorials.Lobby;

public class PlayerLobby : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Stickman stickman;
    public PlayerData playerData;
    private LevelSystemAnimated levelSystemAnimated;

    
    // Start is called before the first frame update
    void Start()
    {
        stickman = GetComponent<Stickman>();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("idle", true);
        //if (networkRoomPlayerLobby.IsReady)
        //{
        //    anim.SetBool("idle", false);
        //}
    }
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;

        this.levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        //play level up animation ( can be same as victory animation (KiNG pose) )
        //spawn particle effect
        //flash for a second
    }
}
