using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrogramy.Algorytm
{
    /// <summary>
    /// Parsuje zadany plik do formatu czytelnego dla algorytmu.
    /// </summary>
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
            List<double> liczby = new List<double>();

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
                    liczby.Add(liczba);
                }
            }
            liczby.Sort();

            double[] liczbyArray = new double[liczby.Count];
            for (int i = 0; i < liczby.Count; ++i)
                liczbyArray[i] = liczby[i];
            return liczbyArray;
        }
    }
}
