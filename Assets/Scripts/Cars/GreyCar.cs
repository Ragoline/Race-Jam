[System.Serializable]
public class GreyCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public GreyCar()
    {
        Name = "Grey";
        Price = 2;
        Look = "Images/Cars/Grey";
        Speed = 1.3f;
        TurnSpeed = 1.5f;
    }
}
