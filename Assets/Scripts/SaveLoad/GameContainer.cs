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
    /// Количество брони (игрок может на каждую гонку набирать себе броню от 1 до 3)
    /// </summary>
    public int Armour;
    /// <summary>
    /// нитро можно содержать по одному; они могут быть и не быть
    /// </summary>
    public bool GreenNitro, YellowNitro, RedNitro;
    /// <summary>
    /// Уровень, на котором остановился игрок
    /// </summary>
    public int Level;
    public Car[] BoughtCars;
    public DateTime DailyGift;

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
}