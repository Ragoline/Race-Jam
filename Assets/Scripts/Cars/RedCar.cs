using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override string Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public RedCar()
    {
        Name = "Red";
        Price = 0;
        Look = "Images/Cars/Red";
        Speed = 1f;
        TurnSpeed = 1f;
    }
}
