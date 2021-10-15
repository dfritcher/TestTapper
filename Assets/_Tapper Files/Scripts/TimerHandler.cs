using System;
using UnityEngine;
using TMPro;

public class TimerHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerDisplay = null;
    [SerializeField] private TextMeshProUGUI _tapCountDisplay = null;
    private float _timer = 0.0f;
    private bool _startTimer = false;


    private void Update()
    {
        if (_startTimer)
        {
            _timer += Time.deltaTime;

            _timerDisplay.text = ((float)Math.Round(_timer * 100f) / 100f).ToString();
        }        
    }

    public void UpdateTapCountDisplay(int tapCount)
    {
        _tapCountDisplay.text = tapCount.ToString();
    }

    public void ResetTimer()
    {
        _startTimer = false;
        _timer = 0.0f;
        _timerDisplay.text = string.Empty;
        _tapCountDisplay.text = string.Empty;
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
