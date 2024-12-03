using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private GameObject _windows;
    [SerializeField] private GameObject _storyWindow;
    [Header("Racing")]
    [SerializeField] private GameObject _racingWindow;
    [SerializeField] private Slider _areas;
    [SerializeField] private Slider _opponents;
    [SerializeField] private Slider _lengths;
    [Header("Windows")]
    [SerializeField] private GameObject _dailyGiftWindow;
    [SerializeField] private GameObject _dailyQuestWindow;
    [SerializeField] private GameObject _optionsWindow;
    [SerializeField] private GameObject _chooseWindow;
    [Header("Choose")]
    [SerializeField] private Button _greenNitro;
    [SerializeField] private Button _yellowNitro;
    [SerializeField] private Button _redNitro;
    [SerializeField] private Button _oneShield;
    [SerializeField] private Button _twoShields;
    [SerializeField] private Image _anotherShield1;
    [SerializeField] private Button _threeShields;
    [SerializeField] private Image _anotherShield2;
    [SerializeField] private Image _anotherShield3;
    [SerializeField] private Transform _choiceShield;
    [SerializeField] private Transform _choiceNitro;
    [Header("Audio")]
    [SerializeField] private Image _soundsImage;
    [SerializeField] private Image _musicImage;
    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;
    [SerializeField] private Sprite _soundsOn;
    [SerializeField] private Sprite _soundsOff;
    [SerializeField] private AudioSource _sounds;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioClip[] sounds = new AudioClip[2];
    [Header("Rest")]
    [SerializeField] private Image _carImage;
    [SerializeField] private Image _dailyGiftImage;
    [SerializeField] private Sprite _dgbColoured;
    [SerializeField] private Sprite _dgbBlacknWhite;
    [SerializeField] private Button _previousCar;
    [SerializeField] private Button _nextCar;
    [SerializeField] private Button _dailyGiftButton;
    [SerializeField] private Toggle _instantMenu;
    [SerializeField] private Text _captionText;
    [SerializeField] private Animator _dailyGiftAnimator;

    private bool up = false;
    public static bool MusicOn;
    public static bool SoundsOn;
    public static MenuManager Instance;
    public static bool InstantMenu;
    private float wait = 0f;
    private int step = 0, shields = 0, nitro = 0;
    private Car[] boughtCars;
    private int currentCar = 0;

    private void Awake()
    {
        float unitsPerPixel = 900f / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        Camera.main.orthographicSize = desiredHalfHeight;

        new GameContainer();

        _areas.maxValue = Enum.GetValues(typeof(Area)).Length-1;
        _opponents.maxValue = AllCars.Cars.Length - 1;
        _lengths.maxValue = Enum.GetNames(typeof(Length)).Length-1;
        SaveLoad.Load();
        Debug.Log(GameContainer.Current.BoughtCars.Length);
        boughtCars = new Car[GameContainer.Current.BoughtCars.Length];
        int cars = 0;
        for (int i = 0; i < GameContainer.Current.BoughtCars.Length; i++)
        {
            boughtCars[cars] = GameContainer.Current.BoughtCars[i];
            cars++;
            Debug.Log("car " + boughtCars[cars - 1].Name + " was added");
            currentCar = cars - 1;
        }
        _carImage.sprite = Resources.Load<Sprite>(boughtCars[currentCar].Look);
        if (currentCar == 0)
            _previousCar.interactable = false;
        else
            _previousCar.interactable = true;

        Debug.Log(GameContainer.Current.BuyCar(new RedCar(), 0));
        SetVolume();
        Instance = this;
        switch (PlayerPrefs.GetInt("InstantMenu", 0))
        {
            case 0:
                InstantMenu = false;
                _instantMenu.isOn = false;
                break;
            case 1:
                InstantMenu = true;
                _instantMenu.isOn = true;
                break;
        }
        CheckDailyGift();
        CheckDailyQuest();
    }

    private void Update()
    {
        if (wait > 0f)
        {
            wait -= Time.deltaTime;
            HideRacingWindow();
            HideWindow(_chooseWindow);
        }
        else
        {
            if (step == 1)
            {
                wait = 0f;
                step = 2;
            }
            else if (step == 2)
            {
                OpenWindow(6);
                step = 0;
            }
            else if (step == 3)
            {
                SceneManager.LoadScene("Racing");
            }

            if (Input.GetKey(KeyCode.Escape))
                CloseWindows();
            if (up)
            {
                if (_storyWindow.activeSelf && _storyWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_storyWindow, 1);
                if (_racingWindow.activeSelf && _racingWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_racingWindow, 1);
                /*if (_dailyGiftWindow.activeSelf && _dailyGiftWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_dailyGiftWindow, 1);*/
                if (_dailyQuestWindow.activeSelf && _dailyQuestWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_dailyQuestWindow, 1);
                if (_optionsWindow.activeSelf && _optionsWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_optionsWindow, 1);
                if (_chooseWindow.activeSelf && _chooseWindow.transform.position.y < Screen.height / 2f)
                    GameManager.Window(_chooseWindow, 1);
            }
            else
            {
                HideWindow(_storyWindow);
                HideWindow(_racingWindow);
                HideWindow(_dailyGiftWindow);
                HideWindow(_dailyQuestWindow);
                HideWindow(_optionsWindow);
                HideWindow(_chooseWindow);
            }
        }
    }

    public void ButtonClick(int num)
    {
        PlaySound(0);
        switch (num)
        {
            case 0: // story button
                OpenWindow(0);
                break;

            case 1: // racing button
                OpenWindow(1);
                break;

            case 2: // options button
                OpenWindow(2);
                break;

            case 3: // daily gift button
                //OpenWindow(3);
                GetDailyGift();
                break;

            case 4: // store button
                SceneManager.LoadScene("Store");
                break;

            case 5: // daily quest button
                OpenWindow(5);
                break;
        }
        PlaySound(1);
    }

    private void OpenWindow(int num)
    {
        up = true;
        _windows.SetActive(true);
        _storyWindow.SetActive(false);
        _racingWindow.SetActive(false);
        //_dailyGiftWindow.SetActive(false);
        _dailyQuestWindow.SetActive(false);
        _optionsWindow.SetActive(false);
        _chooseWindow.SetActive(false);
        switch (num)
        {
            case 0: // story window
                _storyWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _storyWindow.SetActive(true);
                break;

            case 1: // racing window
                _racingWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _racingWindow.SetActive(true);
                break;

            case 2: // options window
                _optionsWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _optionsWindow.SetActive(true);
                break;

            case 3: // daily gift window
                /*_dailyGiftWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _dailyGiftWindow.SetActive(true);*/
                break;

            case 5: // daily quest window
                _dailyQuestWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _dailyQuestWindow.SetActive(true);
                break;

            case 6: // choose Window
                _anotherShield1.color = Color.gray;
                _anotherShield2.color = Color.gray;
                _anotherShield3.color = Color.gray;
                if (GameContainer.Current.Armour >= 1)
                {
                    _oneShield.interactable = true;
                }
                if (GameContainer.Current.Armour >= 2)
                {
                    _twoShields.interactable = true;
                    _anotherShield1.color = Color.white;
                }
                if (GameContainer.Current.Armour >= 3)
                {
                    _threeShields.interactable = true;
                    _anotherShield2.color = Color.white;
                    _anotherShield3.color = Color.white;
                }
                if (GameContainer.Current.GreenNitro)
                {
                    _greenNitro.interactable = true;
                }
                if (GameContainer.Current.YellowNitro)
                {
                    _yellowNitro.interactable = true;
                }
                if (GameContainer.Current.RedNitro)
                {
                    _redNitro.interactable = true;
                }

                _chooseWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _chooseWindow.SetActive(true);
                break;
        }
    }

    public void CloseWindows()
    {
        up = false;
        PlaySound(1);
    }

    private void HideWindow(GameObject go)
    {
        if (go.activeSelf)
        {
            if (go.transform.position.y < Screen.height * 2f)
                GameManager.Window(go, 2);
            else
            {
                _windows.SetActive(false);
                _storyWindow.SetActive(false);
                _racingWindow.SetActive(false);
                _dailyGiftWindow.SetActive(false);
                _dailyQuestWindow.SetActive(false);
                _optionsWindow.SetActive(false);
                _chooseWindow.SetActive(false);
            }
        }
    }

    private void HideRacingWindow()
    {
        if (_racingWindow.activeSelf)
        {
            if (_racingWindow.transform.position.y < Screen.height * 2f)
                GameManager.Window(_racingWindow, 2);
            else
            {
                _racingWindow.SetActive(false);
            }
        }
    }

    #region Story
    // todo 
    #endregion

    #region Racing
    /// <summary>
    /// Меняет текст в определённом месте, когда переключаем слайдер
    /// </summary>
    /// <param name="which">0 - зона, 1 - оппонент, 2 - длительность</param>
    public void ChangeSlider(int which)
    {
        switch (which)
        {
            case 0:
                _captionText.text = ((Area)_areas.value).ToString();
                break;
            case 1:
                _captionText.text = AllCars.Cars[(int)_opponents.value].Name;
                break;
            case 2:
                _captionText.text = ((Length)_lengths.value).ToString();
                break;
        }
    }

    public void StartRace(bool random)
    {
        if (random)
        {
            _areas.value = UnityEngine.Random.Range(0, 2/*_areas.maxValue + 1*/);
            _opponents.value = UnityEngine.Random.Range(0, _opponents.maxValue + 1);
            _lengths.value = UnityEngine.Random.Range(0, _lengths.maxValue + 1);
            GameManager.RandomBonus = true;
        }
        else
        {
            GameManager.RandomBonus = false;
        }
        GameManager.Area = ((Area)_areas.value).ToString();
        CloseWindows();
        step = 1;
        wait = 0.1f;
        PlaySound(0);
    }

    public void HaveChosen()
    {
        PlaySound(0);
        step = 3;
        wait = 0.5f;
        if (GameManager.Health < 3)
            GameManager.Health = 3;
        GameManager.Race = 40f + _lengths.value * 10f;
        GameManager.OpponentExists = true;
        GameManager.OpponentCar = (int)_opponents.value;
        if (shields > 0)
            GameContainer.Current.AddArmour(-shields);
        shields = 0;
        if (nitro > 0)
            GameContainer.Current.AddNitro(nitro-1, false);
        nitro = 0;
        SaveLoad.Save();
        GameManager.Player = boughtCars[currentCar];
    }

    public void ChooseCar(bool next)
    {
        PlaySound(0);
        if (next)
        {
            currentCar++;
        }
        else
        {
            currentCar--;
        }
        _carImage.sprite = Resources.Load<Sprite>(boughtCars[currentCar].Look);
        if (currentCar + 1 == boughtCars.Length)
            _nextCar.interactable = false;
        else
            _nextCar.interactable = true;
        if (currentCar == 0)
            _previousCar.interactable = false;
        else
            _previousCar.interactable = true;
    }

    public void ChooseShields(int howmany)
    {
        PlaySound(0);
        GameManager.Health = 3 + howmany;
        shields = howmany;
        switch (howmany)
        {
            case 0:
                _choiceShield.localPosition = new Vector3(-200, -100, 0);
                break;
            case 1:
                _choiceShield.localPosition = new Vector3(-70, -100, 0);
                break;
            case 2:
                _choiceShield.localPosition = new Vector3(65, -100, 0);
                break;
            case 3:
                _choiceShield.localPosition = new Vector3(195, -100, 0);
                break;
        }
    }

    public void ChooseNitro(int which)
    {
        PlaySound(0);
        GameManager.Nitro = which;
        nitro = which;
        switch (which)
        {
            case 0:
                _choiceNitro.localPosition = new Vector3(-200, -100, 0);
                break;
            case 1:
                _choiceNitro.localPosition = new Vector3(-70, -100, 0);
                break;
            case 2:
                _choiceNitro.localPosition = new Vector3(70, -100, 0);
                break;
            case 3:
                _choiceNitro.localPosition = new Vector3(200, -100, 0);
                break;
        }
    }
    #endregion

    #region Options
    private void SetVolume()
    {
        MusicOn = PlayerPrefs.GetInt("Music", 1) == 1 ? true : false;
        SoundsOn = PlayerPrefs.GetInt("Sounds", 1) == 1 ? true : false;
        ImagesUpdate();
    }

    public void ChangeVolume(bool music)
    {
        PlaySound(0);
        if (music)
        {
            MusicOn = !MusicOn;
            PlayerPrefs.SetInt("Music", 1 - PlayerPrefs.GetInt("Music"));
        }
        else
        {
            SoundsOn = !SoundsOn;
            PlayerPrefs.SetInt("Sounds", 1 - PlayerPrefs.GetInt("Sounds"));
        }
        ImagesUpdate();
    }

    private void ImagesUpdate()
    {
        _musicImage.sprite = MusicOn ? _musicOn : _musicOff;
        _music.volume = MusicOn ? 1f : 0f;
        _soundsImage.sprite = SoundsOn ? _soundsOn : _soundsOff;
        _sounds.volume = SoundsOn ? 1f : 0f;
    }

    public void SwitchInstantMenu()
    {
        PlaySound(0);
        if (_instantMenu.isOn)
            PlayerPrefs.SetInt("InstantMenu", 1);
        else
            PlayerPrefs.SetInt("InstantMenu", 0);
    }
    #endregion

    /// <summary>
    /// Play certain sound
    /// </summary>
    /// <param name="n">0 - click; 1 - car drives little</param>
    private void PlaySound(int n)
    {
        _sounds.clip = sounds[n];
        _sounds.Play();
    }

    #region Daily Gift
    private void CheckDailyGift()
    {
        if (GameContainer.Current.DailyGift.Day < DateTime.Now.Day)
        {
            _dailyGiftImage.sprite = _dgbColoured;
            Debug.Log("new gift");
            _dailyGiftButton.interactable = true;
        }
        else
        {
            _dailyGiftImage.sprite = _dgbBlacknWhite;
            Debug.Log("less than a day");
            _dailyGiftButton.interactable = false;
        }
    }

    public void GetDailyGift()
    {
        if (_dailyGiftImage.sprite == _dgbColoured)
        {
            _dailyGiftImage.sprite = _dgbBlacknWhite;
            _dailyGiftButton.interactable = false;
            GameContainer.Current.AddGears(10);
            GameContainer.Current.SetDailies();
            SaveLoad.Save();
            if (!InstantMenu)
                _dailyGiftAnimator.Play("DailyGift");
        }
    }
    #endregion

    #region Daily Quest
    private void CheckDailyQuest()
    {
        if (GameContainer.Current.DailyQuest.Day < DateTime.Now.Day)
        {
            Debug.Log("new day");
            GameContainer.Current.GenerateQuest();
        }
        else
        {
            Debug.Log("same day");
            SetDailyQuest();
        }
    }

    public void SetDailyQuest()
    {
        Debug.Log(GameContainer.Current.DailyQuest + " " + GameContainer.Current.WhichQuest + " " + GameContainer.Current.WhichLevel);
    }
    // todo сделать проверку следующего дня, сделать генерацию квеста
    // Идеи квестов: использовать закись азота в течение # секунд; проехать # чистых (без трат сердец) поездок; выиграть # гонок; заработать # шестерёнок
    #endregion

    public void DeleteProgress()
    {
        SaveLoad.Delete();
    }

    public void AddCoin()
    {
        GameContainer.Current.AddCoins(1);
        GameContainer.Current.AddGears(10);
        SaveLoad.Save();
    }
}
