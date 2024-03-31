[System.Serializable]
public class PurpleCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public PurpleCar()
    {
        Name = "Purple";
        Price = 35;
        Look = "Images/Cars/Purple";
        Speed = 3.1f;
        TurnSpeed = 3.2f;
    }
}
