using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Canvas Canvas;
    [SerializeField] private GameObject[] _health;
    [SerializeField] private GameObject _nitro;
    [SerializeField] private GameObject _window;
    [SerializeField] private GameObject _finalWindow;
    [SerializeField] private GameObject _finishLine;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _sideObject;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _bigObstacle;
    [SerializeField] private GameObject _vehicle;
    [SerializeField] private GameObject _opponent;
    [SerializeField] private GameObject _pause;
    [SerializeField] private Text _countdown;
    [SerializeField] private Slider _race;
    private Opponent opponent;
    public static bool TimeFlows { get; private set; }
    public static bool OpponentExists = true;
    public static GameManager Instance;
    public int Vehicle = -2;
    public static int Health = 6, Nitro = 0;
    public static bool Final = false;
    private bool victory = false, pausable = true, up = true;
    private float roadObjectTime = 1f, sideObjectTime = 1f, _raceTime, begin = 3f;
    public static float race = 10f;
    private Sprite[] vehicles;
    private Sprite[] obstacles;
    private Sprite[] bigObstacles;
    private Sprite[] sideObjects;
    private Sprite[] cars;

    private void Awake()
    {
        var n = Resources.LoadAll<Sprite>("Images/Vehicles").Length;
        vehicles = new Sprite[n];
        vehicles = Resources.LoadAll<Sprite>("Images/Vehicles");
        n = Resources.LoadAll<Sprite>("Images/Obstacles").Length;
        obstacles = new Sprite[n];
        obstacles = Resources.LoadAll<Sprite>("Images/Obstacles");
        n = Resources.LoadAll<Sprite>("Images/BigObstacles").Length;
        bigObstacles = new Sprite[n];
        bigObstacles = Resources.LoadAll<Sprite>("Images/BigObstacles");
        n = Resources.LoadAll<Sprite>("Images/Cars").Length;
        cars = new Sprite[n];
        cars = Resources.LoadAll<Sprite>("Images/Cars");
        n = Resources.LoadAll<Sprite>("Images/SideObjects").Length;
        sideObjects = new Sprite[n];
        sideObjects = Resources.LoadAll<Sprite>("Images/SideObjects");
    }

    private void Start()
    {
        _race.maxValue = race;
        _countdown.gameObject.SetActive(true);
        Vehicle = -2;
        Instance = this;
        TimeFlows = true;
        if (OpponentExists)
        {
            CreateOpponent();
        }
        SetHealth();
        SetNitro();
    }

    void Update()
    {
        if (up && _window.activeSelf && _finalWindow.transform.position.y < Screen.height / 2f)
            _finalWindow.transform.position = new Vector2(Screen.width / 2, _finalWindow.transform.position.y + 10f);
        if (!up)
        {
            if (_finalWindow.transform.position.y < Screen.height * 2f)
                _finalWindow.transform.position = new Vector2(Screen.width / 2, _finalWindow.transform.position.y + 20f);
            else
                SceneManager.LoadScene("Menu");
        }
        if (TimeFlows)
        {
            if (begin > 0f)
            {
                begin -= Time.deltaTime;
                _countdown.text = ((int)begin).ToString();
            }
            else
            {
                if (begin < 0f && begin > -0.2f)
                {
                    begin = 0f;
                    Begin();
                }

                #region road
                _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - CarController.speed);
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - CarController.speed);
                if (_road.transform.position.y <= -800)
                    _road.transform.position = new Vector2(_road.transform.position.x, _upperRoad.transform.position.y + 1600f);
                if (_upperRoad.transform.position.y <= -800)
                    _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _road.transform.position.y + 1600f);
                #endregion

                #region finish line
                if (Final)
                {
                    if (_finishLine.transform.position.y > -1000)
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - CarController.speed);
                    else
                    {
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, 2000);
                        _finishLine.SetActive(false);
                        Final = false;
                    }
                }
                else
                {
                    if (_finishLine.transform.position.y > -800)
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - CarController.speed);
                }
                #endregion finish line

                _raceTime += Time.deltaTime;
                _race.value = _raceTime;
                if (_race.value == _race.maxValue && !Final)
                    Finish();

                #region objects
                if (roadObjectTime == 1f)
                {
                    switch (Random.Range(0, 1000))
                    {
                        case 0:
                            CreateObstacle();
                            break;

                        case 1:
                            if (Vehicle == -2)
                                CreateBigObstacle();
                            break;

                        case 2:
                            if (Vehicle == -2)
                                CreateVehicle();
                            break;
                    }
                }

                if (roadObjectTime < 1f && roadObjectTime > 0f)
                    roadObjectTime -= Time.deltaTime;
                else
                    roadObjectTime = 1f;
                if (sideObjectTime == 1f)
                {
                    switch (Random.Range(0, 1000))
                    {
                        case 0:
                            CreateSideObject();
                            break;
                    }
                }
                if (sideObjectTime < 1f && sideObjectTime > 0f)
                    sideObjectTime -= Time.deltaTime;
                else
                    sideObjectTime = 1f;
                #endregion
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_window.activeSelf)
                CloseWindow();
            else
                Pause();
        }
    }

    private void CreateSideObject()
    {
        var go = Instantiate(_sideObject, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        int r = Random.Range(0, 2);
        go.GetComponent<SideObject>().Create(r, sideObjects[Random.Range(0, sideObjects.Length)]);
        sideObjectTime -= Time.deltaTime;
    }

    private void CreateObstacle()
    {
        int r = Random.Range(-1, 2);
        while (r == Vehicle)
            r = Random.Range(-1, 2);
        var go = Instantiate(_obstacle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Obstacle>().Create(r, obstacles[Random.Range(0, obstacles.Length)]);
        roadObjectTime -= Time.deltaTime;
    }

    private void CreateBigObstacle()
    {
        var go = Instantiate(_bigObstacle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        switch (Random.Range(-1, 1))
        {
            case -1:
                go.GetComponent<Obstacle>().CreateBig(-1, bigObstacles[Random.Range(0, bigObstacles.Length)]);
                break;

            case 0:
                go.GetComponent<Obstacle>().CreateBig(1, bigObstacles[Random.Range(0, bigObstacles.Length)]);
                break;
        }
        roadObjectTime -= Time.deltaTime;
    }

    private void CreateVehicle()
    {
        Vehicle = Random.Range(-1, 2);
        var go = Instantiate(_vehicle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Vehicle>().Create(Vehicle, vehicles[Random.Range(0, vehicles.Length)]);
        roadObjectTime -= Time.deltaTime;
    }

    private void CreateOpponent()
    {
        var go = Instantiate(_opponent, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Opponent>().Create(Vehicle, cars[Random.Range(0, vehicles.Length)]);
    }

    public void Pause()
    {
        if (pausable)
        {
            TimeFlows = !TimeFlows;
            _pause.SetActive(!_pause.activeSelf);
        }
    }

    public void Begin()
    {
        _countdown.gameObject.SetActive(false);
        if (OpponentExists)
        {
            Opponent.LetsGo = true;

        }
    }

    public void Finish()
    {
        TimeFlows = false;
        Final = true;
        pausable = false;
        Opponent.LetsGo = false;
        if (Opponent.Car.transform.position.y < CarController.Instance.gameObject.transform.position.y)
        {
            Debug.Log("victory");
            victory = true;
        }
        else
        {
            Debug.Log("defeat");
            victory = false;
        }
        _finishLine.SetActive(true);
        CarController.Instance.transform.rotation = new Quaternion(0, 0, 0, 0);
        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, 1800);
        StartCoroutine(FinalCutScene());
    }

    private IEnumerator FinalCutScene()
    {
        yield return null;
        while (_finishLine.transform.position.y > 800)
        {
            if (CarController.Instance.transform.position.x < 450)
            {
                CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x + 1f, CarController.Instance.transform.position.y);
            }
            else if (CarController.Instance.transform.position.x > 450)
            {
                CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x - 1f, CarController.Instance.transform.position.y);
            }
            _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - 1f);
            CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x, CarController.Instance.transform.position.y + 0.8f);

            _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - 1f);
            _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - 1f);
            if (_road.transform.position.y <= -800)
                _road.transform.position = new Vector2(_road.transform.position.x, _upperRoad.transform.position.y + 1600f);
            if (_upperRoad.transform.position.y <= -800)
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _road.transform.position.y + 1600f);

            if (victory)
            {
                // todo
                if (Opponent.Car.transform.position.y > 0)
                    Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y + 0.6f);
                else
                    Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y + 5f);
            }
            else
            {
                    if (Opponent.Car.transform.position.y > _finishLine.transform.position.y + 200f)
                        Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y - 10f);
                    if (Opponent.Car.transform.position.y > _finishLine.transform.position.y + 500f)
                        Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y - 20f);
                    if (Opponent.Car.transform.position.y < _finishLine.transform.position.y + 200f)
                        Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y + 3f);
                    else
                        Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y - 1f);
                }
            yield return null;
        }
        Final = false;
        while (Opponent.Car.transform.position.y < _finishLine.transform.position.y + 200f)
        {
            Opponent.Car.transform.position = new Vector2(Opponent.Car.transform.position.x, Opponent.Car.transform.position.y + 1f);
            yield return null;
        }
        Debug.Log("finish");
        OpenWindow();
    }

    public void LoseHealth()
    {
        Health--;
        SetHealth();
        if (Health <= 0)
        {
            // todo lose
        }
    }

    private void SetHealth()
    {
        switch (Health)
        {
            case 6:
                _health[0].GetComponent<Image>().color = Color.gray;
                _health[1].GetComponent<Image>().color = Color.gray;
                _health[2].GetComponent<Image>().color = Color.gray;
                break;
            case 5:
                _health[0].GetComponent<Image>().color = Color.gray;
                _health[1].GetComponent<Image>().color = Color.gray;
                _health[2].GetComponent<Image>().color = Color.red;
                break;
            case 4:
                _health[0].GetComponent<Image>().color = Color.gray;
                _health[1].GetComponent<Image>().color = Color.red;
                _health[2].GetComponent<Image>().color = Color.red;
                break;
            case 3:
                _health[0].GetComponent<Image>().color = Color.red;
                _health[1].GetComponent<Image>().color = Color.red;
                _health[2].GetComponent<Image>().color = Color.red;
                break;
            case 2:
                _health[0].GetComponent<Image>().color = Color.red;
                _health[1].GetComponent<Image>().color = Color.red;
                _health[2].SetActive(false);
                break;
            case 1:
                _health[0].GetComponent<Image>().color = Color.red;
                _health[1].SetActive(false);
                _health[2].SetActive(false);
                break;
            case 0:
                _health[0].SetActive(false);
                _health[1].SetActive(false);
                _health[2].SetActive(false);
                break;
        }
    }

    public void SetNitro()
    {
        switch (Nitro)
        {
            case 3:
                _nitro.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                _nitro.GetComponent<Image>().color = Color.yellow;
                break;
            case 1:
                _nitro.GetComponent<Image>().color = Color.green;
                break;
            case 0:
                _nitro.SetActive(false);
                break;
        }
    }

    private void OpenWindow()
    {
        up = true;
        _window.SetActive(true);
    }

    private void CloseWindow()
    {
        up = false;
    }

        public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
