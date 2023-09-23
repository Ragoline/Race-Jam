using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour // по итогу надо обязательно сделать возможность фильтровать машины в магазине: можно выбрать все, купленные, не купленные; можно упорядочить по купленности, цене, скорости, скорости поворота
{ // у каждой машины в принципе есть три показателя: внешка (каждый решает для себя), скорость, скорость поворота
    [SerializeField] private Image carImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text buyText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider turningSlider;
    private Car[] cars;
    private Sprite[] looks;
    private int num = 0;

    private void Awake()
    {
        cars = new Car[AllCars.Cars.Length];
        looks = new Sprite[AllCars.Cars.Length];
    }

    private void Start()
    {
        for (int i = 0; i < AllCars.Cars.Length; i++)
        {
            looks[i] = Resources.Load<Sprite>(cars[i].Look);
        }
        SwitchCar();
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
        // speedSlider
        // turningSlider

        prevButton.interactable = num == 0 ? false : true;
        nextButton.interactable = num == (cars.Length-1) ? false : true;
    }
}
