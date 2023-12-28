using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed * Time.deltaTime * GameManager.GameSpeed);
            if (transform.position.y < -8f)
                Destroy(gameObject);
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            if (transform.position.y >= Screen.height)
                Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);
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
