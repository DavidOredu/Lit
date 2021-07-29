using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Timer
{
    public bool timerStarted = false;
    public bool isTimeUp = false;
    bool pause = false;

    float _time;
    float time;
    // Initialize the timer. This is the constructor, obviously
    public Timer(float time)
    {
        this.time = time;
    }
    // This sets the timer to get ready. NOTE: This must be used before using either the "StartTimer" or "UpdateTimer" functions or else they won't work.
    public void SetTimer()
    {
        _time = time;
        timerStarted = true;
    }
    // This will be used in the case of a timer NOT in any update function. For example, in the start of the game if you want a timer from then to begin this would be perfect.
    // The reason for this is because of the crash that usually happens when you stick a "while" loop in an update function. I'm sure you guys have been there once.
    //EDIT: My expectations were not met with this function. It seems even if it was put in the "start" function is still crashed unity with the "while" loop. I(we) set it as a private function so it cannot be used outside this class.
    private void StartTimer()
    {
        while (!(_time <= 0))
        {
            if (pause != true)
            {
                _time -= Time.deltaTime;
            }
        }
        isTimeUp = true;
        _time = 0f;
    }
    // This will be used in the case of a timer in any update function. For example, a timer that starts when you activate a powerup, or stun time.
    // If this is put in the start function the game won't crash BUT your timer won't go anywhere as it would last only one frame. Must be anti-climatic
    public void UpdateTimer()
    {
        if(!(_time <= 0))
        {
            if(pause != true)
            _time -= Time.deltaTime;
        }
        else
        {
            isTimeUp = true;
            _time = 0f;
        }
    }
    // This places a control over the timer's update. It simply pauses or plays the timer.
    public void PauseOrPlayTimer()
    {
        pause = !pause;
    }
    // This resets the timer to the value of the timer set in the constructor.
    public void ResetTimer()
    {
        if (timerStarted)
        {
            _time = time;
            isTimeUp = false;
            UpdateTimer();
        }
    }
    // This resets the time to a new time set in the arguments of this function.
    public void ResetTimer(float newTime)
    {
        if (timerStarted)
        {
            time = newTime;
            _time = time;
            isTimeUp = false;
            UpdateTimer();
        }
    }
    // Checks if the timer is still running.
    public bool IsTimerRunning()
    {
        if(isTimeUp || pause)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // Checks if the timer is paused
    public bool IsTimerPaused()
    {
        return pause;
    }
    // Checks the current time of the timer
    public float CurrentTime()
    {
        return _time;
    }
    public int CurrentTimeWhole()
    {
        return (int)_time;
    }
    public string CurrentTimeString(DecimalType decimalType)
    {
        switch (decimalType)
        {
            case DecimalType.Whole:
                return _time.ToString("0");
            case DecimalType.Decimal:
                return _time.ToString();
            default:
                return _time.ToString();
        }
    }
    // Checks the normalized value of the timer compared to the predetermined time. A value between 0 and 1.
    public float CurrentTimeNormalized()
    {
        var normalized = 1 - (_time / time);
        if (normalized >= 1)
            normalized = 1f;
        return normalized;
    }
    public enum DecimalType
    {
        Whole,
        Decimal
    }
}
