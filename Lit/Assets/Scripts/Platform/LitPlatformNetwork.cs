using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(EdgeCollider2D))]
public class LitPlatformNetwork : NetworkBehaviour
{
    public Platform.PlatformType platformType = Platform.PlatformType.LitPlatform;

    public float WaitToDetectPlatform;
    
    public ColorStateCode colorStateCode;
    public GameObject glow { get; private set; }
    public GameObject bloom { get; private set; }
    public SpriteRenderer spriteRendererOfGlow { get; set; }
    public SpriteRenderer spriteRendererOfBloom { get; set; }
    public SpriteRenderer spriteRendererOfGameObject { get; set; }

    public List<Runner> runners { get; set; } = new List<Runner>();
    public SpriteShapeRenderer spriteShapeRenderer { get; set; }

    public Runner firstRunner { get; set; }
    
    public delegate void StateChangedDelegate(Runner runner);

    public event StateChangedDelegate EventStateChanged;
    
    public Material material { get; set; }

    public bool otherIsOnLit { get; private set; }

    public bool isLit { get; set; }
    public bool changed { get; set; }

   
    private Racer player;
    private StickmanNet stickman;
    private SpriteRenderer spriteRendererOfRunner;
    Rigidbody2D rb;

    private void Awake()
    {
        isLit = false;
        changed = false;
        otherIsOnLit = false;
    }
    // Start is called before the first frame update

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();

        rb.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    { 
        if(runners.Count == 0) { return; }
        
    }

    private void FixedUpdate()
    {
        CheckRunnerOnLit();
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other .collider.CompareTag("Opponent"))
        {


            //   networkIdentity.AssignClientAuthority(connectionToClient);
         //  networkIdentity.GetComponent<NetworkIdentity>().AssignClientAuthority(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);

            spriteRendererOfRunner = other.gameObject.GetComponentInChildren<SpriteRenderer>();
            
            player = other.gameObject.GetComponent<Racer>();
            stickman = other.gameObject.GetComponent<StickmanNet>();

            if (player.runner != firstRunner && !runners.Contains(player.runner))
                runners.Add(player.runner);
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        {
            player = other.gameObject.GetComponent<Racer>();
            stickman = other.gameObject.GetComponent<StickmanNet>();

            if (runners.Contains(player.runner))
                runners.Remove(player.runner);
        }
    }
    void CheckRunnerOnLit()
    {
        if(runners.Count == 0)
        {
            otherIsOnLit = false;
            return;
        }

        foreach (var runner in runners)
        {
            if (runner != firstRunner && runner.player != null)
            {
                if (runner.player.isOnAnotherLit && runner.player.litPlatform == this)
                {
                    otherIsOnLit = true;
                    return;
                }
            }
        }
        otherIsOnLit = false;
    }



}
