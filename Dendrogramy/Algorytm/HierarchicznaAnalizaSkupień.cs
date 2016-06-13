using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dendrogramy.Enumy;

namespace Dendrogramy.Algorytm
{
    class HierarchicznaAnalizaSkupień
    {
        private MetodaSkupień metoda;
        private string nazwa;
        private double[] liczby;

        public HierarchicznaAnalizaSkupień(string nazwa, MetodaSkupień metoda)
        {
            this.nazwa = nazwa;
            this.metoda = metoda;
        }

        public async Task WczytajDane()
        {
            if (!File.Exists(nazwa))
                throw new ApplicationException("Podany plik nie istnieje");
            await ParsujPlik();
        }

        private async Task ParsujPlik()
        {
            int ilośćLiniiWPliku = File.ReadLines(nazwa).Count();
            liczby = new double[ilośćLiniiWPliku];

            using (var sr = new StreamReader(nazwa))
            {
                var format = new NumberFormatInfo();
                format.CurrencyDecimalSeparator = ".";
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string linia = await sr.ReadLineAsync();
                    string wartosc = linia.Replace(System.Environment.NewLine, string.Empty);
                    if (wartosc.Length == 0)
                        continue;
                    double liczba = double.Parse(wartosc, format);
                    liczby[i++] = liczba;
                }
            }
        }
    }
}
