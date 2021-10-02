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

    private RunnerDamagesOperator runnerDamages;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public LayerMask whatToHit;
    public Vector3 startVFXOffset;
    public Vector3 extensionSpeed;
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    public float growthSpeed = 100f;

    public float damageStrength;

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

        beamEnd.SetActive(false);

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
        UpdateLaser(transform.position);
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
        beamStart.transform.position = start - startVFXOffset;

        Vector2 direction = Vector2.down * -(line.GetPosition(1) - line.GetPosition(0));
        Vector2 end = Vector3.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, whatToHit);



        Debug.DrawRay(transform.position, direction, Color.green);
        if (hit)
        {
            end = hit.point;

            if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("Opponent")))
            {
                runnerDamages.Damages[damageType].damaged = true;
                runnerDamages.Damages[damageType].damageInt = damageType;
                runnerDamages.Damages[damageType].damageStrength = damageStrength;
                runnerDamages.Damages[damageType].racer = ownerRacer;
                hit.collider.transform.SendMessage("DamageRunner", runnerDamages);
            }
        }
        else
        {
            end = line.GetPosition(1) + new Vector3(extensionSpeed.x, extensionSpeed.y, extensionSpeed.z);
        }

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        if (hit)
            beamEnd.SetActive(true);
        else
            beamEnd.SetActive(false);
        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
