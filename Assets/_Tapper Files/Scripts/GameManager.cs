using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainScreen = null;
    [SerializeField] private GameObject _endScreen = null;
    [SerializeField] private GameObject _readyPanel = null;
    [SerializeField] private TextMeshProUGUI _readyDisplay = null;
    [SerializeField] private TextMeshProUGUI _goDisplay = null;
    [SerializeField] private TimerHandler _timerHandler = null;
    [SerializeField] private TapHandler _tapHandler = null;
    [SerializeField] private TextMeshProUGUI _balloonState = null;
    [SerializeField] private SpriteRenderer _balloon = null;
    [SerializeField] private Color _orange;

    
    private float _previousPressedTime = 0f;
    private float _timer = 0;
    private WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
    private bool _roundEnd = false;

    private void Start()
    {
        _mainScreen.SetActive(true);
        _readyPanel.SetActive(true);
        _readyDisplay.text = string.Empty;
        _goDisplay.text = string.Empty;
        _balloonState.text = string.Empty;
    }

    private void Update()
    {
        if(_balloon.transform.localScale.x >= 7f)
        {
            _roundEnd = true;
        }

        if (_roundEnd)
        {
            _balloon.gameObject.SetActive(false);
            _balloonState.text = "POP!";
            _timerHandler.StopTimer();
            _tapHandler.SetButtonState(false);
        }
        _timer += Time.deltaTime;
    }

    public void PlayClicked()
    {
        _mainScreen.SetActive(false);
        StartCoroutine(PlayClickedMainHandler());
    }

    private IEnumerator PlayClickedMainHandler()
    {
        yield return StartCoroutine(PlayClickedCoroutine());
        yield return StartCoroutine(StartGameCoroutine());
    }

    public void ResetClicked()
    {
        _readyDisplay.text = string.Empty;
        _goDisplay.text = string.Empty;
        _balloonState.text = string.Empty;
        _balloon.gameObject.SetActive(true);
        _balloon.transform.localScale = new Vector3(1f, 1f, 1f);
        _balloon.color = Color.white;
        _mainScreen.SetActive(true);
        _readyPanel.SetActive(true);
        _tapHandler.SetButtonState(true);
        _tapHandler.Reset();
        _timerHandler.ResetTimer();
        _roundEnd = false;
    }

    internal void UpdateTapCount(int tapCount)
    {
        var increase = (1/_timer)/100f;
        
        Debug.Log(string.Empty);
        Debug.Log($"<color=red>Pressed Difference: {_timer}</color>");
        Debug.Log($"<color=yellow>Increase: {increase}</color>");

        _balloon.transform.localScale = new Vector3(_balloon.transform.localScale.x + increase, _balloon.transform.localScale.y + increase, _balloon.transform.localScale.z);
        if(_balloon.transform.localScale.x <= 2f)
        {
            _balloon.color = Color.white;
        }
        else if (_balloon.transform.localScale.x <= 3f)
        {
            _balloon.color = Color.yellow;
        }
        else if (_balloon.transform.localScale.x <= 4f)
        {
            _balloon.color = _orange;
        }
        else if (_balloon.transform.localScale.x <= 5f)
        {
            _balloon.color = Color.red;
        }
        _timerHandler.UpdateTapCountDisplay(tapCount);
        _timer = 0f;
    }


    private IEnumerator PlayClickedCoroutine()
    {
        yield return new WaitForSeconds(.1f);

        _readyDisplay.text = "READY!";

        yield return _waitOneSecond;

        _readyDisplay.text = "<color=yellow>SET</color>";

        yield return _waitOneSecond;

        _readyDisplay.text = string.Empty;
        _goDisplay.text = "<color=green>TAP!</color>";
        yield return new WaitForSeconds(.5f);
        _readyPanel.SetActive(false);
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        _timerHandler.StartTimer();
    }
}
