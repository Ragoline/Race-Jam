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
        Price = 5;
        Look = "Images/Cars/Purple";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
