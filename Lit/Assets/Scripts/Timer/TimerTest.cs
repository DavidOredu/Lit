using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    public Timer testTimer;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        testTimer = new Timer(time);
        testTimer.SetTimer();
        Debug.Log("Timer is set!");
        
    }

    // Update is called once per frame
    void Update()
    {
        testTimer.UpdateTimer();
        Debug.Log("Timer has started!");
        if (testTimer.isTimeUp)
        {
            Debug.Log("Timer has ended!");
        }
        else
        {
            Debug.Log($"The current time is: {testTimer.CurrentTimeString(Timer.DecimalType.Whole)}");
        }
    }
    public void PauseOrPlay()
    {
        testTimer.PauseOrPlayTimer();
        Debug.Log("Timer is paused!");
        Debug.Log($"The current time is: {testTimer.CurrentTimeNormalized()}");
    }
}
