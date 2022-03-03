using System;
using UnityEngine;

public class TimerUtility : MonoBehaviour
{
    private float _incrementalTime = 0.0f;
    private TimeSpan _timeElapsed = new TimeSpan();
    private bool _startTimer = false;
    private TimeSpan _previousTime = new TimeSpan();
    private int _fiveSecondTimer = 0;

    public delegate void TimerEvent();
    public event TimerEvent OneSecond;
    public event TimerEvent FiveSeconds;


    private void Update()
    {
        if (!_startTimer)
            return;
        _incrementalTime += Time.deltaTime;
        _timeElapsed = TimeSpan.FromSeconds(_incrementalTime);

        
        if (Math.Abs((float)(_previousTime.Milliseconds - _timeElapsed.Milliseconds)) >= 499)
        {
            _previousTime = _timeElapsed;
            OneSecond?.Invoke();
            _fiveSecondTimer++;
        }

        if(_fiveSecondTimer == 5)
        {
            FiveSeconds?.Invoke();
            _fiveSecondTimer = 0;
        }
    }

    private void Start()
    {
        _startTimer = true;
    }


    public void ResetTimer()
    {
        _startTimer = false;
        _incrementalTime = 0.0f;        
    }

    public void StartTimer()
    {
        _startTimer = true;
    }

    public void StopTimer()
    {
        _startTimer = false;
    }
}