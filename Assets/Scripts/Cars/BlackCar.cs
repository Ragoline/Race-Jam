[System.Serializable]
public class BlackCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public BlackCar()
    {
        Name = "Black";
        Price = 66;
        Look = "Images/Cars/Black";
        Speed = 3.08f;
        TurnSpeed = 3.3f;
    }
}
