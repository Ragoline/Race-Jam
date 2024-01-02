using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Canvas;
    [SerializeField] private GameObject[] _health;
    [SerializeField] private GameObject _nitro;
    [SerializeField] private GameObject _window;
    [SerializeField] private Text _textWonLostCrashed;
    [SerializeField] private Text _textTheRace;
    [SerializeField] private Text _textGears;
    [SerializeField] private GameObject _finalWindow;
    [SerializeField] private GameObject _finishLine;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _lowerRoad;
    [SerializeField] private GameObject _sideObject;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _bigObstacle;
    [SerializeField] private GameObject _vehicle;
    [SerializeField] private GameObject _opponent;
    [SerializeField] private GameObject _gear;
    [SerializeField] private GameObject _pause;
    [SerializeField] private Text _countdown;
    [SerializeField] private Text _gearsNumber;
    [SerializeField] private Slider _race;
    [SerializeField] private Slider nitro;
    [SerializeField] private Sprite _redHeart;
    [SerializeField] private Sprite _ironHeart;
    [SerializeField] private Sprite _greenNitro;
    [SerializeField] private Sprite _yellowNitro;
    [SerializeField] private Sprite _redNitro;
    [SerializeField] private Image _nitroBackground;
    private Opponent opponent;
    public static int OpponentCar = 0;
    public static bool TimeFlows { get; private set; }
    public static bool OpponentExists = true;
    public static GameManager Instance;
    public int Vehicle = -2;
    private int gears = 0;
    public static int Health = 3, Nitro = 0;
    public static bool Final = false;
    private bool victory = false, pausable = true, up = true;
    private float roadObjectTime = 2f, sideObjectTime = 3f, _raceTime, begin = 3f, gear = 1.5f; // todo подумать насчёт сложности: стоит ли делать roadObjectTime переменной, которая меняется при высокой/низкой сложности гонки; можно сделать так, чтобы игрок, играющий несколько одинаковых гонок подряд, получал увеличенную сложность
    public static float Race = 20f;
    private Sprite[] vehicles;
    private Sprite[] obstacles;
    private Sprite[] bigObstacles;
    private Sprite[] sideObjects;
    private Car[] cars;

    public static float GameSpeed = 4f;

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
        n = AllCars.Cars.Length;
        cars = new Car[n];
        cars = AllCars.Cars;
        n = Resources.LoadAll<Sprite>("Images/SideObjects").Length;
        sideObjects = new Sprite[n];
        sideObjects = Resources.LoadAll<Sprite>("Images/SideObjects");

        // todo delete:
        CarController.Car = new RedCar();
    }

    private void Start()
    {
        _gearsNumber.text = "00";
        _race.maxValue = Race;
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
            GameManager.Window(_finalWindow, 1);
        if (!up)
        {
            if (_finalWindow.transform.position.y < Screen.height * 2f)
                GameManager.Window(_finalWindow, 2);
            else
                Exit();
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
                _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - CarController.speed * Time.deltaTime * GameSpeed);
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - CarController.speed * Time.deltaTime * GameSpeed);
                _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y - CarController.speed * Time.deltaTime * GameSpeed);
                if (_upperRoad.transform.position.y <= 2)
                    _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y + 16f);
                if (_road.transform.position.y <= -14)
                    _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y + 16f);
                if (_lowerRoad.transform.position.y <= -30)
                    _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y + 16f);
                #endregion

                #region finish line
                if (Final)
                {
                    if (_finishLine.transform.position.y > -16)
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - CarController.speed * Time.deltaTime * GameSpeed);
                    else
                    {
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, 16);
                        _finishLine.SetActive(false);
                        Final = false;
                    }
                }
                else
                {
                    if (_finishLine.transform.position.y > -16)
                        _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - CarController.speed * Time.deltaTime * GameSpeed);
                }
                #endregion finish line

                _raceTime += Time.deltaTime;
                _race.value = _raceTime;
                if (_race.value == _race.maxValue && !Final)
                    Finish(false);

                #region objects
                if (roadObjectTime == 2f)
                {
                    switch (Random.Range(0, 100))
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

                if (roadObjectTime < 2f && roadObjectTime >= 0f)
                    roadObjectTime += Time.deltaTime;
                else
                    roadObjectTime = 2f;

                gear -= Time.deltaTime;
                if (gear < 0f)
                {
                    gear += 3f;
                    CreateGear();
                }

                if (sideObjectTime == 3f)
                {
                    switch (Random.Range(0, 100))
                    {
                        case 0:
                            CreateSideObject();
                            break;
                    }
                }
                if (sideObjectTime < 3f && sideObjectTime > 0f)
                    sideObjectTime -= Time.deltaTime;
                else
                    sideObjectTime = 3f;
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
        roadObjectTime = 0f;
        int r = Random.Range(-1, 2);
        while (r == Vehicle)
            r = Random.Range(-1, 2);
        var go = Instantiate(_obstacle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Obstacle>().Create(r, obstacles[Random.Range(0, obstacles.Length)]);
        //roadObjectTime += Time.deltaTime;
    }

    private void CreateBigObstacle()
    {
        roadObjectTime = 0f;
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
        //roadObjectTime += Time.deltaTime;
    }

    private void CreateVehicle()
    {
        roadObjectTime = 0f;
        Vehicle = Random.Range(-1, 2);
        var go = Instantiate(_vehicle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Vehicle>().Create(Vehicle, vehicles[Random.Range(0, vehicles.Length)]);
        //roadObjectTime += Time.deltaTime;
    }

    private void CreateOpponent()
    {
        var go = Instantiate(_opponent, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Opponent>().Create(Vehicle, cars[OpponentCar]);
    }

    private void CreateGear()
    {
        var go = Instantiate(_gear, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Gear>().Create();
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

    public void Finish(bool crash)
    {
        TimeFlows = false;
        Final = true;
        pausable = false;
        Opponent.LetsGo = false;
        if (crash)
        {
            Debug.Log("defeat");
            victory = false;
            _finishLine.SetActive(false);
            //CarController.Instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            _textWonLostCrashed.text = "Crashed";
            _textWonLostCrashed.color = Color.red;
            _textGears.text = "0";
            _textTheRace.text = "";
            StartCoroutine(CrashCutScene());
        }
        else
        {
            if (Opponent.TheCar.transform.position.y < CarController.Instance.gameObject.transform.position.y)
            {
                Debug.Log("victory");
                victory = true;
            }
            else
            {
                Debug.Log("defeat");
                victory = false;
            }
            _textGears.text = gears.ToString();
            GameContainer.Current.AddGears(gears);
            SaveLoad.Save();
            _finishLine.SetActive(true);
            //CarController.Instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, 16f);
            StartCoroutine(FinalCutScene());
        }
    }

    private IEnumerator CrashCutScene()
    {
        CarController.Instance.LoseAnimation();
        yield return new WaitForSeconds(2);
        Debug.Log("crashed");
        OpenWindow();
    }

    private IEnumerator FinalCutScene()
    {
        yield return null;
        while (_finishLine.transform.position.y > 0f)
        {
            if (CarController.Instance.transform.position.x < 0f)
            {
                CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x + 1f * Time.deltaTime * GameManager.GameSpeed, CarController.Instance.transform.position.y);
            }
            else if (CarController.Instance.transform.position.x > 0f)
            {
                CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x - 1f * Time.deltaTime * GameManager.GameSpeed, CarController.Instance.transform.position.y);
            }
            _finishLine.transform.position = new Vector2(_finishLine.transform.position.x, _finishLine.transform.position.y - 2f * Time.deltaTime * GameManager.GameSpeed);
            CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x, CarController.Instance.transform.position.y + 1f * Time.deltaTime * GameManager.GameSpeed);

            _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);
            _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);
            _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);
            if (_road.transform.position.y <= -14)
                _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y + 16f * Time.deltaTime * GameManager.GameSpeed);
            if (_upperRoad.transform.position.y <= 2)
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y + 16f * Time.deltaTime * GameManager.GameSpeed);
            if (_lowerRoad.transform.position.y <= -30)
                _lowerRoad.transform.position = new Vector2(_lowerRoad.transform.position.x, _lowerRoad.transform.position.y + 16f * Time.deltaTime * GameManager.GameSpeed);

            if (victory)
            {
                _textWonLostCrashed.text = "Won";
                _textWonLostCrashed.color = Color.green;
                if (Opponent.TheCar.transform.position.y > 0)
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y + 1f * Time.deltaTime * GameManager.GameSpeed);
                else
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y + 2f * Time.deltaTime * GameManager.GameSpeed);
            }
            else
            {
                _textWonLostCrashed.text = "Lost";
                _textWonLostCrashed.color = Color.red;
                Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, _finishLine.transform.position.y + 2f); //todo это возможная замена решению
                /*if (Opponent.TheCar.transform.position.y > _finishLine.transform.position.y + 2f)
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y - 10f * Time.deltaTime * GameManager.GameSpeed);
                if (Opponent.TheCar.transform.position.y > _finishLine.transform.position.y + 5f)
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y - 20f * Time.deltaTime * GameManager.GameSpeed);
                if (Opponent.TheCar.transform.position.y < _finishLine.transform.position.y + 2f)
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y + 3f * Time.deltaTime * GameManager.GameSpeed);
                else
                    Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y - 1f * Time.deltaTime * GameManager.GameSpeed);*/
            }
            yield return null;
        }
        Final = false;
        while (Opponent.TheCar.transform.position.y < _finishLine.transform.position.y + 2f)
        {
            Opponent.TheCar.transform.position = new Vector2(Opponent.TheCar.transform.position.x, Opponent.TheCar.transform.position.y + 1f * Time.deltaTime * GameManager.GameSpeed);
            yield return null;
        }
        Debug.Log("finish");
        OpenWindow();
    }

    public void Continue()
    {
        Debug.Log("continue");
        SaveLoad.Save();
        CloseWindow();
    }

    public int LoseHealth()
    {
        Health--;
        SetHealth();
        if (Health <= 0)
        {
            Finish(true);
            return 0;
        }
        return Health;
    }

    private void SetHealth()
    {
        switch (Health)
        {
            case 6:
                _health[0].GetComponent<Image>().sprite = _ironHeart;
                _health[1].GetComponent<Image>().sprite = _ironHeart;
                _health[2].GetComponent<Image>().sprite = _ironHeart;
                break;
            case 5:
                _health[0].GetComponent<Image>().sprite = _ironHeart;
                _health[1].GetComponent<Image>().sprite = _ironHeart;
                _health[2].GetComponent<Image>().sprite = _redHeart;
                break;
            case 4:
                _health[0].GetComponent<Image>().sprite = _ironHeart;
                _health[1].GetComponent<Image>().sprite = _redHeart;
                _health[2].GetComponent<Image>().sprite = _redHeart;
                break;
            case 3:
                _health[0].GetComponent<Image>().sprite = _redHeart;
                _health[1].GetComponent<Image>().sprite = _redHeart;
                _health[2].GetComponent<Image>().sprite = _redHeart;
                break;
            case 2:
                _health[0].GetComponent<Image>().sprite = _redHeart;
                _health[1].GetComponent<Image>().sprite = _redHeart;
                _health[2].SetActive(false);
                break;
            case 1:
                _health[0].GetComponent<Image>().sprite = _redHeart;
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
                _nitroBackground.sprite = _redNitro;
                nitro.maxValue = 30f;
                break;
            case 2:
                _nitroBackground.sprite = _yellowNitro;
                nitro.maxValue = 20f;
                break;
            case 1:
                _nitroBackground.sprite = _greenNitro;
                nitro.maxValue = 10f;
                break;
            case 0:
                _nitro.SetActive(false);
                break;
        }
        //nitro.value = nitro.maxValue;
        nitro.value = 0;
    }

    public void SwitchNitro()
    {
        nitro.value += Time.deltaTime;
    }

    public bool GetNitro()
    {
        return nitro.value < nitro.maxValue ? true : false;
    }

    public void PickGear()
    {
        gears++;
        _gearsNumber.text = (gears < 10) ? "0" + gears : gears.ToString();
    }

    private void OpenWindow()
    {
        up = true;
        _window.SetActive(true);
        Debug.Log("open window");
        SaveLoad.Save();
    }

    private void CloseWindow()
    {
        up = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void Window(GameObject window, int speed)
    {
        window.transform.position = new Vector2(Screen.width / 2, (PlayerPrefs.GetInt("InstantMenu", 0) == 0 ? window.transform.position.y + speed * 500f * Time.deltaTime * GameManager.GameSpeed : (speed == 1 ? Screen.height / 2f : Screen.height * 2f)));
    }
}
