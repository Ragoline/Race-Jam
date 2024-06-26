[System.Serializable]
public class GreenCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public GreenCar()
    {
        Name = "Green";
        Price = 25;
        Look = "Images/Cars/Green";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
