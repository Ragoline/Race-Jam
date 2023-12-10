using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour // менять текст buy, название и цену
{
    [SerializeField] private Image carImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text buyText;
    [SerializeField] private Text gearsText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider turningSlider;
    private Car[] cars;
    private Sprite[] looks;
    private int num = 0;

    private void Awake()
    {
        cars = new Car[AllCars.Cars.Length];
        looks = new Sprite[AllCars.Cars.Length];
        gearsText.text = "" + GameContainer.Current.Gears;
        coinsText.text = "" + GameContainer.Current.Coins;
    }

    private void Start()
    {
        for (int i = 0; i < AllCars.Cars.Length; i++)
        {
            cars[i] = AllCars.Cars[i];
            looks[i] = Resources.Load<Sprite>(cars[i].Look);
        }
        SwitchCar();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    public void PrevCar()
    {
        num--;
        // внешне делать анимацию, как предыдущая машина уезжает вверх, а новая приезжает снизу
        SwitchCar();
    }

    public void NextCar()
    {
        num++;
        // внешне делать анимацию, как предыдущая машина уезжает вверх, а новая приезжает снизу
        SwitchCar();
    }

    private void SwitchCar()
    {
        carImage.sprite = looks[num];
        speedSlider.value = cars[num].Speed / 10f;
        turningSlider.value = cars[num].TurnSpeed / 10f;
        nameText.text = cars[num].Name;
        priceText.text = "" + cars[num].Price;

        for (int i = 0; i < GameContainer.Current.BoughtCars.Length; i++)
            if (GameContainer.Current.BoughtCars[i].Name == cars[num].Name)
            {
                buyButton.interactable = false;
                buyText.text = "Bought";
            }
            else
            {
                buyButton.interactable = true;
                buyText.text = "Buy";
            }

        prevButton.interactable = num == 0 ? false : true;
        nextButton.interactable = num == (cars.Length - 1) ? false : true;
    }
}
