using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _windows;
    [SerializeField] private GameObject _storyWindow;
    [SerializeField] private GameObject _racingWindow;
    [SerializeField] private GameObject _dailyGiftWindow;
    [SerializeField] private GameObject _dailyQuestWindow;
    [SerializeField] private GameObject _optionsWindow;

    private bool up = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            CloseWindows();
        if (up)
        {
            if (_storyWindow.activeSelf && _storyWindow.transform.position.y < Screen.height / 2f)
                _storyWindow.transform.position = new Vector2(Screen.width / 2, _storyWindow.transform.position.y + 10f);
            if (_racingWindow.activeSelf && _racingWindow.transform.position.y < Screen.height / 2f)
                _racingWindow.transform.position = new Vector2(Screen.width / 2, _racingWindow.transform.position.y + 10f);
            if (_dailyGiftWindow.activeSelf && _dailyGiftWindow.transform.position.y < Screen.height / 2f)
                _dailyGiftWindow.transform.position = new Vector2(Screen.width / 2, _dailyGiftWindow.transform.position.y + 10f);
            if (_dailyQuestWindow.activeSelf && _dailyQuestWindow.transform.position.y < Screen.height / 2f)
                _dailyQuestWindow.transform.position = new Vector2(Screen.width / 2, _dailyQuestWindow.transform.position.y + 10f);
            if (_optionsWindow.activeSelf && _optionsWindow.transform.position.y < Screen.height / 2f)
                _optionsWindow.transform.position = new Vector2(Screen.width / 2, _optionsWindow.transform.position.y + 10f);
        }
        else
        {
            HideWindow(_storyWindow);
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
                go.transform.position = new Vector2(Screen.width / 2, go.transform.position.y + 20f);
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
    #endregion

    #region Racing
    #endregion

    #region Options
    #endregion

    #region Daily Gift
    #endregion

    #region Daily Quest
    #endregion
}
