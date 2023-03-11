using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : RoadObject
{
    private bool moving = false, right, toMove = false, toRound = false;
    public static float speed = 2f;
    public static CarController Instance;

    private void Start()
    {
        Instance = this;
        Position = 0;
    }

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    void Update()
    {
        if (GameManager.TimeFlows)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                if (secondPressPos.x - firstPressPos.x > 200 && (firstPressPos.x > 0f || firstPressPos.y > 0f))
                {
                    firstPressPos = new Vector2();
                    secondPressPos = new Vector2();
                    Move(true);
                }
                else if (firstPressPos.x - secondPressPos.x > 200)
                {
                    firstPressPos = new Vector2();
                    secondPressPos = new Vector2();
                    Move(false);
                }
            }

            if (toMove)
                MoveCar(right);
            if (toRound)
                Round();
        }
    }

    private void Move(bool right)
    {
        if (!moving && ((right && Position < 1) || (!right && Position > -1)))
        {
            Debug.Log("move");
            moving = true;
            //StartCoroutine(MoveCar(right));
            this.right = right;
            toMove = true;
        }
    }

    /*IEnumerator MoveCar(bool right)
    {
        var move = 180f;
        yield return null;
        while (move > 0)
        {
            if (right)
                transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
            else
                transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
            move -= 1f;

            if (move <= 65f)
            {
                if (right)
                    Position++;
                else
                    Position--;
            }

            yield return null;
        }
        moving = false;
    }*/
    float move = 180f;
    private void MoveCar(bool right)
    {
        if (right)
            transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
        move -= 1f;

        if (move <= 65f)
        {
            if (right)
                Position++;
            else
                Position--;
        }
        if (move == 0f)
        {
            moving = false;
            toMove = false;
            move = 180f;
        }
    }

    public void Crash()
    {
        // todo минус сердце
        //StartCoroutine(Round());
        toRound = true;
    }

    /*private IEnumerator Round()
    {
        var round = 0f;
        yield return null;
        while (GameManager.TimeFlows && round < 390f)
        {
            round += 1.5f;
            transform.Rotate(new Vector3(0f, 0f, 1.5f));
            yield return null;
        }
        round = 0f;
        while (GameManager.TimeFlows && round < 30f)
        {
            round += 0.5f;
            transform.Rotate(new Vector3(0f, 0f, -0.5f));
            yield return null;
        }
    }*/
    float round = 0f;
    private void Round()
    {
        if (round < 390f)
        {
            round += 1.5f;
            transform.Rotate(new Vector3(0f, 0f, 1.5f));
        }
        //round = 0f;
        if (round < 420f && round >= 390f)
        {
            round += 0.5f;
            transform.Rotate(new Vector3(0f, 0f, -0.5f));
        }
        if (round == 420f)
        {
            toRound = false;
            round = 0f;
        }
    }
}
