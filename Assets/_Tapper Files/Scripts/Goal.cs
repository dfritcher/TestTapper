using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int _playerNumber;
    public int PlayerNumber { get { return _playerNumber; } }
    
    public delegate void GoalEvent(Goal goal, Ball ball);
    public event GoalEvent GoalOccurred;
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        GoalOccurred?.Invoke(this, ball);
        ball.Deactivate();
    }

}
