﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserInput
{
    None,
    Tap,
    Up,
    Down,
    Left,
    Right,
};


public class InputManager : Singleton<InputManager>
{
    private UserInput _currentInput = UserInput.None;

    [SerializeField] private float _swipeResist = 70f;
    private float _swipeAngle = 0f;
    private Vector2 _firstTouchPosition;
    private Vector2 _finalTouchPosition;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
    }

    private void Update()
    {
        SwipeInput();
    }

    private void FixedUpdate()
    {
        /*Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        Vector3 newMove = input * 200f * Time.fixedDeltaTime;
        CharacterManager.Instance.Player.Rigidbody.velocity = newMove;*/
    }

    private void SwipeInput()
    {
        if (GetMouseInput())
        {
            float touchDiff = Vector2.Distance(_finalTouchPosition, _firstTouchPosition);
            if (touchDiff >= _swipeResist)
                CalculateAngle();
            else
                EventManager.OnSwipeFail.Invoke();
        }
    }

    void CalculateAngle()
    {
        _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
        SwipeDir();
        EventManager.OnSwipeDetected.Invoke(_currentInput, true);
    }

    private bool GetMouseInput()
    {
        // Swipe/Click started
        if (Input.GetMouseButtonDown(0))
        {
            _currentInput = UserInput.None;
            _firstTouchPosition = (Vector2)Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _finalTouchPosition = (Vector2)Input.mousePosition;
            return true;
            // Swipe/Click finished
        }

        return false;
    }
    
    private void SwipeDir()
    {
        if (_swipeAngle > -45 && _swipeAngle <= 45)
        {
            //Right Swipe
            _currentInput = UserInput.Right;
        }
        else if (_swipeAngle > 45 && _swipeAngle <= 135)
        {
            //Up Swipe
            _currentInput = UserInput.Up;
        }
        else if (_swipeAngle > 135 || _swipeAngle <= -135)
        {
            //Left Swipe
            _currentInput = UserInput.Left;
        }
        else if (_swipeAngle <= -45 && _swipeAngle > -135)
        {
            //Down Swipe
            _currentInput = UserInput.Down;
        }
    }
}
