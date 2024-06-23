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
    [SerializeField] private AudioSource _sounds;
    [SerializeField] private AudioSource _music;
    private Car[] cars;
    private Sprite[] looks;
    private int num = 0;
    private static int BoughtFilter = 0, SpeedFilter = 0, TurningFilter = 0;
    [SerializeField] private AudioClip[] sounds = new AudioClip[2];

    private void Awake()
    {
        cars = new Car[AllCars.Cars.Length];
        looks = new Sprite[AllCars.Cars.Length];
        UpdateResources();
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
        AudioUpdate();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            if (consumablesWindow.activeSelf)
                consumablesWindow.SetActive(false);
            else
                Close();
    }

    private void UpdateResources()
    {
        gearsText.text = "" + GameContainer.Current.Gears;
        coinsText.text = "" + GameContainer.Current.Coins;
    }

    public void Close()
    {
        PlaySound(0);
        SceneManager.LoadScene("Menu");
    }

    public void OpenConsumablesWindow()
    {
        consumablesWindow.SetActive(true);
        PlaySound(0);
    }

    private void UpdateButtons()
    {
        shieldButton.GetComponent<Button>().interactable = false;
        greenNitroButton.GetComponent<Button>().interactable = false;
        yellowNitroButton.GetComponent<Button>().interactable = false;
        redNitroButton.GetComponent<Button>().interactable = false;

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
                PlaySound(0);
                break;

            case 1:
                GameContainer.Current.AddGears(-20);
                GameContainer.Current.AddNitro(0, true);
                PlaySound(0);
                break;

            case 2:
                GameContainer.Current.AddGears(-30);
                GameContainer.Current.AddNitro(1, true);
                PlaySound(0);
                break;

            case 3:
                GameContainer.Current.AddGears(-40);
                GameContainer.Current.AddNitro(2, true);
                PlaySound(0);
                break;
        }
        UpdateResources();
        SaveLoad.Save();
        UpdateButtons();
    }

    public void PrevCar()
    {
        num--;
        PlaySound(0);
        // todo ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� ����� todo
        SwitchCar();
    }

    public void NextCar()
    {
        num++;
        PlaySound(0);
        // ������ ������ ��������, ��� ���������� ������ ������� �����, � ����� ��������� ����� todo
        SwitchCar();
    }

    private void SwitchCar()
    {
        carImage.sprite = looks[num];
        speedSlider.value = cars[num].Speed / 4f;
        turningSlider.value = cars[num].TurnSpeed / 4f;
        nameText.text = cars[num].Name;
        priceText.text = "" + cars[num].Price;

        for (int i = 0; i < GameContainer.Current.BoughtCars.Length; i++)
            if (GameContainer.Current.BoughtCars[i].Name == cars[num].Name)
            {
                buyButton.interactable = false;
                buyText.text = "Bought";
                break;
            }
            else
            {
                buyButton.interactable = true;
                buyText.text = "Buy";
            }

        prevButton.interactable = num == 0 ? false : true;
        nextButton.interactable = num == (cars.Length - 1) ? false : true;
    }

    public void BuyCar()
    {
        if (GameContainer.Current.BuyCar(cars[num], cars[num].Price))
        {
            Debug.Log("bought!");
            PlaySound(1);
            UpdateResources();

            for (int i = 0; i < GameContainer.Current.BoughtCars.Length; i++)
                if (GameContainer.Current.BoughtCars[i].Name == cars[num].Name)
                {
                    buyButton.interactable = false;
                    buyText.text = "Bought";
                    break;
                }
                else
                {
                    buyButton.interactable = true;
                    buyText.text = "Buy";
                }
        }
    }

    private void PlaySound(int n)
    {
        _sounds.clip = sounds[n];
        _sounds.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="which">0 - cancel all; 1 - bought; 2 - speed; 3 - turning</param>
    public void SetFilter(int which)
    {
        int j = 0;
        num = 0;
        switch (which)
        {
            case 0:
                for (int i = 0; i < AllCars.Cars.Length; i++)
                {
                    cars[i] = AllCars.Cars[i];
                    looks[i] = Resources.Load<Sprite>(cars[i].Look);
                }
                break;

            case 1:
                int k = 0;
                foreach (Car c in AllCars.Cars)
                {
                    foreach (Car car in GameContainer.Current.BoughtCars)
                        if (c.Name == car.Name)
                        {
                            cars[j] = AllCars.Cars[k];
                            looks[j] = Resources.Load<Sprite>(cars[k].Look);
                            j++;
                        }
                    k++;
                }
                Debug.Log(j);
                k = 0;
                foreach (Car c in AllCars.Cars)
                {
                    bool ok = true;
                    foreach (Car car in GameContainer.Current.BoughtCars)
                        if (c.Name == car.Name)
                        {
                            ok = false;
                        }
                    if (ok)
                    {
                        cars[j] = AllCars.Cars[k];
                        looks[j] = Resources.Load<Sprite>(cars[k].Look);
                        j++;
                    }
                    k++;
                    Debug.Log(k + " " + c.Name + " " + ok);
                }
                break;

            case 2:
                break;

            case 3:
                break;
        }
        SwitchCar();
    }

    private void AudioUpdate()
    {
        _music.volume = MenuManager.MusicOn ? 1f : 0f;
        _sounds.volume = MenuManager.MusicOn ? 1f : 0f;
    }
}
