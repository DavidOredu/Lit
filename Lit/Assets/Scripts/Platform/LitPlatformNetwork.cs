using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class LitPlatformNetwork : NetworkBehaviour
{
    public float WaitToDetectPlatform;
    

    public ColorStateCode colorStateCode;
    public GameObject glow { get; private set; }
    public GameObject bloom { get; private set; }
    public SpriteRenderer spriteRendererOfGlow { get; set; }
    public SpriteRenderer spriteRendererOfBloom { get; set; }
    public SpriteRenderer spriteRendererOfGameObject { get; set; }

    private Runner runner;

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


    private void Awake()
    {
        isLit = false;
        changed = false;
        otherIsOnLit = false;
    }
    // Start is called before the first frame update

    void OnEnable()
    {
       

        glow = transform.Find("Glow").gameObject;
        bloom = transform.Find("Bloom").gameObject;
        spriteRendererOfGlow = glow.GetComponent<SpriteRenderer>();
        spriteRendererOfGameObject = gameObject.GetComponent<SpriteRenderer>();
        spriteRendererOfBloom = bloom.GetComponent<SpriteRenderer>();

        glow.SetActive(false);
        bloom.SetActive(false);


    }

    private void Update()
    { 
        if(runner == null) { return; }
        
    }

    private void FixedUpdate()
    {
        if (runner == null) { return; }

        CheckRunnerOnLit();

    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {


            //   networkIdentity.AssignClientAuthority(connectionToClient);
         //  networkIdentity.GetComponent<NetworkIdentity>().AssignClientAuthority(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);

            spriteRendererOfRunner = other.gameObject.GetComponentInChildren<SpriteRenderer>();
            
            player = other.gameObject.GetComponent<Racer>();
            stickman = other.gameObject.GetComponent<StickmanNet>();

            runner = new Runner(null, stickman, spriteRendererOfRunner, player);
            if (runner.stickmanNet.currentColor.colorID == 0)
            {
                changed = true;
            }
        }

    }
    //[Server]
    //void Check()
    //{
    //    EventStateChanged?.Invoke(runner);
    //    CmdUpdateColor();
    //}

    
    [Client]
    void UpdateColor()
    {
       
            if (runner.player.isStayingOnLit)
            {
            
                EventStateChanged?.Invoke(runner);
            //    RpcUpdateColor();
                
            }
        
    }
    
 

    void CheckRunnerOnLit()
    {

        if (runner != firstRunner && runner.player != null)
        {
            if (runner.player.isOnAnotherLit && runner.player.isStayingOnLit)
            {
                otherIsOnLit = true;
            }
            else
            {
                otherIsOnLit = false;
            }
        }


    }



}
