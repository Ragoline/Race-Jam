using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _lowerRoad;

    private float GameSpeed = 15f;
    private bool pause = false, obstacle = false;
    private float wait = 2f;
    private int stage = 0;
    private Vector2 firstPressPos, secondPressPos;

    void Update()
    {
        if (!pause)
            wait -= Time.deltaTime;
        if (wait <= 0f)
            stage++;
        if (stage == 0 && wait <= 0.3f)
            obstacle = true;
        if (stage == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                if (secondPressPos.x - firstPressPos.x > 100 && (firstPressPos.x > 0f || firstPressPos.y > 0f))
                {
                    firstPressPos = new Vector2();
                    secondPressPos = new Vector2();
                    Swipe(true);
                }
                else if (firstPressPos.x - secondPressPos.x > 100)
                {
                    firstPressPos = new Vector2();
                    secondPressPos = new Vector2();
                    Swipe(false);
                }
            }
        }

        switch (stage)
        {
            case 1:
                pause = true;
                Debug.Log("тут возникнет препятствие и надпись 'свайпни, чтобы объехать'"); // игрок может свайпнуть как влево, так и вправо
                wait = 3f;
                stage++;
                break;

            case 3:
                pause = true;
                Debug.Log("на центральной полосе появляется машина соперника и надпись 'задержи палец, чтобы использовать нитро'");

                wait = 5f;
                stage++;
                break;

            case 5:
                Debug.Log("как только игрок обгонит машину соперника, тутор заканчивается");
                SceneManager.LoadScene("Menu");
                break;
        }

        if (!pause)
        {
            _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - Time.deltaTime * GameSpeed);
            _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - Time.deltaTime * GameSpeed);
            _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y - Time.deltaTime * GameSpeed);
            if (_upperRoad.transform.position.y <= 2)
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y + 16f);
            if (_road.transform.position.y <= -14)
                _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y + 16f);
            if (_lowerRoad.transform.position.y <= -30)
                _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y + 16f);

            if (obstacle && (stage == 0 || stage == 2))
                _obstacle.transform.position = new Vector2(_obstacle.transform.position.x, _obstacle.transform.position.y - Time.deltaTime * GameSpeed);
        }
    }

    private void Swipe(bool right)
    {
        if (right)
        {

        }
        else
        {

        }
        stage++;
    }
}
