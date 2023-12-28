[System.Serializable]
public class RetroCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public RetroCar()
    {
        Name = "Retro";
        Price = 5;
        Look = "Images/Cars/Retro";
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
