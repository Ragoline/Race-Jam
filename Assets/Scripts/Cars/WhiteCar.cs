[System.Serializable]
public class WhiteCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public WhiteCar()
    {
        Name = "White";
        Price = 15;
        Look = "Images/Cars/White";
        Speed = 1.5f;
        TurnSpeed = 2.5f;
    }
}
