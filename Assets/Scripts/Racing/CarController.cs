using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : RoadObject
{
    private bool moving = false, right, toMove = false, toRound = false, nitro = false;
    public static Car Car;
    public static float speed, turnSpeed;
    public static CarController Instance;
    private float hold = 0f;

    private void Start()
    {
        Instance = this;
        Position = 0;
        speed = Car.Speed;
        turnSpeed = Car.TurnSpeed;
    }

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    void Update()
    {
        if (GameManager.TimeFlows && Opponent.LetsGo)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButton(0))
            {
                hold += Time.deltaTime;
                if (hold >= 1f)
                {
                    Nitro(true);
                }
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
                if (hold >= 1f)
                {
                    Nitro(false);
                }
                hold = 0f;
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
            this.right = right;
            toMove = true;
        }
    }

    float move = 180f;
    private void MoveCar(bool right)
    {
        if (right)
            transform.position = new Vector2(transform.position.x + turnSpeed, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - turnSpeed, transform.position.y);
        move -= turnSpeed;

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
        GameManager.Instance.LoseHealth();
        toRound = true;
    }

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

    public void Nitro(bool enable)
    {
        if (enable)
        {
            if (!nitro)
            {
                if (GameManager.Nitro > 0)
                {
                    Debug.Log("nitro");
                    speed += 2f;
                    nitro = true;
                }
            }
            else if (GameManager.Instance.GetNitro())
            {
                GameManager.Instance.SwitchNitro();
            }
        }
        else
        {
            if (nitro)
            {
                speed -= 2f;
                nitro = false;
            }
        }
    }
}
