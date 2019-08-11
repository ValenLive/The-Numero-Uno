﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float _speed = 25;

    [Header("Jump")]
    [SerializeField]
    private float _jumpHeight = 25;

    [SerializeField]
    private float _fallGravityScale = 5;

    [Header("Air Control")]

    [SerializeField]
    private float coyoteTimeMax = 0.1f;
    [SerializeField]
    private float verticalMultipl = 0.9f;
    [SerializeField]
    [Tooltip("Deccelerate when in air you change velocity X direction.")]
    private float verticalSwithMultipl = 0.8f;


    [Space]

    private Rigidbody2D _rb;

    private Ground _groundDetection;

    private float _rbGravity, _coyoteTime;

    private float _moveX, _moveY;

    private bool _isFirstV = true, _isFirstH = true;

    float startVelocity = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundDetection = GetComponent<Ground>();

        _rbGravity = _rb.gravityScale;

        _coyoteTime = coyoteTimeMax;

    }


    private void Update()
    {
        CalculateMovement();
        _rb.velocity = new Vector2(_moveX * _speed, _moveY);
    }


    private void CalculateMovement()
    {
        //get input
        _moveX = Input.GetAxis("Horizontal");
        _moveY = _rb.velocity.y;

        CalculateHorizontalMovement();
        CalculateVertialMovement();     
    }


    private void CalculateHorizontalMovement()
    {
        bool isGrounded = _groundDetection.IsGrounded();


        if (!isGrounded)
        {
            //decrease X velocity while jumping
            _moveX *= verticalMultipl;

            //get begin X velocity
            if (_isFirstH)
            {
                startVelocity = _rb.velocity.x;
                _isFirstH = false;
            }

            if (startVelocity < 0)
            {
                if (_rb.velocity.x > 0)
                    _moveX *= verticalSwithMultipl;
            }
            else if (startVelocity > 0)
            {
                if (_rb.velocity.x < 0)
                    _moveX *= verticalSwithMultipl;
            }

        }
        else
        _isFirstH = true;
    }


    private void CalculateVertialMovement()
    {
        bool isGrounded = _groundDetection.IsGrounded();

        //callculate coyote Time
        if (!isGrounded)
        {
            _coyoteTime -= Time.deltaTime;
        }
        else
        {
            _coyoteTime = coyoteTimeMax;
        }

        //jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                _isFirstV = true;
            }
            //coyote jump
            else if (_coyoteTime >= 0 && _moveY < 0 && _isFirstV)
            {
                Jump();
            }

            //relising button during jumping
        }
        else if (Input.GetButtonUp("Jump") && _moveY > 8 && _isFirstV)
        {
            _moveY = _jumpHeight / 2.5f;
            _isFirstV = false;
        }

        //increase gravity if player is falling
        if (!isGrounded && _moveY < 0)
        {
            if(_isFirstV)
            _rb.gravityScale *= _fallGravityScale;

            _isFirstV = false;
        }
        else
        {
            _rb.gravityScale = _rbGravity;
            _isFirstV = true;
        }
    }


    private void Jump()
    {
        _moveY = _jumpHeight;
    }
}