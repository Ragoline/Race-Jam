using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    private int level = 0, step = 0;
    private bool readyToTap = false;
    private float sec = 1f;

    [SerializeField] private SpriteRenderer player, dialogee;
    [SerializeField] private Sprite[] dialogees, players;
    [SerializeField] private Image playerBubble, dialogeeBubble;
    [SerializeField] private Text playerText, dialogeeText;

    void Start()
    {
        //level = GameContainer.Current.Level + 1;
        dialogee.sprite = dialogees[level];
        player.sprite = players[level];

        StartCoroutine(myCar());
    }

    IEnumerator myCar()
    {
        yield return null;
        while (player.transform.position.y < -3)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + Time.deltaTime * 2.5f);
            yield return null;
        }
        StartCoroutine(theirCar());
    }

    IEnumerator theirCar()
    {
        yield return null;
        while (dialogee.transform.position.y > 3)
        {
            dialogee.transform.position = new Vector3(dialogee.transform.position.x, dialogee.transform.position.y - Time.deltaTime * 3);
            yield return null;
        }
        // todo start the dialogue
        Dialog();
    }

    private void Update()
    {
        if (!readyToTap && sec > 0f)
            sec -= Time.deltaTime;
        if (sec <= 0f)
        {
            readyToTap = true;
            sec = 1f;
        }
        // todo if tap
        if (Input.anyKey)
        {
            if (readyToTap)
                Dialog();
        }
    }

    private void Dialog()
    {
        readyToTap = false;
        Debug.Log("dialog");
        step++;
        switch (level)
        {
            case 0:
                // todo диалог перед первым уровнем
                switch (step)
                {
                    case 1:
                        // перва€ реплика
                        playerBubble.gameObject.SetActive(true);
                        playerText.text = "Ќадоела проста€ работа, хочу в гонку!";
                        break;

                    case 2:
                        // втора€ реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Ќу, € теб€ пристрою в одну.";
                        break;

                    case 3:
                        // треть€ реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "—пасибо!";
                        break;

                    case 4:
                        // четвЄрта€ реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "ѕожалуйста.";
                        break;

                    case 5:
                        // начало уровн€
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // todo параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 0;
                        GameManager.Race = 30f;
                        GameManager.Area = Area.City.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new RedCar();
                        GameManager.Health = 3;
                        GameManager.Nitro = 1;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;
                }
                break;

            case 1:
                // todo диалог перед вторым уровнем

                break;

            case 2:
                // todo диалог перед третьим уровнем

                break;

            case 3:
                // todo диалог перед четвЄртым уровнем

                break;

            case 4:
                // todo диалог перед п€тым уровнем

                break;
        }
    }
}
