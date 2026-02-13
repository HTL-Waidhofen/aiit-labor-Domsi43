Lab05_Rechtecke_zeichnen\Program.cs
using System;
using System.Collections.Generic;

namespace LabXX_Rechtecke
{
    using System;
    using System.Collections.Generic;

    namespace LabXX_Rechtecke
    {
        class Rechteck
        {
            public int Breite { get; private set; }
            public int Hoehe { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

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

                        if (i == 0 || i == Hoehe - 1 || j == 0 || j == Breite - 1)
                            Console.Write("*");
                        else
                            Console.Write(" ");
                    }
                }
            }

            public void Bewegen(int deltaX, int deltaY, List<Rechteck> alleRechtecke)
            {
                int neueX = X + deltaX;
                int neueY = Y + deltaY;

                // Bildschirmbegrenzung
                if (neueX < 0 || neueY < 0 ||
                    neueX + Breite >= Console.WindowWidth ||
                    neueY + Hoehe >= Console.WindowHeight)
                    return;

                // Temporäres Rechteck für Kollisionsprüfung
                Rechteck temp = new Rechteck(Breite, Hoehe, neueX, neueY);

                foreach (var r in alleRechtecke)
                {
                    if (r == this)
                        continue;

                    if (temp.BeruehrtOderUeberlappt(r))
                        return; // Bewegung abbrechen
                }

                // Keine Kollision ? Bewegung ausführen
                X = neueX;
                Y = neueY;
            }

            private bool BeruehrtOderUeberlappt(Rechteck anderes)
            {
                // 1 Zeichen Sicherheitsabstand
                return !(X + Breite + 1 <= anderes.X ||
                         X - 1 >= anderes.X + anderes.Breite ||
                         Y + Hoehe + 1 <= anderes.Y ||
                         Y - 1 >= anderes.Y + anderes.Hoehe);
            }
        }
    }

    static void Main(string[] args)
        {
            Console.Clear();

            List<Rechteck> rechtecke = new List<Rechteck>();

            Console.Write("Wie viele Rechtecke möchten Sie zeichnen? ");
            int anzahl = int.Parse(Console.ReadLine());

            for (int i = 0; i < anzahl; i++)
            {
                Rechteck r = null;

                while (true) // wiederholen bis gültiges, nicht-überlappendes (inkl. Kantenberührung) Rechteck eingegeben wurde
                {
                    Console.Clear();
                    Console.WriteLine($"Rechteck {i + 1}");

                    int breite = ReadInt("Breite eingeben: ");
                    int hoehe = ReadInt("Höhe eingeben: ");

                    if (breite <= 0 || hoehe <= 0)
                    {
                        Console.WriteLine("Breite und Höhe müssen größer als 0 sein. Drücken Sie eine Taste und versuchen Sie es erneut.");
                        Console.ReadKey(true);
                        continue;
                    }

                    int maxX = Console.WindowWidth - breite;
                    int maxY = Console.WindowHeight - hoehe;

                    if (maxX < 0 || maxY < 0)
                    {
                        Console.WriteLine("Das Rechteck ist größer als das Konsolenfenster. Drücken Sie eine Taste und geben Sie kleinere Werte ein.");
                        Console.ReadKey(true);
                        continue;
                    }

                    int x = ReadInt($"Position X eingeben (0..{maxX}): ");
                    int y = ReadInt($"Position Y eingeben (0..{maxY}): ");

                    if (x < 0 || x > maxX || y < 0 || y > maxY)
                    {
                        Console.WriteLine("Die Position liegt außerhalb des erlaubten Bereichs. Drücken Sie eine Taste und versuchen Sie es erneut.");
                        Console.ReadKey(true);
                        continue;
                    }

                    r = new Rechteck(breite, hoehe, x, y);

                    bool conflict = false;
                    foreach (Rechteck existing in rechtecke)
                    {
                        if (r.IntersectsOrTouches(existing))
                        {
                            conflict = true;
                            break;
                        }
                    }

                    if (conflict)
                    {
                        Console.WriteLine("Das eingegebene Rechteck überlappt oder berührt die Kanten eines bereits vorhandenen Rechtecks. Drücken Sie eine Taste und geben Sie die Daten erneut ein.");
                        Console.ReadKey(true);
                        continue;
                    }

                    // gültiges Rechteck ohne Überlappung/Kantenberührung -> Schleife verlassen
                    break;
                }

                rechtecke.Add(r);
            }

            // Steuerung des letzten Rechtecks
            Rechteck bewegbaresRechteck = rechtecke[rechtecke.Count - 1];

            while (true)
            {
                Console.Clear();

                foreach (Rechteck rr in rechtecke)
                {
                    rr.Zeichnen();
                }

                ConsoleKeyInfo taste = Console.ReadKey(true);

                switch (taste.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                    bewegbaresRechteck.Bewegen(0, -1, rechtecke);
                    break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                    bewegbaresRechteck.Bewegen(0, -1, rechtecke);
                    break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                    bewegbaresRechteck.Bewegen(0, -1, rechtecke);
                    break;

                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                    bewegbaresRechteck.Bewegen(0, -1, rechtecke);
                    break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}