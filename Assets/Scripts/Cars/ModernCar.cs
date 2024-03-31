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
        Price = 30;
        Look = "Images/Cars/Modern";
        Speed = 3.2f;
        TurnSpeed = 2.5f;
    }
}
