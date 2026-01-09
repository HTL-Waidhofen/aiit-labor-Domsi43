using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lab03_Quader
{
    class Quader
    {
        // Maße in m
        private double hoehe;
        private double breite;
        private double laenge;

        public Quader() : this(1, 1, 1)
        {
        }
        public Quader(double hoehe, double breite, double laenge)
        {
            this.hoehe = hoehe;
            this.breite = breite;
            this.laenge = laenge;
        }
        public static double ParseValue(string text)
        {
            double value = 0;
            if (text.EndsWith("cm"))   // hoehe
            {
                string hoeheStr = text.Replace("cm", "");
                value = Double.Parse(hoeheStr) * 10;
            }
            else if (text.EndsWith("mm"))
            {
                string hoeheStr = text.Replace("mm", "");
                value = Double.Parse(hoeheStr);
            }
            return value;
        }
        public static Quader Parse(string text)  // static -> Klassenmethode
        {
            double hoehe = 0;
            double breite = 0;
            double laenge = 0;

            text = text.Replace(" ", ""); // "2cm; 3cm; 5mm" --> "2cm;3cm,5mm"
            string[] teile = text.Split(';');   //["2cm", "3cm", "5mm"]

            hoehe = ParseValue(teile[0]);
            breite = ParseValue(teile[1]);
            laenge = ParseValue(teile[2]);

            return new Quader(hoehe, breite, laenge);
        }
        public double GetVolume()
        {
            return hoehe * breite * laenge;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Bitte Quader Eingeben: 2cm; 3cm; 5mm
            Console.Write("Bitte Quader Eingeben: ");
            string eingabe = Console.ReadLine();

            Quader q = Quader.Parse(eingabe);   // Klassenmethode

            Console.WriteLine($"Volumen des Quaders: {q.GetVolume()}mm³");  // Instanzmethode
            Console.ReadKey();

            //Quader q1 = new Quader();
            //Console.WriteLine(q1.GetHeight());   // Instanzmethode

            //string intStr = "12";
            //int x = int.Parse(intStr);
        }
    }
}
