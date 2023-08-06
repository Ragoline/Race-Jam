using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCar : Car
{
    public override string Name { get; set; }
    public override int Price { get; set; }
    public override Sprite Look { get; set; }
    public override float Speed { get; set; }
    public override float TurnSpeed { get; set; }

    public GreenCar()
    {
        Name = "Green";
        Price = 10;
        Look = Resources.Load<Sprite>("Images/Cars/Green");
        Speed = 5f;
        TurnSpeed = 5f;
    }
}
