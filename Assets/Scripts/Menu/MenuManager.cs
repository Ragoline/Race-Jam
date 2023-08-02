using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _windows;
    [SerializeField] private GameObject _storyWindow;

    [SerializeField] private GameObject _racingWindow;
    [SerializeField] private Slider _areas;
    [SerializeField] private Slider _opponents;
    [SerializeField] private Slider _lengths;

    [SerializeField] private GameObject _dailyGiftWindow;
    [SerializeField] private GameObject _dailyQuestWindow;
    [SerializeField] private GameObject _optionsWindow;
    [SerializeField] private Slider _soundsVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Toggle _instantMenu;

    private bool up = false;
    public static int MusicVolume;
    public static int SoundsVolume;
    public static MenuManager Instance;
    public static bool InstantMenu;

    private void Awake()
    {
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
        _areas.maxValue = Enum.GetValues(typeof(Area)).Length; // todo doesnt work FIX!
        _opponents.maxValue = Enum.GetNames(typeof(Opponent)).Length;
        _lengths.maxValue = Enum.GetNames(typeof(Length)).Length;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            CloseWindows();
        if (up)
        {
            if (_storyWindow.activeSelf && _storyWindow.transform.position.y < Screen.height / 2f)
                GameManager.Window(_storyWindow, 1);
            if (_racingWindow.activeSelf && _racingWindow.transform.position.y < Screen.height / 2f)
                GameManager.Window(_racingWindow, 1);
            if (_dailyGiftWindow.activeSelf && _dailyGiftWindow.transform.position.y < Screen.height / 2f)
                GameManager.Window(_dailyGiftWindow, 1);
            if (_dailyQuestWindow.activeSelf && _dailyQuestWindow.transform.position.y < Screen.height / 2f)
                GameManager.Window(_dailyQuestWindow, 1);
            if (_optionsWindow.activeSelf && _optionsWindow.transform.position.y < Screen.height / 2f)
                GameManager.Window(_optionsWindow, 1);
        }
        else
        {
            HideWindow(_storyWindow); // todo сделать мгновенное появление меню-машины, если стоит instant menu
            HideWindow(_racingWindow);
            HideWindow(_dailyGiftWindow);
            HideWindow(_dailyQuestWindow);
            HideWindow(_optionsWindow);
        }
    }

    public void ButtonClick(int num)
    {
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
                OpenWindow(3);
                break;

            case 4: // store button
                SceneManager.LoadScene("Store");
                break;

            case 5: // daily quest button
                OpenWindow(5);
                break;
        }
    }

    private void OpenWindow(int num)
    {
        up = true;
        _windows.SetActive(true);
        _storyWindow.SetActive(false);
        _racingWindow.SetActive(false);
        _dailyGiftWindow.SetActive(false);
        _dailyQuestWindow.SetActive(false);
        _optionsWindow.SetActive(false);
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
                _dailyGiftWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _dailyGiftWindow.SetActive(true);
                break;

            case 5: // daily quest window
                _dailyQuestWindow.transform.position = new Vector2(Screen.width / 2, Screen.height * (up ? -1 : 1));
                _dailyQuestWindow.SetActive(true);
                break;
        }
    }

    public void CloseWindows()
    {
        up = false;
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
            }
        }
    }

    #region Story
    // todo 
    #endregion

    #region Racing
    // todo сделать в окне выбор гонки (скорость врага и местность) и кнопку старта, а также кнопку старта случайной гонки

    #endregion

    #region Options
    // todo сделать кнопки переключения громкостей
    private void SetVolume()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            MusicVolume = PlayerPrefs.GetInt("Music", 10);
            _musicVolume.value = MusicVolume;
            SoundsVolume = PlayerPrefs.GetInt("Sounds", 10);
            _soundsVolume.value = SoundsVolume;
        }
        else
        {
            MusicVolume = PlayerPrefs.GetInt("Music", 10);
            _musicVolume.value = MusicVolume;
            SoundsVolume = PlayerPrefs.GetInt("Sounds", 10);
            _soundsVolume.value = SoundsVolume;
        }
    }

    public void ChangeVolume(bool music)
    {
        if (music)
        {
            MusicVolume = (int)_musicVolume.value;
            PlayerPrefs.SetInt("Music", MusicVolume);
        }
        else
        {
            SoundsVolume = (int)_soundsVolume.value;
            PlayerPrefs.SetInt("Sounds", SoundsVolume);
        }
    }

    public void SwitchInstantMenu()
    {
        if (_instantMenu.isOn)
            PlayerPrefs.SetInt("InstantMenu", 1);
        else
            PlayerPrefs.SetInt("InstantMenu", 0);
    }
    #endregion

    #region Daily Gift
    // todo сделать проверку, когда игрок последний раз забирал подарок, и выдавать ему его при клике на соответствующую кнопку
    #endregion

    #region Daily Quest
    // todo сделать проверку следующего дня, сделать генерацию квеста
    // Идеи квестов: использовать закись азота в течение # секунд; проехать # чистых (без трат сердец) поездок; выиграть # гонок; заработать # шестерёнок
    #endregion
}
