using Fundamentals.Abstract;
using Fundamentals.Constructors;
using Fundamentals.Enum;
using Fundamentals.Properties;

namespace Fundamentals;

public static class Program
{
    private static readonly string SepString = "\n" + new string('-', 40) + "\n";

    public static void Main()
    {
        Enum();
        Properties();
        Constructors();
        Abstract();
    }

    public static void Enum()
    {
        // Directions
        Console.WriteLine("Enum - Directions\n");

        Directions dir = Directions.North;
        string directionString = DirectionsExtensions.GetDirectionString(dir);
        string oppositeDirectionString = DirectionsExtensions.GetDirectionString(DirectionsExtensions.GetOppositeDirection(dir));
        Console.WriteLine($"Starting direction: {directionString}.");
        Console.WriteLine($"Opposite direction: {oppositeDirectionString}.");

        Console.WriteLine(SepString);

        // Days of the week
        Console.WriteLine("Enum - Days of the week\n");

        DaysOfTheWeek friday = DaysOfTheWeek.Fri;
        DaysOfTheWeek sunday = DaysOfTheWeek.Sun;

        Console.WriteLine(DaysOfTheWeekExtensions.IsWeekDay(friday) ? 
            $"{friday} is a weekday." : $"{friday} is not a weekday."
        );

        Console.WriteLine(DaysOfTheWeekExtensions.IsWeekDay(sunday) ?
            $"{sunday} is a weekday." : $"{sunday} is not a weekday.");

        Console.WriteLine(SepString);
    }

    public static void Properties()
    {
        Console.WriteLine("Properties - Temperature\n");

        // Temperature
        Temperature t = new() { Celsius = 100, Unit = "ºC" };
        Console.WriteLine(t.Fahrenheit);

        Console.WriteLine(SepString);
    }

    public static void Constructors()
    {
        Console.WriteLine("Constructors - Person\n");

        var p1 = new Person("Alice", 30, "Admin");
        var p2 = new Person("Bob", 25);
        var p3 = new Person("Carol");
        var p4 = Person.CreateAdmin("Dave");

        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        Console.WriteLine(p4);

        Console.WriteLine(SepString);
    }

    public static void Abstract()
    {
        Console.WriteLine("Abstract - Shapes\n");

        List<Shape> shapes = [new Circle(5), new Rectangle(3, 4)];
        foreach (var s in shapes)
            Console.WriteLine(s.Describe());

        Console.WriteLine(SepString);

        Console.WriteLine("Abstract / Interface\n");

        void SaveDoc(ISaveable s) => s.Save("./out.txt");
        void PrintDoc(IPrintable p) => p.Print();

        var doc = new Document { Title = "Notes", Body = "Hello world" };
        SaveDoc(doc);
        PrintDoc(doc);

        Console.WriteLine(SepString);
    }
}