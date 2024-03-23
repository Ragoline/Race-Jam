using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    float a = 1f;
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -12)
                Destroy(gameObject);
        }
        else
        if (GameManager.Final)
        {
            a -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
            //Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 3f * Time.deltaTime * GameManager.GameSpeed);
            Debug.Log("side object final");
        }
    }

    public void Create(int position, Sprite sprite)
    {
        //Debug.Log("side object " + position);
        _image.sprite = sprite;
        transform.position = new Vector2(-3.2f + 6.4f * position, 12);
        if (position == 1)
            transform.Rotate(0, 180f, 0);
    }
}
