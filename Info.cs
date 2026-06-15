using System;
using System.Collections.Generic;
using System.Linq;

namespace MegaMasterCheatSheet
{
    // =========================================================
    // 🧠 DEDOVANJE (INHERITANCE)
    // =========================================================
    // ✔ en razred podeduje drugega
    // ✔ ponovna uporaba kode

    class Vozilo
    {
        public string Znamka { get; set; }

        public void Vozim()
        {
            Console.WriteLine("Vozim vozilo");
        }
    }

    class Avto : Vozilo
    {
        public int StVrata { get; set; }

        public void Hupam()
        {
            Console.WriteLine("Beep beep");
        }
    }

    // =========================================================
    // 🧠 ABSTRACT CLASS
    // =========================================================
    // ✔ osnova za druge razrede
    // ✔ ne moreš narediti new objekta
    // ✔ lahko ima implementacijo + abstraktne metode

    abstract class Oseba
    {
        public string Ime { get; set; }

        public void PredstaviSe()
        {
            Console.WriteLine("Jaz sem " + Ime);
        }

        public abstract void Delo();
    }

    class Student : Oseba
    {
        public override void Delo()
        {
            Console.WriteLine("Študiram");
        }
    }

    class Profesor : Oseba
    {
        public override void Delo()
        {
            Console.WriteLine("Predavam");
        }
    }

    // =========================================================
    // 🧠 INTERFACE
    // =========================================================
    // ✔ pogodba (kaj mora razred znati)

    interface IPredava
    {
        void Predavaj();
    }

    class Asistent : IPredava
    {
        public void Predavaj()
        {
            Console.WriteLine("Vodim vaje");
        }
    }

    // =========================================================
    // 🧠 EXTENSION METHODS
    // =========================================================
    // ✔ dodamo nove metode obstoječim tipom (npr. string)

    static class StringExtensions
    {
        public static void Pozdravi(this string s)
        {
            Console.WriteLine("Živjo " + s);
        }

        public static bool JeDolga(this string s)
        {
            return s.Length > 5;
        }
    }

    // =========================================================
    // 🧠 LINQ (QUERY SYNTAX)
    // =========================================================
    // ✔ SQL-podoben zapis

    class LinqDemo
    {
        public static void Run()
        {
            List<int> stevila = new() { 1, 2, 3, 4, 5, 6 };

            var rezultat =
                from x in stevila
                where x > 3
                orderby x descending
                select x;

            Console.WriteLine("LINQ query syntax:");
            foreach (var x in rezultat)
                Console.WriteLine(x);
        }
    }

    // =========================================================
    // 🧠 LINQ (METHOD SYNTAX)
    // =========================================================
    // ✔ funkcijski zapis (extension methods)

    class LinqMethodDemo
    {
        public static void Run()
        {
            List<int> stevila = new() { 1, 2, 3, 4, 5, 6 };

            var rezultat = stevila
                .Where(x => x > 3)
                .OrderByDescending(x => x)
                .Select(x => x);

            Console.WriteLine("LINQ method syntax:");
            foreach (var x in rezultat)
                Console.WriteLine(x);
        }
    }

    // =========================================================
    // 🧠 DELEGATE
    // =========================================================
    // ✔ shrani funkcijo v spremenljivko

    delegate void MojDelegate(string text);

    class DelegateDemo
    {
        public static void Pozdravi(string ime)
        {
            Console.WriteLine("Živjo " + ime);
        }

        public static void Run()
        {
            MojDelegate d = Pozdravi;
            d("Ana");
        }
    }

    // =========================================================
    // 🧠 LAMBDA + FUNC + ACTION
    // =========================================================
    // ✔ kratke anonimne funkcije
    // ✔ uporabljamo z delegati + LINQ

    class LambdaDemo
    {
        public static void Run()
        {
            // -------------------------
            // ACTION (ne vrača nič)
            // -------------------------
            Action<string> pozdravi = ime =>
            {
                Console.WriteLine("Živjo " + ime);
            };

            pozdravi("Marko");

            // -------------------------
            // FUNC (vrača rezultat)
            // -------------------------
            Func<int, int, int> sestej = (a, b) => a + b;

            Console.WriteLine(sestej(2, 3));

            // -------------------------
            // LAMBDA v LINQ
            // -------------------------
            List<int> stevila = new() { 1, 2, 3, 4, 5 };

            var rezultat = stevila
                .Where(x => x > 3);

            foreach (var x in rezultat)
                Console.WriteLine(x);
        }
    }

    // =========================================================
    // 🚀 MAIN (DEMO)
    // =========================================================
    class Program
    {
        static void Main(string[] args)
        {
            // =========================
            // DEDOVANJE
            // =========================
            Avto a = new Avto
            {
                Znamka = "BMW",
                StVrata = 4
            };
            a.Vozim();
            a.Hupam();

            // =========================
            // ABSTRACT CLASS
            // =========================
            Oseba o = new Student { Ime = "Ana" };
            o.PredstaviSe();
            o.Delo();

            // =========================
            // INTERFACE
            // =========================
            IPredava p = new Asistent();
            p.Predavaj();

            // =========================
            // EXTENSIONS
            // =========================
            "Marko".Pozdravi();
            Console.WriteLine("Hello".JeDolga());

            // =========================
            // LINQ
            // =========================
            LinqDemo.Run();
            LinqMethodDemo.Run();

            // =========================
            // DELEGATE
            // =========================
            DelegateDemo.Run();

            // =========================
            // LAMBDA
            // =========================
            LambdaDemo.Run();
        }
    }
}