using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentals.Abstract;

public abstract class Shape
{
    public string Color { get; set; } = "white";

    // Subclasses MUST implement these
    public abstract double Area();
    public abstract double Perimeter();

    // Shared logic — all shapes get this for free
    public string Describe() =>
        $"{GetType().Name}: area={Area():F2}, perimeter={Perimeter():F2}";
}

public class Circle(double radius) : Shape
{
    public double Radius { get; } = radius;

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}

public class Rectangle(double w, double h) : Shape
{
    public double W { get; } = w; public double H { get; } = h;

    public override double Area() => W * H;
    public override double Perimeter() => 2 * (W + H);
}

public interface ISaveable
{
    void Save(string path);
    bool Load(string path);
}

public interface IPrintable
{
    void Print();
    string Preview();
}

public class Document : ISaveable, IPrintable
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";

    public void Save(string path) =>
        Console.WriteLine($"Saving '{Title}' to {path}");

    public bool Load(string path)
    {
        Console.WriteLine($"Loading from {path}");
        return true;
    }

    public void Print() =>
        Console.WriteLine($"--- {Title} ---\n{Body}");

    public string Preview() =>
        Body.Length > 50 ? Body[..50] + "..." : Body;
}