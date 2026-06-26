using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    //[SerializeField] RectTransform rect;
    float sec = 2f;

    void Update()
    {
        if (sec == 2f) {
            if (transform.position.y-3000 > -200)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - Time.deltaTime * 200);
            }
            else
                sec -= Time.deltaTime;
        }
        else if (sec > 0f)
        {
            sec -= Time.deltaTime;
        }
        else
        {
            if (transform.position.y - 3000 < 200)
                transform.position = new Vector2(transform.position.x, transform.position.y + Time.deltaTime * 200);
            else
                Destroy(transform.parent.gameObject);
            
        }
    }
}
