using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : PlayerBase
{
    private float touchPosition_x;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputHandler();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void InputHandler()
    {
        if (Input.GetMouseButton(0))
        {
            touchPosition_x = GetMousePosition_x();

            if (touchPosition_x - transform.position.x > 0.1f) // for avoid jittering
                state = States.Right;
            else if (touchPosition_x < transform.position.x)
                state = States.Left;
            else
                state = States.Wait;
        }

        if (Input.GetMouseButtonUp(0))
        {
            state = States.Wait;
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
