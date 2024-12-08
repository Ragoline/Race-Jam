using System;
using System.Diagnostics;

[Serializable]
public class GameContainer
{
    public GameContainer()
    {
        Current = this;
        BoughtCars = new Car[1];
        BoughtCars[0] = new RedCar();
    }

    public static GameContainer Current;

    public int Gears, Coins;
    /// <summary>
    /// ���������� ����� (����� ����� �� ������ ����� �������� ���� ����� �� 1 �� 3)
    /// </summary>
    public int Armour;
    /// <summary>
    /// ����� ����� ��������� �� ������; ��� ����� ���� � �� ����
    /// </summary>
    public bool GreenNitro, YellowNitro, RedNitro;
    /// <summary>
    /// �������, �� ������� ����������� �����
    /// </summary>
    public int Level;
    public Car[] BoughtCars;
    public DateTime DailyGift;
    public DateTime DailyQuest;
    public int WhichQuest; // 0 - play 5/10/20 races; 1 - win 3/5/8 races; 2 - spend 50/100/170 gears; 3 - spend 1/3/6 coins
    public int Completed;
    public int Goal;
    public int WhichLevel; // 0-2
    public bool SoftCurrency;

    public void Load(GameContainer gc)
    {
        Gears = gc.Gears;
        Coins = gc.Coins;
        Armour = gc.Armour;
        GreenNitro = gc.GreenNitro;
        YellowNitro = gc.YellowNitro;
        RedNitro = gc.RedNitro;
        Level = gc.Level;
        BoughtCars = gc.BoughtCars;

        Current.Gears = gc.Gears;
        Current.Coins = gc.Coins;
        Current.Armour = gc.Armour;
        Current.GreenNitro = gc.GreenNitro;
        Current.YellowNitro = gc.YellowNitro;
        Current.RedNitro = gc.RedNitro;
        Current.Level = gc.Level;
        Current.BoughtCars = gc.BoughtCars;
        Current.DailyGift = gc.DailyGift;
        Current.DailyQuest = gc.DailyQuest;
        Current.WhichQuest = gc.WhichQuest;
        Current.Completed = gc.Completed;
        Current.Goal = gc.Goal;
        Current.WhichLevel = gc.WhichLevel;
        Current.SoftCurrency = gc.SoftCurrency;
    }

    public void AddGears(int howMany)
    {
        UnityEngine.Debug.Log("add gears " + howMany);
        Current.Gears += howMany;
    }
    public int GetGears()
    {
        return Current.Gears;
    }

    public void AddCoins(int howMany)
    {
        Current.Coins += howMany;
    }
    public int GetCoins()
    {
        return Current.Coins;
    }

    public void AddArmour(int howMany)
    {
        Current.Armour += howMany;
    }
    public int GetArmour()
    {
        return Current.Armour;
    }

    public void AddNitro(int which, bool add)
    {
        switch (which)
        {
            case 0:
                Current.GreenNitro = add ? true : false;
                break;

            case 1:
                Current.YellowNitro = add ? true : false;
                break;

            case 2:
                Current.RedNitro = add ? true : false;
                break;
        }
    }
    public bool GetNitro(int which)
    {
        switch (which)
        {
            case 0:
                return Current.GreenNitro;

            case 1:
                return Current.YellowNitro;

            case 2:
                return Current.RedNitro;

            default:
                return true;
        }
    }

    public Car[] GetBoughtCars()
    {
        return Current.BoughtCars;
    }

    public bool BuyCar(Car car, int price)
    {
        //SaveLoad.Load();
        foreach (Car c in GetBoughtCars())
        {
            if (c.Name == car.Name)
            {
                UnityEngine.Debug.Log("car was bought already");
                return false;
            }
        }
        if (GetCoins() >= price)
        {
            AddCoins(-price);
            var cars = new Car[Current.BoughtCars.Length + 1];
            for (int i = 0; i < Current.BoughtCars.Length; i++)
                cars[i] = Current.BoughtCars[i];
            cars[cars.Length - 1] = car;
            Current.BoughtCars = new Car[cars.Length];
            Current.BoughtCars = cars;
            SaveLoad.Save();
            return true;
        }
        return false;
    }
    
    public void SetDailies()
    {
        DailyGift = new DateTime();
        DailyGift = DateTime.Now;
        UnityEngine.Debug.Log(DailyGift);
    }

    public void GenerateQuest()
    {
        WhichQuest = UnityEngine.Random.Range(0, 4);
        WhichLevel = UnityEngine.Random.Range(0, 3);
        SoftCurrency = UnityEngine.Random.Range(0, 2) == 0 ? false : true;
        Completed = 0;
        switch (WhichQuest)
        {
            case 0:
                switch (WhichLevel)
                {
                    case 0:
                        Goal = 5;
                        break;

                    case 2:
                        Goal = 10;
                        break;

                    case 3:
                        Goal = 20;
                        break;
                }
                break;

            case 1:
                switch (WhichLevel)
                {
                    case 0:
                        Goal = 3;
                        break;

                    case 2:
                        Goal = 5;
                        break;

                    case 3:
                        Goal = 8;
                        break;
                }
                break;

            case 2:
                switch (WhichLevel)
                {
                    case 0:
                        Goal = 50;
                        break;

                    case 2:
                        Goal = 100;
                        break;

                    case 3:
                        Goal = 200;
                        break;
                }
                break;

            case 3:
                switch (WhichLevel)
                {
                    case 0:
                        Goal = 1;
                        break;

                    case 2:
                        Goal = 3;
                        break;

                    case 3:
                        Goal = 6;
                        break;
                }
                break;
        }
        DailyQuest = new DateTime();
        DailyQuest = DateTime.Now;
        MenuManager.Instance.SetDailyQuest();
        SaveLoad.Save();
    }
}