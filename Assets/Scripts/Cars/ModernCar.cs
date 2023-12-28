[System.Serializable]
public class ModernCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public ModernCar()
    {
        Name = "Modern";
        Price = 5;
        Look = "Images/Cars/Modern";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
