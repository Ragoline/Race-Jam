[System.Serializable]
public class CyanCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public CyanCar()
    {
        Name = "Cyan";
        Price = 10;
        Look = "Images/Cars/Cyan";
        Speed = 1.8f;
        TurnSpeed = 2.2f;
    }
}
