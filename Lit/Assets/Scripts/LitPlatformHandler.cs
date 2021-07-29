using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//ROLE: Used by the network player to call a function on a litplatform directly since the rpc doesn't work on the litplatform class for some reason. TODO: Look in the cause of the rpc not being called
public class LitPlatformHandler : NetworkBehaviour
{
    //reference to the stickman component of the network player the init the color state
    StickmanNet stickman;
    
    Racer player;

    Runner runner;

    public LitPlatformNetwork litPlatform { get; set; }

    private void Start()
    {
        stickman = GetComponent<StickmanNet>();
        player = GetComponent<Racer>();
        
        runner = new Runner(null, stickman, null, player);

       player.OnLitPlatformChanged += Instance_OnLitPlatformChanged;
    }

    private void Instance_OnLitPlatformChanged()
    {
        litPlatform = player.litPlatform;
        Debug.Log("Has run On lit platform changed logic");
    }

    private void FixedUpdate()
    {
            
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.collider.CompareTag("LitPlatform"))
        //{
        //    litPlatform = other.gameObject.GetComponent<LitPlatformNetwork>();
        //}
    }

    public void UpdateColor()
    {
        switch (player.currentRacerType)
        {
            case Racer.RacerType.Player:
                CmdUpdateColor();
                break;
            case Racer.RacerType.Opponent:
                OpponentUpdateColor();
                break;
            default:
                break;
        }
    }
    [Command]
    void CmdUpdateColor()
    {
        
       
            
            RpcUpdateColor();
        

    }
    [ClientRpc]
    void RpcUpdateColor()
    {


        litPlatform.glow.SetActive(true);
        litPlatform.bloom.SetActive(true);


        litPlatform.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");
        // material.SetColor("_EmissionColor", runner.spriteRendererOfPlayer.color);

        litPlatform.firstRunner = runner;
        //litPlatform.spriteRendererOfGlow.color = runner.spriteRendererOfPlayer.color;
        //litPlatform.spriteRendererOfGameObject.color = runner.spriteRendererOfPlayer.color;
        //litPlatform.spriteRendererOfBloom.color = runner.spriteRendererOfPlayer.color;
        litPlatform.spriteRendererOfGlow.material = litPlatform.material;
        litPlatform.spriteRendererOfGameObject.material = litPlatform.material;
        litPlatform.spriteRendererOfBloom.material = litPlatform.material;

        
        litPlatform.isLit = true;
    }
    void OpponentUpdateColor()
    {


        litPlatform.glow.SetActive(true);
        litPlatform.bloom.SetActive(true);


        litPlatform.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");
        // material.SetColor("_EmissionColor", runner.spriteRendererOfPlayer.color);

        litPlatform.firstRunner = runner;
        //litPlatform.spriteRendererOfGlow.color = runner.spriteRendererOfPlayer.color;
        //litPlatform.spriteRendererOfGameObject.color = runner.spriteRendererOfPlayer.color;
        //litPlatform.spriteRendererOfBloom.color = runner.spriteRendererOfPlayer.color;
        litPlatform.spriteRendererOfGlow.material = litPlatform.material;
        litPlatform.spriteRendererOfGameObject.material = litPlatform.material;
        litPlatform.spriteRendererOfBloom.material = litPlatform.material;


        litPlatform.isLit = true;
    }
}
