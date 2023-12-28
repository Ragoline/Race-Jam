using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -8)
                Destroy(gameObject);
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);
        }
    }

    public void Create(int position, Sprite sprite)
    {
        //Debug.Log("side object " + position);
        _image.sprite = sprite;
        transform.position = new Vector2(-3.2f + 6.4f * position, 10);
        if (position == 1)
            transform.Rotate(0, 180f, 0);
    }
}
