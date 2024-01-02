using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : RoadObject
{
    [SerializeField] private SpriteRenderer _image;
    public int Length { get; private set; }
    private bool obstacle = true, moving = false, right;
    private float speed, move = 1.80f;

    private void Start()
    {
        Length = 1;
        if (CarController.speed > 5f)
            speed = CarController.speed - Random.Range(0.7f, 1f);
        else
            speed = CarController.speed - Random.Range(0.2f, 0.6f);
    }

    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed + speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -16 || transform.position.y > 18)
            {
                Destroy(gameObject);
                GameManager.Instance.Vehicle = -2;
            }
            /*if (obstacle && ((Length == 1 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 213 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 150) || (Length == 2 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 125 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 275)))
            {
                CarController.Instance.Crash();
                _image.color = new Color(1, 1, 1, 0.5f);
                obstacle = false;
            }*/
            if (!moving && move > 0f && Random.Range(0, 2000) == 10)
            {
                Move();
            }
            if (moving)
            {
                Step();
            }
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 3f * Time.deltaTime * GameManager.GameSpeed);
            Debug.Log("vehicle final");
        }
    }

    public void Create(int position, Sprite sprite)
    {
        //Debug.Log("vehicle");
        Length = 1;
        Position = position;
        _image.sprite = sprite;
        transform.position = new Vector2(0, 10);
        VisualPosition();
    }

    private void VisualPosition()
    {
        var x = transform.position.x + Position * 1.80f;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }

    private void Move()
    {
        moving = true;
        switch (Position)
        {
            case -1:
                right = true;
                break;

            case 0:
                if (Random.Range(0, 2) == 0)
                    right = true;
                else
                    right = false;
                break;

            case 1:
                right = false;
                break;

        }
        if (right)
            Position++;
        else
            Position--;
        GameManager.Instance.Vehicle = Position;
    }

    private void Step()
    {
        transform.position = new Vector2(transform.position.x + (right ? speed / 10f : -speed / 10f) * Time.deltaTime * GameManager.GameSpeed, transform.position.y);
        move -= speed / 10f * Time.deltaTime * GameManager.GameSpeed;
        if (move <= 0f)
        {
            moving = false;
        }
    }
}
