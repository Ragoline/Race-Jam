using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Slider _nitro;
    [SerializeField] private GameObject _dialogSwipe;
    [SerializeField] private GameObject _dialogHold;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _opponent;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _lowerRoad;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSource _sounds;

    private float GameSpeed = 15f;
    private bool pause = false, obstacle = false, nitro = false;
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
                _dialogSwipe.SetActive(true);
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
                    _opponent.transform.position = new Vector2(_opponent.transform.position.x, _opponent.transform.position.y - 0.02f);
                if (wait <= 0f)
                {
                    stage++;
                    wait = 1f;
                }
                break;

            case 4:
                pause = true;
                _dialogHold.SetActive(true);
                Hold();
                break;

            case 5:
                if (_opponent.transform.position.y <= -10f)
                {
                    Destroy(_opponent);
                    stage++;
                }
                if (Input.GetMouseButtonDown(0))
                    _sounds.Play();
                if (Input.GetMouseButton(0))
                {
                    pause = false;
                    GameSpeed = 20f;

                    _nitro.value = 1f - (_opponent.transform.position.y + 10f) / 17.3f; // 7.3f -10f

                    if (_opponent.transform.position.y < 6f)
                        _dialogHold.SetActive(false);
                }
                else
                {
                    GameSpeed = 15f;
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
                _opponent.transform.position = new Vector2(_opponent.transform.position.x, _opponent.transform.position.y - Time.deltaTime * GameSpeed * 0.1f * (nitro && GameSpeed == 15f ? 0f : 1f));
        }
    }

    private void Hold()
    {
        _nitro.gameObject.SetActive(true);
        if (Input.GetMouseButton(0) && wait > 0f)
           wait -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        Debug.Log(wait);

        if (Input.GetMouseButton(0) && wait <= 0f)
        {
            // ��� �����
            stage++;
            _sounds.Play();
            GameSpeed += 3f;
            nitro = true;
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
        _dialogSwipe.SetActive(false);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
