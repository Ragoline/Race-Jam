using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideObject : MonoBehaviour
{
    [SerializeField] private Image _image;
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed);
            if (transform.position.y < -800)
                Destroy(gameObject);
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
        }
    }

    public void Create(int position, Sprite sprite)
    {
        Debug.Log("side object " + position);
        _image.sprite = sprite;
        transform.position = new Vector2(120 + 660 * position, 1800);
        if (position == 1)
            transform.Rotate(0, 180f, 0);
    }
}
