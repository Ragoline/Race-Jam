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
        Price = 5;
        Look = "Images/Cars/Orange";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
