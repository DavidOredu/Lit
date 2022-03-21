using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SensorTest : MonoBehaviour
{
    public AISensor boxSensor;
    public AISensor groundSensor;
    public Transform boxCheck;
    public LayerMask whatIsBox;
    public LayerMask whatIsGround;
    public Vector2 sensorDirection = Vector2.down;
    public float sensorDistance;
    public float boxCheckRadius;
    public AISensor.SensorType sensorType = AISensor.SensorType.Radial;

    public bool detectAll;
    // Start is called before the first frame update
    void Start()
    {
        boxSensor = new AISensor(boxCheck, whatIsBox, sensorType,sensorRadius:boxCheckRadius, detectAll: detectAll);
        groundSensor = new AISensor(boxCheck, whatIsGround, AISensor.SensorType.Linear, sensorDistance, sensorDirection);
    }
    public void DetectGround()
    {
        groundSensor.Detect();
        Debug.Log($"{groundSensor.newDetection.collider.name} was the box found.");
    }
    public void DetectBox()
    {
        boxSensor.Detect();
        if (!detectAll)
            Debug.Log($"{boxSensor.newDetection.name} was the box found.");


        else
        {
            Debug.Log($"{boxSensor.GetNewDetectionCount()} in the number of boxes found!");
            foreach (var box in boxSensor.newDetection)
            {
                Debug.Log($"{box.name} was found.");
            }
        }
        Debug.Log(boxSensor.CompareNewDetectionTag("Opponent"));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        boxSensor.GizmosDebug(sensorRadius:boxSensor.sensorRadius);
        groundSensor.GizmosDebug(groundSensor.sensorDirection, groundSensor.sensorDistance);
    }
}
