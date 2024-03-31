[System.Serializable]
public class SportCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public SportCar()
    {
        Name = "Sport";
        Price = 60;
        Look = "Images/Cars/Sport";
        Speed = 3.5f;
        TurnSpeed = 3f;
    }
}
