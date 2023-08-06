using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    public abstract string Name { get; set; }
    public abstract int Price { get; set; }
    public abstract Sprite Look { get; set; }
    public abstract float Speed { get; set; }
    public abstract float TurnSpeed { get; set; }
}
