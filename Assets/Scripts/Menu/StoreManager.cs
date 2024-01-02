using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour // ������ ����� buy, �������� � ����
{
    [SerializeField] private GameObject consumablesWindow;
    [SerializeField] private GameObject shieldButton;
    [SerializeField] private GameObject greenNitroButton;
    [SerializeField] private GameObject yellowNitroButton;
    [SerializeField] private GameObject redNitroButton;
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
        UpdateButtons();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            if (consumablesWindow.activeSelf)
                consumablesWindow.SetActive(false);
            else
                Close();
    }

    public void Close()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenConsumablesWindow()
    {
        consumablesWindow.SetActive(true);
    }

    private void UpdateButtons()
    {
        if (GameContainer.Current.Gears >= 10)
        {
            shieldButton.GetComponent<Button>().interactable = true;

            if (GameContainer.Current.Gears >= 20)
            {
                if (!GameContainer.Current.GreenNitro)
                    greenNitroButton.GetComponent<Button>().interactable = true;

                if (GameContainer.Current.Gears >= 30)
                {
                    if (!GameContainer.Current.YellowNitro)
                        yellowNitroButton.GetComponent<Button>().interactable = true;

                    if (GameContainer.Current.Gears >= 40)
                    {
                        if (!GameContainer.Current.RedNitro)
                            redNitroButton.GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
    }

    public void Buy(int what)
    {
        switch (what)
        {
            case 0:
                GameContainer.Current.AddGears(-10);
                GameContainer.Current.AddArmour(1);
                gearsText.text = GameContainer.Current.GetGears() + "";
                break;

            case 1:
                GameContainer.Current.AddGears(-20);
                GameContainer.Current.AddNitro(0);
                gearsText.text = GameContainer.Current.GetGears() + "";
                break;

            case 2:
                GameContainer.Current.AddGears(-30);
                GameContainer.Current.AddNitro(1);
                gearsText.text = GameContainer.Current.GetGears() + "";
                break;

            case 3:
                GameContainer.Current.AddGears(-40);
                GameContainer.Current.AddNitro(2);
                gearsText.text = GameContainer.Current.GetGears() + "";
                break;
        }
        SaveLoad.Save();
        UpdateButtons();
    }

    public void PrevCar()
    {
        num--;
        // ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� ����� todo
        SwitchCar();
    }

    public void NextCar()
    {
        num++;
        // ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� ����� todo
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
