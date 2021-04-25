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

    private HoleManager hole;

    private Vector3 direction;

    private float speed = 3;

    private bool collidedWithAnObject = false; // for AIPlayer to check collision point correctly

    private int hitCountToPlayers = 0;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        rigidbody = GetComponent<Rigidbody>();

        hole = GameObject.FindGameObjectWithTag("Hole").GetComponent<HoleManager>();

        materialState = MaterialStates.Starting;

        direction = StartingDirectionForBlue();
        transform.position = StartingPositionForBlue();
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
            hole.ChangePosition_x();
            hole.InitializeScale();
            collidedWithAnObject = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BluePlayer"))
        {
            materialState = MaterialStates.Blue;
            renderer.material = blueMaterial;
            HitAPlayer(collision);
        }

        if (collision.gameObject.CompareTag("RedPlayer"))
        {
            materialState = MaterialStates.Red;
            renderer.material = redMaterial;
            HitAPlayer(collision);
        }

        if (collision.gameObject.CompareTag("Edge"))
        {
            direction = CalculateNewDirectionOnCollisionEdge(collision.gameObject.name);
            collidedWithAnObject = true;
        }
    }

    private void HitAPlayer(Collision collision)
    {
        direction = CalculateNewDirection(transform.InverseTransformPoint(collision.transform.position));
        collidedWithAnObject = true;
        hitCountToPlayers++;
        IncreaseHoleScale();
    }

    private void Score()
    {
        if (materialState == MaterialStates.Blue)
        {
            scoreManager.ScoreForBlue();
            transform.position = StartingPositionForRed();
            direction = StartingDirectionForRed();
        }
        else if (materialState == MaterialStates.Red)
        {
            scoreManager.ScoreForRed();
            transform.position = StartingPositionForBlue();
            direction = StartingDirectionForBlue();
        }
        else
            Debug.LogWarning("Wrong ball color!");
    }

    private Vector3 CalculateNewDirection(Vector3 collisionPoint)
    {
        Vector3 newDirection = direction;

        if (HitMiddleofPlayer(collisionPoint))
        {
            newDirection.x = 0;
            newDirection.z *= -1;
        }
        else if (HitLeftSideofPlayer(collisionPoint))
        {
            if (GoingRight())
            {
                newDirection.z *= -1;
                newDirection.x *= -1;
            }
            else if (GoingStraight())
            {
                newDirection.z *= -1;
                newDirection.x = -1;
            }
            else
            {
                newDirection.z *= -1;
            }
        }
        else if (HitRightSideofPlayer(collisionPoint))
        {
            if (GoingLeft())
            {
                newDirection.z *= -1;
                newDirection.x *= -1;
            }
            else if (GoingStraight())
            {
                newDirection.z *= -1;
                newDirection.x = 1;
            }
            else
            {
                newDirection.z *= -1;
            }
        }
        else if(HitRightEdgeofPlayer(collisionPoint))
        {
            if (GoingUp())
                newDirection.x = -1;
            else
                newDirection.x = -1;

            newDirection.z = FindDegreeForEdgeShotAccordingToColors();
        }
        else if(HitLeftEdgeofPlayer(collisionPoint))
        {
            if (GoingUp())
                newDirection.x = 1;
            else
                newDirection.x = 1;

            newDirection.z = FindDegreeForEdgeShotAccordingToColors();
        }

        return newDirection;
    }

    private Vector3 CalculateNewDirectionOnCollisionEdge(string collisionName)
    {
        Vector3 newDirection = direction;

        if (collisionName == "RightEdge" || collisionName == "LeftEdge")
            newDirection.x *= -1;
        else if (collisionName == "BottomEdge" || collisionName == "TopEdge")
            newDirection.z *= -1;

        return newDirection;
    }

    private bool GoingUp()
    {
        return direction.z > 0;
    }

    private bool GoingLeft()
    {
        return direction.x < 0;
    }

    private bool GoingRight()
    {
        return direction.x > 0;
    }

    private bool GoingStraight()
    {
        return direction.x == 0;
    }

    private float FindDegreeForEdgeShotAccordingToColors()
    {
        if (transform.position.z < 0) // Blue
            return 0.16f;
        else
            return -0.16f;
    }

    private Vector3 StartingPositionForBlue()
    {
        return new Vector3(0, 0.195f, -2);
    }

    private Vector3 StartingPositionForRed()
    {
        return new Vector3(0, 0.195f, 2);
    }

    private Vector3 StartingDirectionForBlue()
    {
        return new Vector3(0, 0, -1);
    }

    private Vector3 StartingDirectionForRed()
    {
        return new Vector3(0, 0, 1);
    }

    public float Direction_x
    {
        get { return direction.x; }
    }
    public float Direction_z
    {
        get { return direction.z; }
    }

    public bool CollidedWithAnObject
    {
        get { return collidedWithAnObject; }
        set { collidedWithAnObject = value; }
    }

    private bool HitMiddleofPlayer(Vector3 collisionPoint)
    {
        return collisionPoint.x <= GameManager.playerWidth / 12 && collisionPoint.x >= -GameManager.playerWidth / 12;
    }

    private bool HitLeftSideofPlayer(Vector3 collisionPoint)
    {
        return collisionPoint.x < -GameManager.playerWidth / 12 && collisionPoint.x >= -GameManager.playerWidth;
    }    
    
    private bool HitRightSideofPlayer(Vector3 collisionPoint)
    {
        return collisionPoint.x <= GameManager.playerWidth && collisionPoint.x > GameManager.playerWidth / 12;
    }

    private bool HitRightEdgeofPlayer(Vector3 collisionPoint)
    {
        return collisionPoint.x > GameManager.playerWidth; ;
    }

    private bool HitLeftEdgeofPlayer(Vector3 collisionPoint)
    {
        return collisionPoint.x < -GameManager.playerWidth;
    }

    private void IncreaseHoleScale()
    {
        if (hitCountToPlayers > 1 && hitCountToPlayers <= 21)
            hole.IncreaseScale();
    }
}
