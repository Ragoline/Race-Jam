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
    [SerializeField] private Text _textCoins;
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
    [SerializeField] private AudioSource _sounds;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _playerCar;
    [SerializeField] private AudioSource _opponentCar;
    private Opponent opponent;
    public static int OpponentCar = 0;
    public static bool TimeFlows { get; private set; }
    public static bool OpponentExists = true;
    public static GameManager Instance;
    public int Vehicle = -2;
    private int gears = 0, coins = 0;
    public static int Health, Nitro;
    public static bool Final = false, RandomBonus = false;
    private bool victory = false, pausable = true, up = true;
    private float roadObjectTime = 2f, sideObjectTime = 3f, _raceTime, begin = 3f, gear = 1.5f; // todo подумать насчёт сложности: стоит ли делать roadObjectTime переменной, которая меняется при высокой/низкой сложности гонки; можно сделать так, чтобы игрок, играющий несколько одинаковых гонок подряд, получал увеличенную сложность
    public static float Race = 20f;
    public static string Area = "City";
    private Sprite[] vehicles;
    private Sprite[] obstacles;
    private Sprite[] bigObstacles;
    private Sprite[] sideObjects;
    private Car[] cars;
    public static Car Player;
    public static float GameSpeed = 4f;
    [SerializeField] private AudioClip[] sounds = new AudioClip[8];

    private void Awake()
    {
        var n = Resources.LoadAll<Sprite>("Images/Vehicles").Length;
        vehicles = new Sprite[n];
        vehicles = Resources.LoadAll<Sprite>("Images/Vehicles");
        n = Resources.LoadAll<Sprite>("Images/Obstacles/" + Area).Length;
        obstacles = new Sprite[n];
        obstacles = Resources.LoadAll<Sprite>("Images/Obstacles/" + Area);
        n = Resources.LoadAll<Sprite>("Images/BigObstacles/" + Area).Length;
        bigObstacles = new Sprite[n];
        bigObstacles = Resources.LoadAll<Sprite>("Images/BigObstacles/" + Area);
        n = AllCars.Cars.Length;
        cars = new Car[n];
        cars = AllCars.Cars;
        n = Resources.LoadAll<Sprite>("Images/SideObjects/" + Area).Length;
        sideObjects = new Sprite[n];
        sideObjects = Resources.LoadAll<Sprite>("Images/SideObjects/" + Area);
        _road.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Roads/" + Area);
        Debug.Log("Images/Roads/" + Area);

        begin = 3f;

        CarController.Car = Player;
    }

    private void Start()
    {
        _upperRoad.GetComponent<SpriteRenderer>().sprite = _road.GetComponent<SpriteRenderer>().sprite;
        _lowerRoad.GetComponent<SpriteRenderer>().sprite = _road.GetComponent<SpriteRenderer>().sprite;
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
        AudioUpdate();
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
            if (begin > 0f) // countdown
            {
                begin -= Time.deltaTime;
                if (begin > 2.95f && begin < 3.00f)
                    PlaySound(1);
                else if (begin > 1.99f && begin < 2.01f)
                    PlaySound(1); // todo подправить звук? сделать его продолжительнее - чтобы он был на секунду
                else if (begin > 0.99f && begin < 1.01f)
                    PlaySound(1);
                else if (begin >= 0f && begin < 0.01f)
                {
                    PlaySound(2);
                    PlaySound(5);
                    PlaySound(7);
                }
                _countdown.text = ((int)(begin + 1f)).ToString();
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

                if (MenuManager.SoundsOn)
                    _opponentCar.volume = ((10.0f - Mathf.Abs(_opponent.transform.position.y + 6.0f)) / 10.0f) + 0.2f;
                else
                    _opponentCar.volume = 0f;
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
        _opponent = go;
        _opponentCar = go.GetComponent<AudioSource>();
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
            PlaySound(0);
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
        _playerCar.Stop();
        _opponentCar.Stop();
        if (crash)
        {
            Debug.Log("defeat");
            victory = false;
            _finishLine.SetActive(false);
            //CarController.Instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            _textWonLostCrashed.text = "Crashed";
            _textWonLostCrashed.color = Color.red;
            _textGears.text = "0";
            _textCoins.text = "0";
            _textTheRace.text = "";
            StartCoroutine(CrashCutScene());
        }
        else
        {
            PlaySound(4);
            if (Opponent.TheCar.transform.position.y < CarController.Instance.gameObject.transform.position.y)
            {
                Debug.Log("victory");
                coins = 1;
                if (Opponent.Car.Speed > CarController.Car.Speed)
                {
                    coins = 5;
                }
                else if (CarController.Car.Speed == Opponent.Car.Speed)
                {
                    coins = 3;
                }
                else if (CarController.Car.Speed - Opponent.Car.Speed < 0.5f)
                {
                    coins = 2;
                }
                if (RandomBonus)
                    coins *= 2;
                victory = true;
            }
            else
            {
                Debug.Log("defeat");
                victory = false;
            }
            _textGears.text = gears.ToString();
            _textCoins.text = coins.ToString();
            GameContainer.Current.AddGears(gears);
            GameContainer.Current.AddCoins(coins);
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

    private IEnumerator FinalCutScene() // todo: выключить звуки всех машин
    {
        yield return null;
        while (_finishLine.transform.position.y > 0f)
        {
            if (CarController.Instance.transform.position.x < -0.5f)
            {
                CarController.Instance.transform.position = new Vector2(CarController.Instance.transform.position.x + 1f * Time.deltaTime * GameManager.GameSpeed, CarController.Instance.transform.position.y);
            }
            else if (CarController.Instance.transform.position.x > 0.5f)
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
                //coins = 1;
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
        PlaySound(3);
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

    public void PickUpGear()
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
        PlaySound(0);
        SceneManager.LoadScene("Menu");
    }

    public static void Window(GameObject window, int speed)
    {
        window.transform.position = new Vector2(Screen.width / 2, (PlayerPrefs.GetInt("InstantMenu", 0) == 0 ? window.transform.position.y + speed * 500f * Time.deltaTime * GameManager.GameSpeed : (speed == 1 ? Screen.height / 2f : Screen.height * 2f)));
    }

    private void AudioUpdate()
    {
        _music.volume = MenuManager.MusicOn ? 1f : 0f;
        _sounds.volume = MenuManager.SoundsOn ? 1f : 0f;
        if (MenuManager.MusicOn)
        {
            AudioClip[] music = new AudioClip[3];
            music = Resources.LoadAll<AudioClip>("Music/Racing Music");
            Debug.Log(music.Length);
            _music.clip = music[Random.Range(0, music.Length)];
            _music.Play();
        }
    }

    /// <summary>
    /// Play the sound
    /// </summary>
    /// <param name="n">0 - click; 1 - countdown; 2 - countdownfinish; 3 - crash; 4 - finish; 5 - your car; 6 - vehicle; 7 - enemy car</param>
    private void PlaySound(int n)
    {
        if (n < 5)
        {
            _sounds.clip = sounds[n];
            _sounds.Play();
        }
        else if (n == 5)
        {
            _playerCar.clip = sounds[5]; // todo изменить звук
            _playerCar.Play();
        }
        else if (n == 7)
        {
            _opponentCar.clip = sounds[7];
            _opponentCar.Play();
            // todo сделать звук машины врага, который будет становится громче при приближении и туше при отдалении
        }
    }
}
