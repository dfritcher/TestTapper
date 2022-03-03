using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ShooterManager : MonoBehaviour
{

    [SerializeField] private GameObject _p1BallPrefab = null;
    [SerializeField] private GameObject _p2BallPrefab = null;
    [SerializeField] private Transform _ballParent = null;
    [SerializeField] private int _maxBallCount = 30;
    [SerializeField] private int _startBallCount = 5;
    [SerializeField] private int _currentBallCount = 0;
    [SerializeField] private Transform _spawnPointOne;
    [SerializeField] private Transform _spawnPointTwo;
    [SerializeField] private Goal[] _goals;
    [SerializeField] private PlayerHud[] _playerHuds;

    private int _player1Score = 0;
    private int _player2Score = 0;

    private List<Ball> _p1BallPool = new List<Ball>();
    private List<Ball> _p2BallPool = new List<Ball>();

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate a few Ball instances 
        for (var i = 0; i < _startBallCount; i++)
        {
            InstantiatePlayerOneBall();
            InstantiatePlayerTwoBall();
        }
        for(var i = 0; i < _goals.Length; i++)
        {
            _goals[i].GoalOccurred += GoalEvent_Score;
        }               
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayerOneButtonClicked();
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            OnPlayerTwoButtonClicked();
        }
    }

    public void OnPlayerOneButtonClicked()
    {
        StartCoroutine(SpawnBall(1));
    }

    public void OnPlayerTwoButtonClicked()
    {
        StartCoroutine(SpawnBall(2));
    }

    public void BallEvent_OnActivate(Ball ball)
    {
        _currentBallCount++;
    }

    public void BallEvent_OnDeActivate(Ball ball)
    {
        _currentBallCount--;
    }

    private IEnumerator SpawnBall(int player)
    {
        yield return new WaitForSeconds(.1f);
        if (_currentBallCount < _maxBallCount)
        {
            switch (player)
            {
                case 1:
                    var p1Ball = GetPlayerOneBallInstanceFromPool();
                    p1Ball.transform.position = _spawnPointOne.position;
                    p1Ball.Activate();
                    break;
                case 2:
                    var p2Ball = GetPlayerTwoBallInstanceFromPool();
                    p2Ball.transform.position = _spawnPointTwo.position;
                    p2Ball.Activate();
                    break;
            }            
        }
    }

    private Ball GetPlayerOneBallInstanceFromPool()
    {
        if (_p1BallPool.Any(b => !b.gameObject.activeInHierarchy))
        {
            return _p1BallPool.First(b => !b.gameObject.activeInHierarchy);
        }
        
        return InstantiatePlayerOneBall();
    }

    private Ball GetPlayerTwoBallInstanceFromPool()
    {
        if (_p2BallPool.Any(b => !b.gameObject.activeInHierarchy))
        {
            return _p2BallPool.First(b => !b.gameObject.activeInHierarchy);
        }

        return InstantiatePlayerTwoBall();
    }

    private void UpdateScoreDisplay()
    {
        for(var i = 0; i < _playerHuds.Length; i++)
        {
            _playerHuds[i].UpdateScoreDisplay(_player1Score, _player2Score);
        }
    }

    private Ball InstantiatePlayerOneBall()
    {
        var newBall = Instantiate(_p1BallPrefab, _ballParent, false).GetComponent<Ball>();
        newBall.ActivateEvent += BallEvent_OnActivate;
        newBall.DeactivateEvent += BallEvent_OnDeActivate;
        newBall.gameObject.SetActive(false);
        _p1BallPool.Add(newBall);
        return newBall;
    }

    private Ball InstantiatePlayerTwoBall()
    {
        var newBall = Instantiate(_p2BallPrefab, _ballParent, false).GetComponent<Ball>();
        newBall.ActivateEvent += BallEvent_OnActivate;
        newBall.DeactivateEvent += BallEvent_OnDeActivate;
        newBall.gameObject.SetActive(false);
        _p2BallPool.Add(newBall);
        return newBall;
    }

    private void GoalEvent_Score(Goal goal, Ball ball)
    {
        Debug.Log($"Goal Event: Goal - {goal.PlayerNumber}, Ball - {ball.PlayerNumber}");
        switch (goal.PlayerNumber)
        {
            case 1:
                if(ball.PlayerNumber == 2)
                    _player2Score += ball.GetScore();
                break;
            case 2:
                if(ball.PlayerNumber == 1)
                    _player1Score += ball.GetScore();
                break;
        }
        _currentBallCount--;
        UpdateScoreDisplay();
    }
}
