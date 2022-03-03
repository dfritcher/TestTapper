using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    #region Fields, Properties
    [SerializeField] Transform _kart = null;
    [SerializeField] Transform _frontWheel = null;
    [SerializeField] Transform _backWheel = null;
    [SerializeField] float _velocity = 0;
    [SerializeField] float _maxVelocity = 0;
    [SerializeField] float _acceleration = 0.0f;
    [SerializeField] float _accelerationRate = 0.0f;
    [SerializeField] Animator _animator = null;
    [SerializeField] float _tireSpinTime = 0.0f;
    [SerializeField] float _tireSpinTimeMax = 0.0f;
    [SerializeField] bool _tireSpinning = false;
    #endregion Fields, Properties (end)

    #region Methods
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("SpinTire", true);
            _tireSpinning = true;
        }
        if (_tireSpinning)
        {
            _tireSpinTime += Time.deltaTime;
        }
        if(_tireSpinTime >= _tireSpinTimeMax)
        {
            _animator.SetBool("SpinTire", false);
            _tireSpinning = false;
            _tireSpinTime = 0.0f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _velocity += _accelerationRate;
            _animator.SetFloat("Velocity", _velocity);
            
        }
        
        if(_velocity > _maxVelocity)
            _velocity = _maxVelocity;
        HandleVelocity();
    }


    private void HandleVelocity()
    {
        _kart.Translate(Vector3.right * _velocity / 1000);
        _backWheel.Rotate(Vector3.forward * -1 * _velocity);
        _frontWheel.Rotate(Vector3.forward * -1 * _velocity);
    }
    #endregion Methods (end)
}
