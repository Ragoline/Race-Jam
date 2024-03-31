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
        Price = 40;
        Look = "Images/Cars/Retro";
        Speed = 2.5f;
        TurnSpeed = 3.5f;
    }
}
