using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    float a = 1f;
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -16f)
                Destroy(gameObject);
        }
        else
        if (GameManager.Final)
        {
            a -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
            //Destroy(gameObject);
            if (transform.position.y >= Screen.height)
                Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 3f * Time.deltaTime * GameManager.GameSpeed);
            Debug.Log("gear final");
        }
    }

   public void Create()
    {
        //Debug.Log("gear");
        transform.position = new Vector2(0, 10);
        var x = transform.position.x + Random.Range(-1, 2) * 1.8f;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
