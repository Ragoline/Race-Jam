using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : RoadObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private AudioSource _sound;
    private bool moving = false, right, moved = false, nitro = false;
    public static Car Car;
    public static float speed, turnSpeed;
    public static CarController Instance;
    private float hold = 0f;

    private void Start()
    {
        sprite.sprite = Resources.Load<Sprite>(Car.Look);
        Instance = this;
        Position = 0;
        speed = Car.Speed;
        turnSpeed = Car.TurnSpeed;
        if (!MenuManager.SoundsOn)
            _sound.volume = 0f;
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

                if (secondPressPos.x - firstPressPos.x > 100 && (firstPressPos.x > 0f || firstPressPos.y > 0f))
                {
                    firstPressPos = new Vector2();
                    secondPressPos = new Vector2();
                    Move(true);
                }
                else if (firstPressPos.x - secondPressPos.x > 100)
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

            if (moving)
                MoveCar(right);
        }
    }

    private void Move(bool right)
    {
        if (!moving && ((right && Position < 1) || (!right && Position > -1)))
        {
            moving = true;
            this.right = right;
            moved = false;
        }
    }

    float move = 1.8f;
    private void MoveCar(bool right)
    {
        if (right)
            transform.position = new Vector2(transform.position.x + turnSpeed * Time.deltaTime * GameManager.GameSpeed, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - turnSpeed * Time.deltaTime * GameManager.GameSpeed, transform.position.y);
        move -= turnSpeed * Time.deltaTime * GameManager.GameSpeed;

        if (move <= 0.65f && !moved)
        {
            moved = true;
            if (right)
                Position++;
            else
                Position--;
        }
        if (move <= 0f)
        {
            moving = false;
            moved = false;
            move = 1.8f;
        }
    }

    public void Crash()
    {
        if (GameManager.Instance.LoseHealth() > 0)
            animator.Play("Crash");
    }

    public void LoseAnimation()
    {
        animator.Play("Lose");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Final)
            if (collision.gameObject.name.Contains("Gear"))
            {
                Destroy(collision.gameObject);
                GameManager.Instance.PickUpGear();
            }
            else
            {
                Crash();
                collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                Destroy(collision.gameObject.GetComponent<BoxCollider2D>());
            }
    }

    public void Nitro(bool enable)
    {
        if (enable)
        {
            if (!nitro)
            {
                if (GameManager.Nitro > 0 && GameManager.Instance.GetNitro())
                {
                    Debug.Log("nitro");
                    GameManager.Instance.NitroSound();
                    speed += 2f;
                    nitro = true;
                }
            }
            else if (GameManager.Instance.GetNitro())
            {
                GameManager.Instance.SwitchNitro();
            }
            else
            {
                speed -= 2f;
                nitro = false;
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

    public void Finish()
    {
        animator.enabled = false;
        SoundsOff(true);
    }

    public void SoundsOff(bool off)
    {
        _sound.volume = off ? 0f : 1f;
    }
}
