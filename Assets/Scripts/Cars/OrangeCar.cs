[System.Serializable]
public class OrangeCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public OrangeCar()
    {
        Name = "Orange";
        Price = 3;
        Look = "Images/Cars/Orange";
        Speed = 1.8f;
        TurnSpeed = 1.2f;
    }
}
