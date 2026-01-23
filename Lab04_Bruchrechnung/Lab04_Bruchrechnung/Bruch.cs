using System;

namespace Lab04_Bruchrechnung
{
    class Bruch
    {
        private int zaehler;
        private int nenner;
        public Bruch(int zaehler, int nenner)
        {
            this.zaehler = zaehler;
            this.nenner = nenner;
        }
        public int getZaehler()
        {
            return zaehler;
        }
        public int getNenner()
        {
            return nenner;
        }
        public void setZahler(int zaehler)
        {
             this.zaehler = zaehler;
        }
        public void setNenner(int nenner)
        {
            if (nenner == 0)
                throw new Exception();
             this.nenner = nenner;
        }
        public override string ToString()
        {
            return $"{zaehler} / {nenner}";
        }
        public void Kuerzen()
        {
            // 28 - 35
            int kleinster = Math.Min(zaehler, nenner);
            for (int i = kleinster; i > 1; i--)
            {
                if (zaehler % i == 0 && nenner % i == 0)
                {
                    zaehler /= i;
                    nenner /= i;
                    break;
                }
            }
        }
        public void Add(Bruch b)
        {
            // a/b + c/d = (a*d + b*c) / (b*d)
            int neuerZaehler = this.zaehler * b.nenner + this.nenner * b.zaehler;
            int neuerNenner = this.nenner * b.nenner;
            this.zaehler = neuerZaehler;
            this.nenner = neuerNenner;
            Kuerzen();
        }
        public void Sub(Bruch b)
        {
            // a/b - c/d = (a*d - b*c) / (b*d)
            int neuerZaehler = this.zaehler * b.nenner - this.nenner * b.zaehler;
            int neuerNenner = this.nenner * b.nenner;
            this.zaehler = neuerZaehler;
            this.nenner = neuerNenner;
            Kuerzen();
        }
        public void Mul(Bruch b)
        {
            // a/b * c/d = (a*c) / (b*d)
            int neuerZaehler = this.zaehler * b.zaehler;
            int neuerNenner = this.nenner * b.nenner;
            this.zaehler = neuerZaehler;
            this.nenner = neuerNenner;
            Kuerzen();
        }
        public void Div(Bruch b)
        {
            // a/b : c/d = (a*d) / (b*c)
            int neuerZaehler = this.zaehler * b.nenner;
            int neuerNenner = this.nenner * b.zaehler;
            this.zaehler = neuerZaehler;
            this.nenner = neuerNenner;
            Kuerzen();
        }
        public static Bruch Parse(string str)
        {
           string [] teile = str.Split('/');
           int zaehler = int.Parse(teile[0]);
           int nenner = int.Parse(teile[1]);
            return new Bruch (zaehler, nenner);
        }
    }
}
