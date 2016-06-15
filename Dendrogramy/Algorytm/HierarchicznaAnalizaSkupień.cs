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

        public HierarchicznaAnalizaSkupień(double[] liczby, MetodaSkupień metoda)
        {
            this.liczby = liczby;
            this.metoda = metoda;

            UmieśćKażdyObiektWOsobnymKlastrze();
            SkonstruujMacierzOdległościMiędzyklastrowej();
        }

        private void UmieśćKażdyObiektWOsobnymKlastrze()
        {
            klastry = new int[liczby.Length];
            for (int i = 0; i < liczby.Length; ++i)
                klastry[i] = i;

            poziomyZagłębień = new int[liczby.Length];
            Array.Clear(poziomyZagłębień, 0, poziomyZagłębień.Length);
        }

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

        public JednoPołączenie PołączGrupy()
        {
            ++krok;
            var najbliższaPara = ZnajdźNajbliższąParęKlastrów();
            PołączKlastry(najbliższaPara);
            UaktualnijMacierzOdległościMiędzyklastrowych();
            if (WszystkieGrupyNależąDoJednegoKlastra())
                możnaŁączyćSkupiska = false;

            return new JednoPołączenie()
            {
                IndeksOd = najbliższaPara.Item1,
                IndeksDo = najbliższaPara.Item2,
                PoziomZagłębienia = poziomyZagłębień[najbliższaPara.Item1]
            };
        }

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
        
        private void PołączKlastry(Tuple<int,int> najbliższaPara)
        {
            int[] indeksyKolejnychKlastrów = ZmagazynujIndeksyKolejnychKlastrów();
            int nowyKlaster = klastry[indeksyKolejnychKlastrów[najbliższaPara.Item1]];
            int nowyPoziom = poziomyZagłębień[najbliższaPara.Item1];

            int przelatujOd = indeksyKolejnychKlastrów[najbliższaPara.Item1] + 1;
            int przelatujDo = -1;
            if (najbliższaPara.Item2 + 1 == liczbaGrup)
                przelatujDo = liczbaGrup - 1;
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
            for (int i = najbliższaPara.Item1; i <= najbliższaPara.Item2; ++i)
            {
                poziomyZagłębień[i] = nowyPoziom;
            }
            --liczbaGrup;
        }

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

        private bool WszystkieGrupyNależąDoJednegoKlastra()
        {
            int jedenKlaster = klastry[0];
            for (int i = 1; i < klastry.Length; ++i)
                if (klastry[i] != jedenKlaster)
                    return false;
            return true;
        }

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
