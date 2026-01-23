using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04_Bruchrechnung
{

    internal class Program
    {
        static void Main(string[] args)
        {
            // Eingabe: 67 / 69

            Console.Write("Bitte Bruch eingeben: ");
            string line1 = Console.ReadLine();
            Console.Write("Bitte Bruch eingeben: ");
            string line2 = Console.ReadLine();

            Bruch b1 = Bruch.Parse(line1);
            Bruch b2 = Bruch.Parse(line2);       
            b1.Add(b2);

            Console.WriteLine($"Gekuerzter Bruch: {b1}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
