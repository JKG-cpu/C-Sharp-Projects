using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentals.Properties;

public class Temperature
{
    private double _celsius;

    public double Celsius
    {
        get => _celsius;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, -273.15);
            _celsius = value;
        }
    }

    public double Fahrenheit => _celsius * 9 / 5 + 32;

    public string Label { get; set; } = "Room Temp";
    public string Unit { get; set; } = "ºC";
}