using System;
using UnityEngine;

[Serializable]
public abstract class Car
{
    public abstract string Name { get; set; }
    public abstract int Price { get; set; }
    public abstract string Look { get; set; }
    public abstract float Speed { get; set; }
    public abstract float TurnSpeed { get; set; }
}
