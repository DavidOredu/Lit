using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class BeamProjectileScript : MonoBehaviour
{
    public Racer ownerRacer;
    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;
    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;

    public int currentBeam = 0;
    public int damageType;
    public BeamType beamType = BeamType.BeamPowerup;

    private RunnerDamagesOperator runnerDamages;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public BeamExtensionType extensionType;
    public LayerMask whatToHit;
    public Vector3 startVFXOffset;
    public Vector3 extensionSpeed;
    public Vector2 directionToHit = Vector2.down;
    public float extensionLimit;
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    public float damagePercentage;
    public float damageRate;
    public bool canExtend = true;

    [Header("Put Sliders here (Optional)")]
    public Slider endOffSetSlider; //Use UpdateEndOffset function on slider
    public Slider scrollSpeedSlider; //Use UpdateScrollSpeed function on slider

    [Header("Put UI Text object here to show beam name")]
    public Text textBeamName;

    // Use this for initialization
    void Start()
    {
        if (textBeamName)
            textBeamName.text = beamLineRendererPrefab[currentBeam].name;
        if (endOffSetSlider)
            endOffSetSlider.value = beamEndOffset;
        if (scrollSpeedSlider)
            scrollSpeedSlider.value = textureScrollSpeed;

        beamStart = Instantiate(beamStartPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        beamEnd = Instantiate(beamEndPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        beam = Instantiate(beamLineRendererPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        line = beam.GetComponent<LineRenderer>();

        beamEnd.SetActive(true);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        beamStart.transform.position = transform.position - startVFXOffset;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, line.GetPosition(0));

        runnerDamages.InitDamages();
    }
    void DestroyBeam()
    {
        Destroy(beamStart);
        Destroy(beamEnd);
        Destroy(beam);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (canExtend)
            UpdateLaser(transform.position);
        else
            StopLaser(transform.position);
    }

    public void UpdateEndOffset()
    {
        beamEndOffset = endOffSetSlider.value;
    }

    public void UpdateScrollSpeed()
    {
        textureScrollSpeed = scrollSpeedSlider.value;
    }

    void UpdateLaser(Vector3 start)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        

        Vector3 direction = directionToHit/* * -(line.GetPosition(1) - line.GetPosition(0))*/;
        float distance = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
        Vector2 end = Vector3.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, whatToHit);


        if (hit)
        {
            end = hit.point;

            if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("Opponent")))
            {
                Utils.SetDamageVariables(runnerDamages, ownerRacer, damageType, damagePercentage, damageRate, hit.collider.gameObject);
            }
            if(beamType == BeamType.ElectricOrb)
                Destroy(gameObject);
            beamEnd.SetActive(true);
            Debug.DrawLine(transform.position, end, Color.green);

        }
        else
        {
            if (extensionType == BeamExtensionType.Fixed)
            {
                end = line.GetPosition(0) + direction * extensionLimit;
            }
            else
            {
                var newExtension = extensionSpeed;
                newExtension.Scale(direction);
                end = line.GetPosition(1) + newExtension;
                
            }
            Debug.DrawLine(transform.position, end, Color.green);
        }
        line.SetPosition(1, end);
        beamEnd.SetActive(true);
        beamEnd.transform.position = end;

        float dist = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(dist / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
    void StopLaser(Vector3 start)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start - startVFXOffset;

        line.SetPosition(1, start);
        beamEnd.SetActive(false);
    }
    public enum BeamType
    {
        ElectricOrb, 
        BeamPowerup
    }
    public enum BeamExtensionType
    {
        [Tooltip("Is the beam the shoot to infinity until it detects an object?")]
        Infinite,
        [Tooltip("Is the beam to only shoot a fixed, defined distance?")]
        Fixed, 
    }
}
