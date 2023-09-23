[System.Serializable]
public class RacingCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public RacingCar()
    {
        Name = "Racing";
        Price = 100;
        Look = "Images/Cars/Racing";
        Speed = 10f;
        TurnSpeed = 10f;
    }
}
