using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : PlayerBase
{
    public Transform hole;
    public Transform ball;

    private Ball ballScript;

    private enum AIStates { FindCollisionPoint, SelectAIType, BestCaseAI, WorstCaseAI }
    private AIStates aIState;

    float possibleCollisionPoint_x;

    private float newDestination_x;
    private bool reachedNewDestination = false;
    private bool newDestinationDefinedForWorstCase = false;

    private int gameLevel;

    private void OnEnable()
    {
        startingPosition = new Vector3(0, 0.2f, 8);
        transform.position = startingPosition;
    }

    private void Start()
    {
        ballScript = ball.GetComponent<Ball>();
    }

    private void Update()
    {
        base.Update();
        AIHandler();

        if (ballScript.CollidedWithAnObject)
        {
            aIState = AIStates.FindCollisionPoint;
            ballScript.CollidedWithAnObject = false;
        }
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void AIHandler()
    {
        if (aIState == AIStates.FindCollisionPoint)
        {
            possibleCollisionPoint_x = Distance() * ballScript.Direction_x + ball.position.x;
            reachedNewDestination = false;
            newDestinationDefinedForWorstCase = false;
            aIState = AIStates.SelectAIType;
        }
        else if (aIState == AIStates.SelectAIType)
        {
            int rnd = Random.Range(1, gameLevel);

            if (rnd <= gameLevel - 1)
                aIState = AIStates.BestCaseAI;
            else
                aIState = AIStates.WorstCaseAI;
        }
        else if (aIState == AIStates.BestCaseAI)
        {
            BestCaseAI();
            CheckMovement();
        }
        else if (aIState == AIStates.WorstCaseAI)
        {
            WorstCaseAI();
            CheckMovement();
        }
    }

    private float Distance()
    {
        if (ball.position.z < transform.position.z)
            return transform.position.z - ball.position.z;
        else
            return ball.position.z - transform.position.z;
    }

    private void BestCaseAI()
    {
        if (HoleAndCollisionPointAreCloseInX())
            ShootFromMiddle();
        else if (HoleIsInRightSideofCollisionPoint())
            ShootFromLeft();
        else if (HoleIsInLeftSideofCollisionPoint())
            ShootFromRight();
    }

    private void WorstCaseAI()
    {
        if (!newDestinationDefinedForWorstCase)
        {
            newDestination_x = Random.Range(possibleCollisionPoint_x - GameManager.playerWidth - 0.3f, possibleCollisionPoint_x + GameManager.playerWidth - 0.3f);
            newDestinationDefinedForWorstCase = true;
        }
    }

    private void ShootFromMiddle()
    {
        newDestination_x = possibleCollisionPoint_x;
    }

    private void ShootFromLeft()
    {
        newDestination_x = possibleCollisionPoint_x + GameManager.playerWidth / 2f;
    }

    private void ShootFromRight()
    {
        newDestination_x = possibleCollisionPoint_x - GameManager.playerWidth / 2;
    }

    private void CheckMovement()
    {
        if (!reachedNewDestination)
        {
            if (newDestination_x - transform.position.x < 0.1 && newDestination_x - transform.position.x > -0.1) // for avoid jittering
                reachedNewDestination = true;
            else if (newDestination_x < transform.position.x)
                state = States.Left;
            else if (newDestination_x > transform.position.x)
                state = States.Right;
        }
        else
            state = States.Wait;
    }

    private bool HoleAndCollisionPointAreCloseInX()
    {
        return hole.position.x - possibleCollisionPoint_x < 1 && hole.position.x - possibleCollisionPoint_x > -1;
    }

    private bool HoleIsInLeftSideofCollisionPoint()
    {
        return hole.position.x < possibleCollisionPoint_x;
    }

    private bool HoleIsInRightSideofCollisionPoint()
    {
        return hole.position.x > possibleCollisionPoint_x;
    }

    public int GameLevel
    {
        set { gameLevel = value; }
    }
}
