[System.Serializable]
public class PinkCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public PinkCar()
    {
        Name = "Pink";
        Price = 50;
        Look = "Images/Cars/Pink";
        Speed = 3f;
        TurnSpeed = 3.5f;
    }
}
