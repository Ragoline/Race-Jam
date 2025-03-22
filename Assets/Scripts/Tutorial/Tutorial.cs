using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _opponent;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _lowerRoad;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSource _sounds;

    private float GameSpeed = 15f;
    private bool pause = false, obstacle = false;
    private float wait = 2f;
    private int stage = 0;
    private Vector2 firstPressPos, secondPressPos;

    void Update()
    {
        if (!pause)
            wait -= Time.deltaTime;
        if (wait <= 0f && stage == 0)
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

            case 2:
                if (_obstacle.transform.position.y <= -5f)
                {
                    Destroy(_obstacle);
                    stage++;
                }
                if (_player.position.x < 0)
                {
                    if (_player.position.x > -2f)
                        _player.position = new Vector2(_player.position.x - 0.05f, _player.position.y);
                    else
                        stage++;
                }
                else if (_player.position.x > 0)
                {
                    if (_player.position.x < 2f)
                        _player.position = new Vector2(_player.position.x + 0.05f, _player.position.y);
                    else
                        stage++;
                }
                break;

            case 3:
                if (wait <= 0.3f)
                    _opponent.transform.position = new Vector2(_opponent.transform.position.x, _opponent.transform.position.y - 0.01f);
                if (wait <= 0f)
                {
                    stage++;
                    wait = 1f;
                }
                break;

            case 4:
                pause = true;
                Debug.Log("на центральной полосе появляется машина соперника и надпись 'задержи палец, чтобы использовать нитро'");
                Hold();
                break;

            case 5:
                if (_opponent.transform.position.y <= -10f)
                {
                    Destroy(_opponent);
                    stage++;
                }
                break;

            case 6:
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

            if (obstacle && (stage == 0 || stage == 2 || stage == 3))
                _obstacle.transform.position = new Vector2(_obstacle.transform.position.x, _obstacle.transform.position.y - Time.deltaTime * GameSpeed);
            if (stage == 5)
                _opponent.transform.position = new Vector2(_opponent.transform.position.x, _opponent.transform.position.y - Time.deltaTime * GameSpeed * 0.01f);
            
            // в целом: пока игрок держит палец на экране, машина врага двигается вниз, и пауза = фолс, если не держит - нет этого
            // todo надо сделать часть с нитро так, чтобы когдла игрок перестаёт держать палец на экране, ставится на паузу; а машина врага двигается вниз быстрее
        }
    }

    private void Hold()
    {
        wait -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && wait <= 0f)
        {
            // вкл нитро

            Debug.Log("nitro");
            _sounds.Play();
            GameSpeed += 10f;
        }
    }

    private void Swipe(bool right)
    {
        pause = false;
        if (right)
        {
            Debug.Log("swipe right");
            _player.position = new Vector2(_player.position.x + 0.05f, _player.position.y);
        }
        else
        {
            Debug.Log("swipe left");
            _player.position = new Vector2(_player.position.x - 0.05f, _player.position.y);
        }
        wait = 3f;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
