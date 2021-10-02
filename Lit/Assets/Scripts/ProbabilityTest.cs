using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityTest : MonoBehaviour
{
    public GameObject racerObj;
    public Probability<int> racerSpawnProbability;
    public List<int> colorCodes = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
    public AnimationCurve probabilityCurve;
    public RangeInt range;

    // Start is called before the first frame update
    void Start()
    {
        racerSpawnProbability = new Probability<int>(probabilityCurve);
        racerSpawnProbability.InitDictionary(colorCodes);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var colorCode = racerSpawnProbability.ProbabilityGenerator();
            var racerInGame = Instantiate(racerObj);
            var racer = racerInGame.GetComponent<Racer>();
            racer.runner.stickmanNet.dynamicUpdate = true;
            racer.runner.stickmanNet.code = colorCode;
            Debug.Log(racerSpawnProbability.maximumProbabilityRange);
            Debug.Log($"Color code is: {colorCode}");
        }
    }
}
