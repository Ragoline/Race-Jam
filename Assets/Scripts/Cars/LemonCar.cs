[System.Serializable]
public class LemonCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public LemonCar()
    {
        Name = "Lemon";
        Price = 5;
        Look = "Images/Cars/Lemon";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
