using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentals.Constructors;

public class Person(string name, int age, string role)
{
    public string Name { get; } = name;
    public int Age { get; } = age;
    public string Role { get; } = role;

    public Person(string name, int age)
        : this(name, age, "Guest") { }

    public Person(string name)
        : this(name, 0) { }

    public static Person CreateAdmin(string name) =>
        new(name, 0, "Admin");

    public override string ToString() =>
        $"{Name} ({Age}) [{Role}]";
}