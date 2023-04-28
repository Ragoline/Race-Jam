using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opponent : RoadObject
{
    [SerializeField] private Image _image;
    [SerializeField] private float _speed;
    public static bool LetsGo = false;
    private float start = 200f, touchable = 0f;
    public static GameObject Car;

    private void Start()
    {
        Car = gameObject;
        Position = -1;
        _speed = CarController.speed + 1f; // todo хрень
    }

    private void Update()
    {
        if (GameManager.TimeFlows && LetsGo)
        {
            if (start < 2000f)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 6f);
                start += 6f;
            }
            else
            {
                if (touchable == 0f && _image.color.a < 1f)
                    _image.color = new Color(1f, 1f, 1f, 1f);
                transform.position = new Vector2(transform.position.x, transform.position.y + _speed - CarController.speed);
                if (touchable == 0f && ((Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 160 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 115)))
                {
                    CarController.Instance.Crash();
                    touchable = 2f;
                    _image.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
    }

    public void Create(int position, Sprite sprite)
    {
        Debug.Log("vehicle");
        Position = position;
        _image.sprite = sprite;
        transform.position = new Vector2(630, 200);
        VisualPosition();
    }

    private void VisualPosition()
    {
        var x = transform.position.x + Position * 180;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
