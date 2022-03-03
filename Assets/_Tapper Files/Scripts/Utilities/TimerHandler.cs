using System;
using UnityEngine;
using TMPro;

public class TimerHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerDisplay = null;
    [SerializeField] private Transform _timerHand = null;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _clockTick = null;
    private float _incrementalTime = 0.0f;
    private TimeSpan _timeElapsed = new TimeSpan();
    private bool _startTimer = false;
    private TimeSpan _previousTime = new TimeSpan();
    private bool _isAudioPlaying = false;

    private void Update()
    {
        if (_startTimer)
        {
            _incrementalTime += Time.deltaTime;
            _timeElapsed = TimeSpan.FromSeconds(_incrementalTime);


            //When a half second passes tick the clock
            if (Math.Abs((float)(_previousTime.Milliseconds - _timeElapsed.Milliseconds)) > 200)
            {
                _timerHand.Rotate(new Vector3(0, 0, -10f));
                _previousTime = _timeElapsed;
                
            }
            _timerDisplay.text = _timeElapsed.ToString(@"ss\:ff");
            if (!_isAudioPlaying)
            {
                _audioSource.Play();
                _isAudioPlaying = true;
            }                            
        }
        else
        {
            _audioSource.Stop();
            _isAudioPlaying = false;
        }
    }

    public string GetTimeDisplay()
    {
        return _timerDisplay.text;
    }

    public TimeSpan GetTime()
    {
        return _timeElapsed;
    }

    public void ResetTimer()
    {
        _startTimer = false;
        _incrementalTime = 0.0f;
        _timeElapsed = TimeSpan.Zero;
        _timerHand.Rotate(Vector3.zero);
        _timerDisplay.text = _timeElapsed.ToString(@"ss\:ff");
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
