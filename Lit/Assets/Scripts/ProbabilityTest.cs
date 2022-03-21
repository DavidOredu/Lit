using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityTest : MonoBehaviour
{
    public GameObject racerObj;
    public Probability<bool> racerSpawnProbability;
    public List<bool> colorCodes = new List<bool> ();
    public AnimationCurve probabilityCurve;
    public RangeInt range;

    public Vector2 force;

    // Start is called before the first frame update
    void Start()
    {
        racerSpawnProbability = new Probability<bool>(probabilityCurve, colorCodes);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var jump = racerSpawnProbability.ProbabilityGenerator();
            Debug.Log(jump);
            //   var racerInGame = Instantiate(racerObj);
            //   var racer = racerInGame.GetComponent<Racer>();
            //   racer.runner.stickmanNet.enabled = true;
            //   racer.runner.stickmanNet.dynamicUpdate = true;
            //   racer.runner.stickmanNet.code = colorCode;
            //   Debug.Log(racerSpawnProbability.maximumProbabilityRange);
            //   Debug.Log($"Color code is: {colorCode}");

            racerObj.SetActive(jump);
        }
    }
}
