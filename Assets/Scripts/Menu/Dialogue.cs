using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    private int level = 0, step = 0;
    private bool readyToTap = false;
    private float sec = 3f;

    [SerializeField] private SpriteRenderer player, dialogee, road;
    [SerializeField] private Sprite[] dialogees, players, roads;
    [SerializeField] private Image playerBubble, dialogeeBubble;
    [SerializeField] private Text playerText, dialogeeText;

    void Start()
    {
        level = MenuManager.Level;
        if (level > 4)
            level = 4;
        //level = GameContainer.Current.Level + 1;
        dialogee.sprite = dialogees[level];
        player.sprite = players[level];
        road.sprite = roads[level];

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

    private void Dialog() // todo заменить на последнем уровне зону на мост И заменить зону в туторе на мост
    {
        readyToTap = false;
        Debug.Log("dialog");
        step++;
        switch (level)
        {
            case 0:
                // диалог перед первым уровнем
                switch (step)
                {
                    case 1:
                        // первая реплика
                        playerBubble.gameObject.SetActive(true);
                        playerText.text = "Мне нужно достать денег, чтобы выкупить машину отца.";
                        break;

                    case 2:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Я тебя пристрою в гонку.";
                        break;

                    case 3:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Но ты не выиграешь никакую гонку просто со своей машиной.";
                        break;

                    case 4:
                        // третья реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Держи закись азота!";
                        break;

                    case 5:
                        // четвёртая реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Спасибо, дружище! Сейчас попробую выиграть гонку для любителей.";
                        break;

                    case 6:
                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 12;
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
                // диалог перед вторым уровнем
                switch (step)
                {
                    case 1:
                        // первая реплика
                        playerBubble.gameObject.SetActive(true);
                        playerText.text = "Ух, прохладно у вас тут.";
                        break;

                    case 2:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Хочешь жары, приезжий из города? Я тебе устрою!";
                        break;

                    case 3:
                        // третья реплика
                        /*playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Хорошо";*/

                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 4;
                        GameManager.Race = 40f;
                        GameManager.Area = Area.Winter.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new GreyCar();
                        GameManager.Health = 3;
                        GameManager.Nitro = 2;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;

                    case 4:
                        // четвёртая реплика
                        /*playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Уровень 2";*/
                        break;

                    case 5:
                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 4;
                        GameManager.Race = 40f;
                        GameManager.Area = Area.Winter.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new GreyCar();
                        GameManager.Health = 4;
                        GameManager.Nitro = 1;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;
                }
                break;

            case 2:
                // диалог перед третьим уровнем
                switch (step)
                {
                    case 1:
                        // первая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Добрейшего денёчка, гонщик! Я здесь самый кислый водила.";
                        break;

                    case 2:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Да ты просто железный лимон!";
                        break;

                    case 3:
                        // третья реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Не суди фрукт по кожуре, гонщик. Попробуй обгони меня!";
                        break;

                    case 4:
                        // четвёртая реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Легко!";
                        break;

                    case 5:
                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 6;
                        GameManager.Race = 50f;
                        GameManager.Area = Area.Village.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new CyanCar();
                        GameManager.Health = 4;
                        GameManager.Nitro = 3;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;
                }
                break;

            case 3:
                // диалог перед четвёртым уровнем
                switch (step)
                {
                    case 1:
                        // первая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Эй, красавчик, за мной не угнаться!";
                        break;

                    case 2:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "У меня здесь другие цели, принцесса.";
                        break;

                    case 3:
                        // третья реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Боюсь, на этом пляже тебе ничего не светит, если не обгонишь меня.";
                        break;

                    case 4:
                        // четвёртая реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Тогда заводись, подруга!";
                        break;

                    case 5:
                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 8;
                        GameManager.Race = 60f;
                        GameManager.Area = Area.Beach.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new LemonCar();
                        GameManager.Health = 4;
                        GameManager.Nitro = 2;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;
                }
                break;

            case 4:
                // диалог перед пятым уровнем
                switch (step)
                {
                    case 1:
                        // первая реплика
                        playerBubble.gameObject.SetActive(true);
                        playerText.text = "Время пришло.";
                        break;

                    case 2:
                        // вторая реплика
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Ты меня в жизни не обгонишь. Машина твоего отца моя!";
                        break;

                    case 3:
                        // третья реплика
                        playerBubble.gameObject.SetActive(true);
                        dialogeeBubble.gameObject.SetActive(false);
                        playerText.text = "Сегодня я её верну.";
                        break;

                    case 4:
                        // четвёртая реплика
                        /*playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(true);
                        dialogeeText.text = "Уровень 5";*/


                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 15;
                        GameManager.Race = 70f;
                        //GameManager.Area = Area.Bridge.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new PinkCar();
                        GameManager.Health = 3;
                        GameManager.Nitro = 3;
                        GameManager.Story = true;
                        //CarController.Car = new RedCar();
                        SceneManager.LoadScene("Racing");
                        break;

                    case 5:
                        // начало уровня
                        playerBubble.gameObject.SetActive(false);
                        dialogeeBubble.gameObject.SetActive(false);

                        // параметры гонки в уровне (какие машины и тд)
                        GameManager.OpponentExists = true;
                        GameManager.OpponentCar = 15;
                        GameManager.Race = 70f;
                        //GameManager.Area = Area.Bridge.ToString();
                        GameManager.RandomBonus = false;
                        GameManager.Player = new PinkCar();
                        GameManager.Health = 3;
                        GameManager.Nitro = 1;
                        GameManager.Story = true;
                        //CarController.Car = new BlackCar();
                        SceneManager.LoadScene("Racing");
                        break;
                }
                break;
        }
    }
}
