using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class LitPlatform : LitPlatformNetwork
{
   

    private Opponent opponent;
    private Stickman stickman;
    private SpriteRenderer spriteRendererOfRunner;

    AnimationCurve curve;
    
    private void Awake()
    {
       
    }
    // Start is called before the first frame update

    void OnEnable()
    {
        
    }

    private void Update()
    { 
        
    }

    private void FixedUpdate()
    {
        
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        {


            //   networkIdentity.AssignClientAuthority(connectionToClient);
         //  networkIdentity.GetComponent<NetworkIdentity>().AssignClientAuthority(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);

            spriteRendererOfRunner = other.gameObject.GetComponentInChildren<SpriteRenderer>();

            //if (other.gameObject.GetComponent<Player>())
            //{
            //    player = other.gameObject.GetComponent<Player>();
            //}
            //else if (other.gameObject.GetComponent<Opponent>())
            //{
            //    opponent = other.gameObject.GetComponent<Opponent>();
            //}
            //if(other.collider.gameObject.GetComponent<Player>())
            //stickman = other.gameObject.GetComponent<Stickman>();

            //runner = new Runner(stickman, null, spriteRendererOfRunner, player, null, opponent);
            //if(runner.stickman.currentColor.colorID == 0)
            //{
            //    changed = true;
            //}

        }

    }
    //[Server]
    //void Check()
    //{
    //    EventStateChanged?.Invoke(runner);
    //    CmdUpdateColor();
    //}


}
