using System;

namespace DesignPatternsDemo
{
    // =========================================================
    // 1. SINGLETON
    // =========================================================
    // Namen: obstaja samo en objekt (npr. Printer)
    class Printer
    {
        private static Printer _instance = new Printer();

        public static Printer Instance => _instance;

        private Printer() { }

        public void Print(string text)
        {
            Console.WriteLine("[Printer] " + text);
        }
    }

    // =========================================================
    // 2. FACTORY
    // =========================================================
    // Namen: ustvarjanje objektov preko metode
    class Animal { }

    class Dog : Animal { }
    class Cat : Animal { }

    class AnimalFactory
    {
        public static Animal Create(string type)
        {
            if (type == "dog") return new Dog();
            if (type == "cat") return new Cat();
            return null;
        }
    }

    // =========================================================
    // 3. ABSTRACT FACTORY
    // =========================================================
    // Namen: ustvarjanje družine povezanih objektov

    interface IButton
    {
        void Render();
    }

    class WindowsButton : IButton
    {
        public void Render() => Console.WriteLine("Windows gumb");
    }

    class MacButton : IButton
    {
        public void Render() => Console.WriteLine("Mac gumb");
    }

    interface IGUIFactory
    {
        IButton CreateButton();
    }

    class WindowsFactory : IGUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
    }

    class MacFactory : IGUIFactory
    {
        public IButton CreateButton() => new MacButton();
    }

    // =========================================================
    // 4. BUILDER
    // =========================================================
    // Namen: gradnja objekta po korakih

    class Car
    {
        public string Engine;
        public string Color;
    }

    class CarBuilder
    {
        private Car car = new Car();

        public CarBuilder SetEngine(string engine)
        {
            car.Engine = engine;
            return this;
        }

        public CarBuilder SetColor(string color)
        {
            car.Color = color;
            return this;
        }

        public Car Build()
        {
            return car;
        }
    }

    // =========================================================
    // 5. FLUENT INTERFACE
    // =========================================================
    // Namen: verižni klici metod (lep API)

    class Person
    {
        public string Name;
        public int Age;

        public Person SetName(string name)
        {
            Name = name;
            return this;
        }

        public Person SetAge(int age)
        {
            Age = age;
            return this;
        }

        public Person Print()
        {
            Console.WriteLine($"Ime: {Name}, Starost: {Age}");
            return this;
        }
    }

    // =========================================================
    // 6. PROTOTYPE
    // =========================================================
    // Namen: kopiranje obstoječega objekta

    class CarPrototype
    {
        public string Model;

        public CarPrototype Clone()
        {
            return new CarPrototype
            {
                Model = this.Model
            };
        }
    }

    // =========================================================
    // MAIN (DEMO)
    // =========================================================
    class Program
    {
        static void Main(string[] args)
        {
            // Singleton
            Printer.Instance.Print("Hello Singleton");

            // Factory
            Animal a = AnimalFactory.Create("dog");

            // Abstract Factory
            IGUIFactory factory = new WindowsFactory();
            IButton button = factory.CreateButton();
            button.Render();

            // Builder
            Car car = new CarBuilder()
                .SetEngine("V8")
                .SetColor("Red")
                .Build();

            Console.WriteLine($"Car: {car.Engine}, {car.Color}");

            // Fluent
            new Person()
                .SetName("Ana")
                .SetAge(20)
                .Print();

            // Prototype
            CarPrototype c1 = new CarPrototype { Model = "BMW" };
            CarPrototype c2 = c1.Clone();

            Console.WriteLine("Clone model: " + c2.Model);
        }
    }
}