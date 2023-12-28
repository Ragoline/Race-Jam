[System.Serializable]
public class FuturisticCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public FuturisticCar()
    {
        Name = "Futuristic";
        Price = 5;
        Look = "Images/Cars/Futuristic";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
