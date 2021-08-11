using Mirror;
using UnityEngine;

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
        litPlatform.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");
        litPlatform.runner = runner;
        litPlatform.firstRunner = runner;
        litPlatform.spriteShapeRenderer.color = stickman.currentColor.color;

        for (int i = 0; i < litPlatform.spriteShapeRenderer.materials.Length; i++)
        {
            litPlatform.spriteShapeRenderer.materials[i] = litPlatform.material;
        }

        litPlatform.isLit = true;
    }
    void OpponentUpdateColor()
    {


        litPlatform.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");

        litPlatform.runner = runner;
        litPlatform.firstRunner = runner;
        litPlatform.spriteShapeRenderer.color = stickman.currentColor.color;

        for (int i = 0; i < litPlatform.spriteShapeRenderer.materials.Length; i++)
        {
            litPlatform.spriteShapeRenderer.materials[i] = litPlatform.material;
            //       litPlatform.spriteShapeRenderer.sharedMaterials[i] = litPlatform.material;
        }


        litPlatform.isLit = true;
    }
}
