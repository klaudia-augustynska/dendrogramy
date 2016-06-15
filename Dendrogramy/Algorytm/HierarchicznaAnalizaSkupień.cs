using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dendrogramy.Enumy;

namespace Dendrogramy.Algorytm
{
    /// <summary>
    /// Algorytm grupowania hierarchicznego. 
    /// </summary>
    public class HierarchicznaAnalizaSkupień
    {
        private readonly MetodaSkupień metoda;
        private readonly double[] liczby;

        private int[] klastry;
        private int[] poziomyZagłębień;
        private int liczbaGrup;
        private double[][] macierzOdległościMiędzyklastrowej;
        private int krok = 0;

        public bool możnaŁączyćSkupiska = true;
        
        /// <param name="liczby">Lista liczb do pogrupowania, posortowana.</param>
        /// <param name="metoda">Metoda określania odległości grup</param>
        public HierarchicznaAnalizaSkupień(double[] liczby, MetodaSkupień metoda)
        {
            this.liczby = liczby;
            this.metoda = metoda;

            UmieśćKażdyObiektWOsobnymKlastrze();
            SkonstruujMacierzOdległościMiędzyklastrowej();
        }

        /// <summary>
        /// Klastrów jest tyle co liczb i w każdym jest inna wartość na znak, 
        /// że każda liczba jest w osobnej grupie.
        /// Każda grupa znajduje się na poziomie zagłębień wykresu = 0, bo to początek.
        /// </summary>
        private void UmieśćKażdyObiektWOsobnymKlastrze()
        {
            klastry = new int[liczby.Length];
            for (int i = 0; i < liczby.Length; ++i)
                klastry[i] = i;

            poziomyZagłębień = new int[liczby.Length];
            Array.Clear(poziomyZagłębień, 0, poziomyZagłębień.Length);
        }

        /// <summary>
        /// Tworzy macierz kwadratową (dolnotrójkątną) wielkości równej ilości liczb.
        /// (W późniejszych etapach gdy ta macierz będzie się pomniejszać, 
        /// jej wielkość będzie logicznie określać zmienna "liczbaGrup".)
        /// Macierz wypełniana jest odległością euklidesową między każdą parą liczb.
        /// </summary>
        private void SkonstruujMacierzOdległościMiędzyklastrowej()
        {
            liczbaGrup = liczby.Length;
            macierzOdległościMiędzyklastrowej = new double[liczbaGrup][];
            for (int i = 0; i < liczbaGrup; ++i)
            {
                macierzOdległościMiędzyklastrowej[i] = new double[liczbaGrup];
                Array.Clear(macierzOdległościMiędzyklastrowej[i], 0, liczbaGrup);
            }

            PrzejrzyjMacierz((i, j) =>
            {
                macierzOdległościMiędzyklastrowej[i][j] =
                    FunkcjeMatematyczne.OdległośćEuklidesowa(liczby[i], liczby[j]);
            });
        }

        /// <summary>
        /// Łączy dwie grupy w jedną zgodnie z algorytmem.
        /// </summary>
        /// <returns>Struktura określająca, które dwie grupy z poprzedniego poziomu zostały połączone.</returns>
        public JednoPołączenie PołączGrupy()
        {
            ++krok;
            var najbliższaPara = ZnajdźNajbliższąParęKlastrów();
            int[] indeksy = ZmagazynujIndeksyKolejnychKlastrów();
            PołączKlastry(najbliższaPara,indeksy);
            UaktualnijMacierzOdległościMiędzyklastrowych();
            if (WszystkieGrupyNależąDoJednegoKlastra())
                możnaŁączyćSkupiska = false;
            if (liczbaGrup > 1 && najbliższaPara.Item2 + 1 < indeksy.Length)
                return new JednoPołączenie()
                {
                    IndeksOd = indeksy[najbliższaPara.Item1],
                    IndeksDo = indeksy[najbliższaPara.Item2+1]-1,
                    PoziomZagłębienia = (indeksy[najbliższaPara.Item2 + 1] - 1) - (indeksy[najbliższaPara.Item1])
                };
            if (liczbaGrup > 1)
            {
                return new JednoPołączenie()
                {
                    IndeksOd = indeksy[najbliższaPara.Item1],
                    IndeksDo = klastry.Length - 1,
                    PoziomZagłębienia = (klastry.Length - 1) - indeksy[najbliższaPara.Item1]
                };
            }
            return new JednoPołączenie()
            {
                IndeksOd = 0,
                IndeksDo = klastry.Length - 1,
                PoziomZagłębienia = (klastry.Length - 1)
            };
        }

        /// <summary>
        /// Wyciąga najmniejszą wartość z macierzy odległości.
        /// </summary>
        /// <returns>Współrzędne w macierzy, a jednocześnie indeksy liczb i klastrów.</returns>
        private Tuple<int,int> ZnajdźNajbliższąParęKlastrów()
        {
            int minI = 1, minJ = 0;
            PrzejrzyjMacierz((i, j) =>
            {
                if (macierzOdległościMiędzyklastrowej[i][j]
                    < macierzOdległościMiędzyklastrowej[minI][minJ])
                {
                    minI = i;
                    minJ = j;
                }
            });

            int a, b;
            a = Math.Min(minI, minJ);
            b = Math.Max(minI, minJ);

            return new Tuple<int, int>(a,b);
        }
        
        /// <summary>
        /// W tablicy "klastry" ustawia jednakową wartość dla połączonych klastrów.
        /// W tablicy "poziomyZagłębień" ustawia jednakową wartość pokazującą poziom wykresu.
        /// </summary>
        /// <param name="najbliższaPara">Indeksy klastrów, które należy ze sobą połączyć.</param>
        private void PołączKlastry(Tuple<int,int> najbliższaPara, int[] indeksyKolejnychKlastrów)
        {
            int nowyKlaster = klastry[indeksyKolejnychKlastrów[najbliższaPara.Item1]];
            int nowyPoziom = poziomyZagłębień[indeksyKolejnychKlastrów[najbliższaPara.Item1]];

            int przelatujOd = indeksyKolejnychKlastrów[najbliższaPara.Item1] + 1;
            int przelatujDo = -1;
            if (liczbaGrup == 2 || najbliższaPara.Item2 + 1 == liczbaGrup)
                przelatujDo = klastry.Length - 1;
            else
                przelatujDo = indeksyKolejnychKlastrów[najbliższaPara.Item2 + 1] - 1;

            for (int i = przelatujOd; i <= przelatujDo; ++i)
            {
                if (klastry[i] != nowyKlaster)
                    klastry[i] = nowyKlaster;
                
                // znajdź największy poziom w nowym klastrze
                if (nowyPoziom < poziomyZagłębień[i])
                    nowyPoziom = poziomyZagłębień[i];
            }
            ++nowyPoziom;
            for (int i = przelatujOd-1; i <= przelatujDo; ++i)
            {
                poziomyZagłębień[i] = nowyPoziom;
            }
            --liczbaGrup;
        }

        /// <summary>
        /// Uaktualnia odległości między nową, mniejszą ilością klastrów.
        /// Wielkość macierzy wyznacza "liczbaGrup" w sposób logiczny.
        /// </summary>
        private void UaktualnijMacierzOdległościMiędzyklastrowych()
        {
            int[] indeksyKolejnychKlastrów = ZmagazynujIndeksyKolejnychKlastrów();
            for (int grupaI = 1; grupaI < liczbaGrup; ++grupaI)
            {
                double[] elementyGrupyI = WeźElementyGrupy(indeksyKolejnychKlastrów[grupaI]);
                for (int grupaJ = 0; grupaJ < grupaI; ++grupaJ)
                {
                    double[] elementyGrupyJ = WeźElementyGrupy(indeksyKolejnychKlastrów[grupaJ]);
                    macierzOdległościMiędzyklastrowej[grupaI][grupaJ] = PoliczOdległośćMiędzyKlastrami(elementyGrupyI, elementyGrupyJ);
                }
            }
        }

        /// <summary>
        /// Tworzy tablicę wskaźników na początki kolejnych klastrów w tablicy "klastry".
        /// Wynika to z budowy tej tablicy, np. jeśli mamy 3 klastry:
        ///     klastry = [1, 1, 1, 2, 2, 2, 2, 3, 3]
        /// to funkcja zwróci
        ///     return    [0,       3,          7   ]
        /// </summary>
        /// <returns>Indeksy kolejnych klastrów.</returns>
        private int[] ZmagazynujIndeksyKolejnychKlastrów()
        {
            int[] indeksy = new int[liczbaGrup];

            for (int i = 0, j = 0; i < klastry.Length; ++i)
            {
                indeksy[j] = klastry[i];
                if (j + 1 < liczbaGrup) ++j;
                for (; i+1 < klastry.Length && klastry[i] == klastry[i + 1]; ++i) ;
            }

            return indeksy;
        }
        
        /// <param name="grupa">Liczba identyfikująca klaster.</param>
        /// <returns>Zbiór liczb zgrupowanych tym klastrem.</returns>
        private double[] WeźElementyGrupy(int grupa)
        {
            int indeksPierwszego = 0;
            for (int i = 0; i < klastry.Length; ++i)
                if (klastry[i] == grupa)
                {
                    indeksPierwszego = i;
                    break;
                }
            int indeksOstatniego = 0;
            for (int i = klastry.Length-1; i >= 0; --i)
                if (klastry[i] == grupa)
                {
                    indeksOstatniego = i;
                    break;
                }

            double[] zawartość = new double[indeksOstatniego-indeksPierwszego+1];

            for (int i = 0; i + indeksPierwszego <= indeksOstatniego; ++i)
            {
                zawartość[i] = liczby[i + indeksPierwszego];
            }

            return zawartość;
        }
        
        /// <summary>
        /// Na podstawie wybranej wcześniej metody skupień liczy odległości między klastrami.
        /// </summary>
        /// <param name="A">Pierwszy klaster</param>
        /// <param name="B">Drugi klaster</param>
        /// <returns>Odległość między klastrami</returns>
        private double PoliczOdległośćMiędzyKlastrami(double[] A, double[] B)
        {
            switch (metoda)
            {
                case MetodaSkupień.PojedynczegoPołączenia:
                    return FunkcjeMatematyczne.MetodaPojedynczegoPołączenia(ref A, ref B);
                case MetodaSkupień.CałkowitegoPołączenia:
                    return FunkcjeMatematyczne.MetodaCałkowitegoPołączenia(ref A, ref B);
                case MetodaSkupień.CentroidalnegoPołączenia:
                    return FunkcjeMatematyczne.MetodaCentroidalnegoPołączenia(ref A, ref B);
                case MetodaSkupień.ŚrednichGrupowych:
                    return FunkcjeMatematyczne.MetodaŚrednichGrupowych(ref A, ref B);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Warunek czy jeszcze się da grupować klastry.
        /// </summary>
        /// <returns>Tak / nie</returns>
        private bool WszystkieGrupyNależąDoJednegoKlastra()
        {
            int jedenKlaster = klastry[0];
            for (int i = 1; i < klastry.Length; ++i)
                if (klastry[i] != jedenKlaster)
                    return false;
            return true;
        }

        /// <summary>
        /// Jeśli chce się przejrzeć wszystkie elementy macierzy. Żeby było przejrzyściej.
        /// </summary>
        /// <param name="action">Funkcja jaką by się umieściło w dwóch pętlach.</param>
        private void PrzejrzyjMacierz(Action<int, int> action)
        {
            for (int i = 1; i < liczbaGrup; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    action.Invoke(i,j);
                }
            }
        }
    }
}
