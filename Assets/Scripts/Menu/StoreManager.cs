using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour // �� ����� ���� ����������� ������� ����������� ����������� ������ � ��������: ����� ������� ���, ���������, �� ���������; ����� ����������� �� �����������, ����, ��������, �������� ��������
{ // � ������ ������ � �������� ���� ��� ����������: ������ (������ ������ ��� ����), ��������, �������� ��������
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
        // ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� �����
        SwitchCar();
    }

    public void NextCar()
    {
        num++;
        // ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� �����
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
