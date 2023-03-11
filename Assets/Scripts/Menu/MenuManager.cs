using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        
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
        switch (num)
        {
            case 0: // story window
                break;

            case 1: // racing window
                break;

            case 2: // options window
                break;

            case 3: // daily gift window
                break;

            case 5: // daily quest window
                break;
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
