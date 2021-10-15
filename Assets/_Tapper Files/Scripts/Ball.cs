using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform _transform = null;
    [SerializeField] private Rigidbody2D _rigidBody = null;
    [SerializeField] private float _initialSpeed = 100f;
    [SerializeField] private float _speedIncrement = .1f;
    [SerializeField] private float _maxSpeed = 5f;
    
    [SerializeField] private Vector3 _lastVelocity;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private Vector3 _force;
    [SerializeField] private int _scoreAmount = 5;
    [SerializeField] private int _playerNumber = 0;
    [SerializeField] private float _yDirection = 0f;
    public int PlayerNumber { get { return _playerNumber; } }

    private bool _increaseSpeed = true;

    public delegate void BallEvent(Ball ball);

    public event BallEvent ActivateEvent;
    public event BallEvent DeactivateEvent;

    private void Awake()
    {
        
    }
    private void Update()
    {
        _lastVelocity = _rigidBody.velocity;
        if(_lastVelocity == Vector3.zero)
            _rigidBody.AddForce(new Vector3(Random.Range(-10f, 10f), _yDirection, 0f), ForceMode2D.Impulse);
    }

    public void Activate()
    {
        ActivateEvent?.Invoke(this);
        gameObject.SetActive(true);
        _rigidBody.AddForce(new Vector3(Random.Range(-10f, 10f), _yDirection, 0f), ForceMode2D.Impulse);
    }


    public void Deactivate()
    {
        DeactivateEvent?.Invoke(this);
        _rigidBody.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_increaseSpeed)
        {
            return;
        }
        _currentSpeed = _lastVelocity.magnitude;
        var direction = Vector3.Reflect(_lastVelocity.normalized, collision.GetContact(0).normal);
        _rigidBody.velocity = direction * Mathf.Min(_currentSpeed + _speedIncrement, _maxSpeed);
        _increaseSpeed = false;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        _increaseSpeed = true;
    }

    internal int GetScore()
    {
        return _scoreAmount;
    }
}
