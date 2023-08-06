using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override Sprite Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public BlueCar()
    {
        Name = "Blue";
        Price = 5;
        Look = Resources.Load<Sprite>("Images/Cars/Blue");
        Speed = 3f;
        TurnSpeed = 3f;
    }
}
