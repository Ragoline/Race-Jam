using System;

[Serializable]
public class GameContainer
{
    public GameContainer()
    {
        Current = this;
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
    }

    public void AddGears(int howMany)
    {
        Gears += howMany;
    }
    public int GetGears()
    {
        return Gears;
    }

    public void AddCoins(int howMany)
    {
        Coins += howMany;
    }
    public int GetCoins()
    {
        return Coins;
    }

    public void AddArmour(int howMany)
    {
        Armour += howMany;
    }
    public int GetArmour()
    {
        return Armour;
    }

    public void AddNitro(int which)
    {
        switch (which)
        {
            case 0:
                GreenNitro = true;
                break;

            case 1:
                YellowNitro = true;
                break;

            case 2:
                RedNitro = true;
                break;
        }
    }
    public bool GetNitro(int which)
    {
        switch (which)
        {
            case 0:
                return GreenNitro;

            case 1:
                return YellowNitro;

            case 2:
                return RedNitro;

            default:
                return true;
        }
    }
}