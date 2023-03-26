using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] bool _lockCursor;
    [SerializeField] float _mouseSensitivity = 10f;
    [SerializeField] Transform _target;
    [SerializeField] float _distanceFromTarget = 2f;
    [SerializeField] Vector2 _pitchMinMax = new Vector2(-40, 85);

    [SerializeField] float _rotationSmoothTime = .12f;
    Vector3 _rotationSmoothVelocity;
    Vector3 _currentRotation;

    float _yaw;
    float _pitch;


    void Start()
    {
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void LateUpdate()
    {
        _yaw += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, _pitchMinMax.x, _pitchMinMax.y);
    
        _currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationSmoothVelocity, _rotationSmoothTime);
        transform.eulerAngles = _currentRotation;

        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }
}
