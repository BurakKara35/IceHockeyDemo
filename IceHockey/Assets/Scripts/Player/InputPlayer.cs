using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : PlayerBase
{
    private float touchPosition_x;

    private enum InputStates { Up, Down}
    private InputStates inputState;
    private bool inputRemaining = false;

    private void OnEnable()
    {
        startingPosition = new Vector3(0, 0.2f, -8);
        transform.position = startingPosition;
    }

    private void Start()
    {
        inputState = InputStates.Up;
    }

    private void Update()
    {
        base.Update();
        InputHandler();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void InputHandler()
    {
        CheckInput();

        if (inputState == InputStates.Down)
        {
            touchPosition_x = GetMousePosition_x();

            if (inputRemaining)
            {
                if (touchPosition_x - transform.position.x < 0.1 && touchPosition_x - transform.position.x > -0.1) // for avoid jittering
                    inputRemaining = false;
                else if (touchPosition_x > transform.position.x)
                    state = States.Right;
                else if (touchPosition_x < transform.position.x)
                    state = States.Left;
            }
            else
                inputState = InputStates.Up;

        }
        else if (inputState == InputStates.Up)
        {
            state = States.Wait;
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputState = InputStates.Down;
            inputRemaining = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            inputState = InputStates.Up;
            inputRemaining = false;
        }
    }

    private float GetMousePosition_x()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
            return hit.point.x;
        else
            return transform.position.x;
    }
}
