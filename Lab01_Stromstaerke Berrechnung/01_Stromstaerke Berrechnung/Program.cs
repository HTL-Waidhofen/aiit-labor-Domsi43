string Spannung;
string  Widerstand;
double Spannung_double;
double Widerstand_double;
double Strom;
do
{
    Console.WriteLine("Geben sie den Wert ihres Widerstandes ein und danach mit einen Leerzeichen die Einheit");
    Console.WriteLine("Widerstand (Ohm, kOhm, MOhm):");
    Widerstand = Console.ReadLine();
    string[] Widerstand_split = Widerstand.Split(' ');
    if (Widerstand == "q")
        break;
    switch (Widerstand_split[1])
    {
        case "Ohm":
            Widerstand_double = double.Parse(Widerstand_split[0]);
            Widerstand_double = Widerstand_double;
            break;
        case "kOhm":
            Widerstand_double = double.Parse(Widerstand_split[0]);
            Widerstand_double = Widerstand_double * 1000;
            break;
        case "MOhm":
            Widerstand_double = double.Parse(Widerstand_split[0]);
            Widerstand_double = Widerstand_double * 1000000;
            break;
        default:
            Console.WriteLine("Falsche Eingabe!");
            return;
    }

    Console.WriteLine("Geben sie den Wert ihrer Spannung ein und danach mit einen Leerzeichen die Einheit");
    Console.WriteLine("Spannung (V, kV, mV):");
    Spannung = Console.ReadLine();
    string[] Spannung_split = Spannung.Split(' ');
    if (Spannung == "q")
        break;
    switch (Spannung_split[1])
    {
        case "V":
            Spannung_double = double.Parse(Spannung_split[0]);
            Spannung_double = Spannung_double;
            break;
        case "kV":
            Spannung_double = double.Parse(Spannung_split[0]);
            Spannung_double = Spannung_double * 1000;
            break;
        case "mV":
            Spannung_double = double.Parse(Spannung_split[0]);
            Spannung_double = Spannung_double / 1000;
            break;
        default:
            Console.WriteLine("Falsche Eingabe!");
            return;
    }
    Strom = Spannung_double / Widerstand_double;
    Console.WriteLine($"Die Stromstärke beträgt {Strom} Ampere.");
}while ((Widerstand  != "q") || (Spannung != "q"));