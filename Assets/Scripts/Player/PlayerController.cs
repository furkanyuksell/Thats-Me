using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float _walkSpeed = 2f;
    [SerializeField] float _runSpeed = 6f;
    [SerializeField] float _jump = 1f;
    [SerializeField] float _gravity = -12f;
    [Range(0, 1)]
    [SerializeField] float _airControlPercent;

    [SerializeField] float _turnSmoothTime = 0.2f;
    float _turnSmoothVelocity;

    [SerializeField] float _speedSmoothTime = 0.1f;
    float _speedSmoothVelocity;
    float _currentSpeed;
    float _velocityY;

    Animator _animator;

    Transform _cameraT;

    CharacterController _controller;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _cameraT = Camera.main.transform;
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        
        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        float animationSpeedPercent = ((running) ? _currentSpeed / _runSpeed : _currentSpeed / _walkSpeed * .5f);
        _animator.SetFloat("speedPercent", animationSpeedPercent, _speedSmoothTime, Time.deltaTime);
    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, GetModifiedSmoothTime(_turnSmoothTime));
        }

        
        float targetSpeed = ((running) ? _runSpeed : _walkSpeed) * inputDir.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, GetModifiedSmoothTime(_speedSmoothTime));

        _velocityY += Time.deltaTime * _gravity;
        Vector3 velocity = transform.forward * _currentSpeed + Vector3.up * _velocityY;
        _controller.Move(velocity * Time.deltaTime);

        _currentSpeed = new Vector2(_controller.velocity.x, _controller.velocity.z).magnitude;

        if (_controller.isGrounded)
        {
            _velocityY = 0;
        }

    }


    void Jump()
    {
        if (_controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * _gravity * _jump);
            _velocityY = jumpVelocity;
            _animator.SetTrigger("Jump");
            _animator.SetFloat("Gravity", jumpVelocity/12);
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (_controller.isGrounded)
        {
            return smoothTime;
        }

        if (_airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / _airControlPercent;
    }

}
