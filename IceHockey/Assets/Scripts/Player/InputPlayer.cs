using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private enum States { Wait, Left, Right}
    private States state;

    private Rigidbody rigidbody;
    private Vector3 movement;
    private float speed = 2;

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
        if (state != States.Wait)
        {
            if (state == States.Left)
                MoveLeft();
            else
                MoveRight();

            Move();
        }
    }

    private void MoveLeft()
    {
        movement = -Vector3.right * Time.fixedDeltaTime * speed;
    }

    private void MoveRight()
    {
        movement = Vector3.right * Time.fixedDeltaTime * speed;
    }

    private void Move()
    {
        Vector3 pos = transform.position + movement;
        pos.x = Mathf.Clamp(pos.x, -3.6f, 3.6f);
        rigidbody.MovePosition(pos);
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
