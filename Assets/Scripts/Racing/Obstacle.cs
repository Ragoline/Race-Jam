using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : RoadObject
{
    [SerializeField] private Image _image;
    public int Length { get; private set; }
    private bool obstacle = true;

    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed);
            if (transform.position.y < -800)
                Destroy(gameObject);
            if (obstacle && ((Length == 1 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 160 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 115) || (Length == 2 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 125 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 275)))
            {
                CarController.Instance.Crash();
                obstacle = false;
            }
        }
    }

    public void Create(int position, Sprite sprite)
    {
        Debug.Log("obstacle");
        Length = 1;
        Position = position;
        _image.sprite = sprite;
        transform.position = new Vector2(450, 1800);
        VisualPosition();
    }

    public void CreateBig(int position, Sprite sprite)
    {
        Debug.Log("big obstacle");
        Length = 2;
        Position = position;
        _image.sprite = sprite;
        if (Position == 1)
        {
            transform.Rotate(new Vector3(180f, 0f, 0f));
            transform.position = new Vector2(400, 1800);
        }
        else
        {
            transform.position = new Vector2(500, 1800);
        }
        VisualPosition();
    }

    private void VisualPosition()
    {
        var x = transform.position.x + Position * 180;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
