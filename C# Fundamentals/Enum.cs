namespace Fundamentals.Enum;

// Simple Directions with Enum
public static class DirectionsExtensions
{
    public static Directions GetOppositeDirection(Directions dir)
    {
        return dir switch
        {
            Directions.North => Directions.South,
            Directions.South => Directions.North,
            Directions.East => Directions.West,
            Directions.West => Directions.East,
            _ => throw new ArgumentOutOfRangeException(nameof(dir))
        };
    }

    public static string GetDirectionString(Directions dir)
    {
        return dir switch
        {
            Directions.North => "North",
            Directions.South => "South",
            Directions.East => "East",
            Directions.West => "West",
            _ => throw new ArgumentOutOfRangeException(nameof(dir))
        };
    }
}

public enum Directions
{
    North,
    South,
    East,
    West
}

// Days of the Week
public static class DaysOfTheWeekExtensions
{
    private static readonly HashSet<DaysOfTheWeek> WeekDays = new() 
    { 
        DaysOfTheWeek.Mon, 
        DaysOfTheWeek.Tues, 
        DaysOfTheWeek.Wed, 
        DaysOfTheWeek.Thurs, 
        DaysOfTheWeek.Fri 
    };
    public static bool IsWeekEnd(DaysOfTheWeek day) => day == DaysOfTheWeek.Sun || day == DaysOfTheWeek.Sat;
    public static bool IsWeekDay(DaysOfTheWeek day) => WeekDays.Contains(day);
}

public enum DaysOfTheWeek
{
    Sun, Mon, Tues, Wed, Thurs, Fri, Sat
}