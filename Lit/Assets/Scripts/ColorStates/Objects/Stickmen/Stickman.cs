using SpriteGlow;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Stickman : MonoBehaviour
{
    public List<SpriteRenderer> stickmanColorRenderers { get; private set; } = new List<SpriteRenderer>();
    public List<SpriteRenderer> auraRenderers { get; private set; } = new List<SpriteRenderer>();

    
    private Material material;
    private Material auraMaterial;

    public ColorStateCode blackColor { get; private set; }
    public ColorStateCode redColor { get; private set; }
    public ColorStateCode blueColor { get; private set; }
    public ColorStateCode greenColor { get; private set; }
    public ColorStateCode yellowColor { get; private set; }
    public ColorStateCode purpleColor { get; private set; }
    public ColorStateCode cyanColor { get; private set; }
    public ColorStateCode magentaColor { get; private set; }
    public ColorStateCode orangeColor { get; private set; }
    public ColorStateCode yellowGreenColor { get; private set; }
    public ColorStateCode whiteColor { get; private set; }

    public List<ColorStateCode> colorStateCodes { get; private set; } = new List<ColorStateCode>();

    private PlayerData playerData;
    private ColorData colorData;

    public int code = 10;
    public bool dynamicUpdate;
    private NetworkState networkState;
    private Player playerNN;
    public ColorStateCode currentColor { get; private set; }
    private GameManager gameManager;
  ///  private StickmenNet stickmenNet;

    private void Awake()
    {
        
        if (playerData == null)
            playerData = Resources.Load<PlayerData>("PlayerData");
        if (colorData == null)
            colorData = Resources.Load<ColorData>("ColorData");
        var renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
        {
            if (renderer.name.Contains("_Color"))
            {
                stickmanColorRenderers.Add(renderer);
            }
            else if (renderer.name.Contains("_Aura"))
            {
                auraRenderers.Add(renderer);
            }
        } 
    
        networkState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<NetworkState>();
        

        blackColor = new ColorStateCode(0, colorData.black, false);
        redColor = new ColorStateCode(1, colorData.red, false);
        blueColor = new ColorStateCode(2, colorData.blue, false);
        greenColor = new ColorStateCode(3, colorData.green, false);
        yellowColor = new ColorStateCode(4, colorData.yellow, false);
        purpleColor = new ColorStateCode(5, colorData.purple, false);
        cyanColor = new ColorStateCode(6, colorData.cyan, false);
        magentaColor = new ColorStateCode(7, colorData.pink, false);
        orangeColor = new ColorStateCode(8, colorData.orange, false);
        yellowGreenColor = new ColorStateCode(9, colorData.yellowGreen, false);
        whiteColor = new ColorStateCode(10, Color.white, false);

        colorStateCodes.Add(blackColor);
        colorStateCodes.Add(redColor);
        colorStateCodes.Add(blueColor);
        colorStateCodes.Add(greenColor);
        colorStateCodes.Add(yellowColor);
        colorStateCodes.Add(purpleColor);
        colorStateCodes.Add(cyanColor);
        colorStateCodes.Add(magentaColor);
        colorStateCodes.Add(orangeColor);
        colorStateCodes.Add(yellowGreenColor);
        colorStateCodes.Add(whiteColor);



    }
    // Start is called before the first frame update
    private void Start()
    {
       

    }

    // Update is called once per frame
    private void Update()
    {
        switch (networkState.currentNetworkState)
        {
            case NetworkState.State.Network:
                if (playerNN == null)
                    playerNN = GetComponent<Player>();
                if (gameManager == null)
                    gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                if (gameManager.currentGameState == GameManager.GameState.InGame)
                {
                    code = playerData.colorCode;
                    DefineCode();
                }

                break;
            case NetworkState.State.NonNetwork:
                if (playerNN == null)
                    playerNN = GetComponent<Player>();
                if (gameManager == null)
                    gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                if (gameManager.currentGameState == GameManager.GameState.InGame)
                {
                    code = playerData.colorCode;
                    DefineCode();
                }
                break;
            case NetworkState.State.None:
                if (playerNN == null)
                    playerNN = GetComponent<Player>();
                if (gameManager == null)
                    gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                {
                    code = playerData.colorCode;
                    DefineCode();
                }

                break;
            default:
                break;
        }

        if (dynamicUpdate == true)
            DefineCode();
    }

    void DefineCode()
    {

        foreach (ColorStateCode colorStateCode in colorStateCodes)
        {
            if (code == colorStateCode.colorID)
            {
                try
                {
                    colorStateCode.isPicked = true;
                    currentColor = colorStateCode;

                    material = Resources.Load<Material>($"{currentColor.colorID}");
                    auraMaterial = Resources.Load<Material>("1/FlameMat");

                    foreach (SpriteRenderer renderer in stickmanColorRenderers)
                    {
                   //     renderer.material = material;
                        renderer.color = currentColor.color;
                    }
                    foreach (SpriteRenderer renderer in auraRenderers)
                    {
                        renderer.material = auraMaterial;
                        renderer.color = currentColor.color;
                        renderer.gameObject.SetActive(false);
                    }
                    return;
                }
                catch
                {
                    Debug.LogError("Code does not not correspond to any Color State Code!");
                }
               
            }
        }
    }
}
