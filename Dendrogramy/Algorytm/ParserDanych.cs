using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrogramy.Algorytm
{
    class ParserDanych
    {
        private string nazwa;

        public ParserDanych(string nazwa)
        {
            this.nazwa = nazwa;
        }

        public async Task<double[]> WczytajDane()
        {
            if (!File.Exists(nazwa))
                throw new ApplicationException("Podany plik nie istnieje");
            return await ParsujPlik();
        }

        private async Task<double[]> ParsujPlik()
        {
            int ilośćLiniiWPliku = File.ReadLines(nazwa).Count();
            var liczby = new double[ilośćLiniiWPliku];
            double[] liczby2;

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

                liczby2 = new double[i];
                for (int j = 0; j < i; ++j)
                {
                    liczby2[j] = liczby[i];
                }
            }
            return liczby2;
        }
    }
}
