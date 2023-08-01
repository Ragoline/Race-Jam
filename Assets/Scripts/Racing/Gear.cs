using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.TimeFlows)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - CarController.speed);
            if (transform.position.y < -800)
                Destroy(gameObject);
            if (((Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 160 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 50) || (Mathf.Abs(transform.position.y - CarController.Instance.gameObject.transform.position.y) < 125 && Mathf.Abs(transform.position.x - CarController.Instance.gameObject.transform.position.x) < 50)))
            {
                PickUp();
            }
        }
        else
        if (GameManager.Final)
        {
            //Destroy(gameObject);
            if (transform.position.y >= Screen.height)
                Destroy(gameObject);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
        }
    }

    private void PickUp()
    {
        Destroy(gameObject);
        GameManager.Instance.PickGear();
    }

   public void Create()
    {
        Debug.Log("gear");
        transform.position = new Vector2(450, 1800);
        var x = transform.position.x + Random.Range(-1, 2) * 180;
        transform.position = new Vector2(x, transform.position.y);
        gameObject.transform.SetParent(GameManager.Instance.Canvas.transform);
    }
}
