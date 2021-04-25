using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    protected enum States { Wait, Left, Right }
    protected States state;

    protected Rigidbody rigidbody;
    protected Vector3 movement;
    protected float speed = 4;

    protected Vector3 startingPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        if (state != States.Wait)
        {
            if (state == States.Left)
                MoveLeft();
            else
                MoveRight();
        }
        else
        {
            Wait();
        }
    }

    protected void FixedUpdate()
    {
        Move();
    }

    protected void MoveLeft()
    {
        movement = -Vector3.right * Time.fixedDeltaTime * speed;
    }

    protected void MoveRight()
    {
        movement = Vector3.right * Time.fixedDeltaTime * speed;
    }

    protected void Move()
    {
        Vector3 pos = transform.position + movement;
        pos.x = Mathf.Clamp(pos.x, -3.6f, 3.6f);
        rigidbody.MovePosition(pos);
    }

    protected void Wait()
    {
        movement = Vector3.zero;
    }
}
