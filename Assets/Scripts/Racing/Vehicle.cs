using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// todo сделать у автомобилей возможность поворачивать
public class Vehicle : RoadObject
{
    [SerializeField] private Image _image;
    public int Length { get; private set; }
    private bool obstacle = true;
    private float speed;

    private void Start()
    {
        Length = 1;
        speed = Random.Range(1.2f, 1.7f);
    }

    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed + speed);
            if (transform.position.y < -800 || transform.position.y > 1800)
            {
                Destroy(gameObject);
                GameManager.Instance.Vehicle = -2;
            }
            if (obstacle && ((Length == 1 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 220 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 138) || (Length == 2 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 125 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 275)))
            {
                CarController.Instance.Crash();
                obstacle = false;
            }
        }
    }

    public void Create(int position, Sprite sprite)
    {
        Debug.Log("vehicle");
        Length = 1;
        Position = position;
        _image.sprite = sprite;
        transform.position = new Vector2(450, 1800);
        VisualPosition();
    }

    private void VisualPosition()
    {
        var x = transform.position.x + Position * 180;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
