using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : RoadObject
{
    [SerializeField] private SpriteRenderer _image;
    public int Length { get; private set; }
    private bool obstacle = true;

    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -16)
                Destroy(gameObject);
            /*if (obstacle && ((Length == 1 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 160 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 115) || (Length == 2 && Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 125 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 275)))
            {
                CarController.Instance.Crash();
                _image.color = new Color(1, 1, 1, 0.5f);
                obstacle = false;
            }*/
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 3f * Time.deltaTime * GameManager.GameSpeed);
            Debug.Log("obstacle final");
        }
    }

    public void Create(int position, Sprite sprite)
    {
        Length = 1;
        Position = position;
        _image.sprite = sprite;
        transform.position = new Vector2(0, 12);
        VisualPosition();
    }

    public void CreateBig(int position, Sprite sprite)
    {
        Length = 2;
        Position = position;
        _image.sprite = sprite;
        if (Position == 1)
        {
            transform.position = new Vector2(0, 12);
        }
        else
        {
            transform.Rotate(new Vector3(0f, 180f, 0f));
            transform.position = new Vector2(0, 12);
        }
        VisualPosition();
    }

    private void VisualPosition()
    {
        float x;
        if (Length == 1)
            x = transform.position.x + Position * 1.80f;
        else
            x = transform.position.x + Position * 1.3f;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
