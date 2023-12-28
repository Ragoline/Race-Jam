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
        Price = 5;
        Look = "Images/Cars/White";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
