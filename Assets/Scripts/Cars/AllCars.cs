using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllCars
{
    public static Car[] Cars = { new RedCar(), new BlueCar(), new GreenCar(), new RacingCar() }; // todo при каждом добавлении автомобилей в игру нужно добавлять их в этот массив
}
