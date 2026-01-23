using System;
using System.Collections.Generic;

namespace LabXX_Rechtecke
{
    class Rechteck
    {
        public int Breite { get; set; }
        public int Hoehe { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Rechteck(int breite, int hoehe, int x, int y)
        {
            Breite = breite;
            Hoehe = hoehe;
            X = x;
            Y = y;          
        }

        public void Zeichnen()
        {
            for (int i = 0; i < Hoehe; i++)
            {
                for (int j = 0; j < Breite; j++)
                {
                    Console.SetCursorPosition(X + j, Y + i);

                    // Rand zeichnen
                    if (i == 0 || i == Hoehe - 1 || j == 0 || j == Breite - 1)
                        Console.Write("*");
                    else
                        Console.Write(" ");
                }
            }
        }

        public void Bewegen(int deltaX, int deltaY)
        {
            X += deltaX;
            Y += deltaY;

            // Begrenzung im Konsolenfenster
            if (X < 0) X = 0;
            if (Y < 0) Y = 0;
            if (X + Breite >= Console.WindowWidth)
                X = Console.WindowWidth - Breite - 1;
            if (Y + Hoehe >= Console.WindowHeight)
                Y = Console.WindowHeight - Hoehe - 1;
        }
    

static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();

            List<Rechteck> rechtecke = new List<Rechteck>();

            Console.Write("Wie viele Rechtecke möchten Sie zeichnen? ");
            int anzahl = int.Parse(Console.ReadLine());

            for (int i = 0; i < anzahl; i++)
            {
                Console.Clear();
                Console.WriteLine($"Rechteck {i + 1}");

                Console.Write("Breite eingeben: ");
                int breite = int.Parse(Console.ReadLine());

                Console.Write("Höhe eingeben: ");
                int hoehe = int.Parse(Console.ReadLine());

                Console.Write("Position X eingeben: ");
                int x = int.Parse(Console.ReadLine());

                Console.Write("Position Y eingeben: ");
                int y = int.Parse(Console.ReadLine());

                Rechteck r = new Rechteck(breite, hoehe, x, y);
                rechtecke.Add(r);
            }

            // Steuerung des letzten Rechtecks
            Rechteck bewegbaresRechteck = rechtecke[rechtecke.Count - 1];

            while (true)
            {
                Console.Clear();

                foreach (Rechteck r in rechtecke)
                {
                    r.Zeichnen();
                }

                ConsoleKeyInfo taste = Console.ReadKey(true);

                switch (taste.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        bewegbaresRechteck.Bewegen(0, -1);
                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        bewegbaresRechteck.Bewegen(0, 1);
                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        bewegbaresRechteck.Bewegen(-1, 0);
                        break;

                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        bewegbaresRechteck.Bewegen(1, 0);
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}