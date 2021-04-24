using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material startingMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;

    private enum MaterialStates { Starting, Red, Blue }
    private MaterialStates materialState;

    private Renderer renderer;
    private ScoreManager scoreManager;
    private Rigidbody rigidbody;

    private Vector3 direction;

    private float speed = 3;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        rigidbody = GetComponent<Rigidbody>();

        materialState = MaterialStates.Starting;

        direction = StartingDirection();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        rigidbody.MovePosition(transform.position + direction * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            Score();
            renderer.material = startingMaterial;
            // Start with non-score
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BluePlayer"))
        {
            materialState = MaterialStates.Blue;
            renderer.material = blueMaterial;
            direction = CalculateNewDirection(transform.InverseTransformPoint(collision.transform.position));
        }

        if (collision.gameObject.CompareTag("RedPlayer"))
        {
            materialState = MaterialStates.Red;
            renderer.material = redMaterial;
            direction = CalculateNewDirection(transform.InverseTransformPoint(collision.transform.position));
        }

        if (collision.gameObject.CompareTag("Edge"))
        {
            direction = CalculateNewDirectionOnCollisionEdge(transform.InverseTransformPoint(collision.transform.position), collision.gameObject.name);
        }
    }

    private void Score()
    {
        if (materialState == MaterialStates.Blue)
            scoreManager.ScoreForBlue();
        else if (materialState == MaterialStates.Red)
            scoreManager.ScoreForRed();
        else
            Debug.LogError("Wrong ball color!");
    }

    private Vector3 StartingDirection()
    {
        return new Vector3(Random.Range(-1, 2), 0, -1);
    }

    private Vector3 CalculateNewDirection(Vector3 collisionPoint)
    {
        Vector3 newDirection = direction;

        if (collisionPoint.x <= 0.15 && collisionPoint.x >= -0.15)
        {
            newDirection.x = 0;
            newDirection.z *= -1;
        }
        else if (collisionPoint.x < -0.15 && collisionPoint.x >= -1.5)
        {
            if (direction.x >= 0)
            {
                newDirection.z *= -1;
                newDirection.x *= -1;
            }
            else
            {
                newDirection.z *= -1;
            }
        }
        else if (collisionPoint.x <= 1.5 && collisionPoint.x > 0.15)
        {
            if (direction.x <= 0)
            {
                newDirection.z *= -1;
                newDirection.x *= -1;
            }
            else
            {
                newDirection.z *= -1;
            }
        }
        else if(collisionPoint.x > 1.5)
        {
            newDirection.z *= -1;
            newDirection.x = -0.84f;
        }
        else if(collisionPoint.x < -1.5)
        {
            newDirection.z *= -1;
            newDirection.x = 0.84f;
        }

        return newDirection;
    }

    private Vector3 CalculateNewDirectionOnCollisionEdge(Vector3 collisionPoint, string collisionName)
    {
        Vector3 newDirection = direction;

        if (collisionName == "RightEdge" || collisionName == "LeftEdge")
            newDirection.x *= -1;
        else if (collisionName == "BottomEdge" || collisionName == "TopEdge")
            newDirection.z *= -1;

        return newDirection;
    }
}
