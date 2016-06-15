using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dendrogramy.Algorytm;
using Dendrogramy.Enumy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dendrogramy.Testy.AlgorytmTesty
{
    [TestClass]
    public class HierarchicznaAnalizaSkupieńTesty
    {
        private double[] dane = 
        {
            2, 5, 9, 15, 16, 18, 25, 33, 33, 45
        };

        [TestMethod]
        public void Konstruktor_TworzyKlastry()
        {
            int[] klastry = 
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);

            PrivateObject accessor = new PrivateObject(algorytm);
            int[] uzyskaneKlastry = (int[]) accessor.GetField("klastry");
            CollectionAssert.AreEqual(klastry, uzyskaneKlastry);
        }

        [TestMethod]
        public void Konstruktor_TworzyMacierzOdległościMiędzyklastrowych()
        {
            double[][] macierz = new double[10][];
            macierz[0] = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[1] = new double[] {3, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[2] = new double[] {7, 4, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[3] = new double[] {13,10,6, 0, 0, 0, 0, 0, 0, 0 };
            macierz[4] = new double[] {14,11,7, 1, 0, 0, 0, 0, 0, 0 };
            macierz[5] = new double[] {16,13,9, 3, 2, 0, 0, 0, 0, 0 };
            macierz[6] = new double[] {23,20,16,10,9, 7, 0, 0, 0, 0 };
            macierz[7] = new double[] {31,28,24,18,17,15,8, 0, 0, 0 };
            macierz[8] = new double[] {31,28,24,18,17,15,8, 0, 0, 0 };
            macierz[9] = new double[] {43,40,36,30,29,27,20,12,12,0 };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);

            PrivateObject accessor = new PrivateObject(algorytm);
            double[][] uzyskanaMacierz = (double[][])accessor.GetField("macierzOdległościMiędzyklastrowej");
            for (int i = 0; i < 10; ++i)
                CollectionAssert.AreEqual(macierz[i], uzyskanaMacierz[i]);
        }

        [TestMethod]
        public void PołączGrupy_ZnajdujeWłaściwąGrupę_Krok1()
        {
            JednoPołączenie grupa = new JednoPołączenie()
            {
                IndeksOd = 7,
                IndeksDo = 8,
                PoziomZagłębienia = 1
            };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            JednoPołączenie uzyskanaGrupa = algorytm.PołączGrupy();

            Assert.AreEqual(grupa, uzyskanaGrupa);
        }

        [TestMethod]
        public void PołączGrupy_ŁączyGrupy_Krok1()
        {
            int[] klastry = new int[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 7, 9
            };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();

            PrivateObject accessor = new PrivateObject(algorytm);
            int[] uzyskaneKlastry = (int[])accessor.GetField("klastry");
            CollectionAssert.AreEqual(klastry, uzyskaneKlastry);
        }

        [TestMethod]
        public void PołączGrupy_UaktualniaMacierz_Krok1()
        {
            // tak ma wyglądać macierz po tym jak utworzyła się grupa
            // więc jakby rozmiar macierzy logicznie się zmienił
            // wiersz 9, kolumna 9 - mają stare wartości
            // wiersz 8 - ma wartości te co poprzednio w 9, bez odległości do 8
            // metoda tu jeszcze nie ma znaczenia, bo grupa zawiera 2 takie same elementy
            double[][] macierz = new double[10][];
            macierz[0] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[1] = new double[] { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[2] = new double[] { 7, 4, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[3] = new double[] { 13, 10, 6, 0, 0, 0, 0, 0, 0, 0 };
            macierz[4] = new double[] { 14, 11, 7, 1, 0, 0, 0, 0, 0, 0 };
            macierz[5] = new double[] { 16, 13, 9, 3, 2, 0, 0, 0, 0, 0 };
            macierz[6] = new double[] { 23, 20, 16, 10, 9, 7, 0, 0, 0, 0 };
            macierz[7] = new double[] { 31, 28, 24, 18, 17, 15, 8, 0, 0, 0 };
            macierz[8] = new double[] { 43, 40, 36, 30, 29, 27, 20, 12, 0, 0 };
            macierz[9] = new double[] { 43, 40, 36, 30, 29, 27, 20, 12, 12, 0 };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();
            
            PrivateObject accessor = new PrivateObject(algorytm);
            double[][] uzyskanaMacierz = (double[][])accessor.GetField("macierzOdległościMiędzyklastrowej");
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 10; ++j)
                    if (macierz[i][j] != uzyskanaMacierz[i][j])
                        Assert.Fail("i:{0}\tj:{1}\tA:{2}\tB:{3}", i, j, macierz[i][j].ToString(), uzyskanaMacierz[i][j].ToString());
        }

        [TestMethod]
        public void PołączGrupy_ZnajdujeKolejneGrupy_ZTegoSamegoPoziomuZagłębienia()
        {
            JednoPołączenie grupa2 = new JednoPołączenie()
            {
                IndeksOd = 3,
                IndeksDo = 4,
                PoziomZagłębienia = 1
            };
            JednoPołączenie grupa3 = new JednoPołączenie()
            {
                IndeksOd = 3,
                IndeksDo = 4,
                PoziomZagłębienia = 2
            };


            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();

            JednoPołączenie uzyskanaGrupa = algorytm.PołączGrupy();
            Assert.AreEqual(grupa2, uzyskanaGrupa);

            uzyskanaGrupa = algorytm.PołączGrupy();
            Assert.AreEqual(grupa3, uzyskanaGrupa);
        }

        [TestMethod]
        public void PołączGrupy_ZnajdujeKolejneGrupy_ZKolejnychPoziomów()
        {
            List<JednoPołączenie> grupyOd4Do9 = new List<JednoPołączenie>
            {
                new JednoPołączenie() { IndeksOd = 0, IndeksDo =  1, PoziomZagłębienia = 2},
                new JednoPołączenie() { IndeksOd = 0, IndeksDo =  1, PoziomZagłębienia = 3},
                new JednoPołączenie() { IndeksOd = 0, IndeksDo =  1, PoziomZagłębienia = 4},
                new JednoPołączenie() { IndeksOd = 0, IndeksDo =  1, PoziomZagłębienia = 5},
                new JednoPołączenie() { IndeksOd = 0, IndeksDo =  1, PoziomZagłębienia = 6}
            };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            for (int i = 1; i <= 3; ++i)
                algorytm.PołączGrupy();

            for (int i = 4; i <= 9; ++i)
            {
                JednoPołączenie uzyskanaGrupa = algorytm.PołączGrupy();
                //Assert.AreEqual(grupyOd4Do9[i], uzyskanaGrupa);
            }
        }

        [TestMethod]
        public void PołączGrupy_UaktualniaMacierz_Krok2()
        {
            double[][] macierz = new double[10][];
            macierz[0] = new double[] { 0,  0,  0,  0,  0,  0,  0, 0, 0, 0 };
            macierz[1] = new double[] { 3,  0,  0,  0,  0,  0,  0, 0, 0, 0 };
            macierz[2] = new double[] { 7,  4,  0,  0,  0,  0,  0, 0, 0, 0 };
            macierz[3] = new double[] { 13, 10, 6,  0,  0,  0,  0, 0, 0, 0 };
            macierz[4] = new double[] { 16, 13, 9,  2,  0,  0,  0, 0, 0, 0 };
            macierz[5] = new double[] { 23, 20, 16, 9,  7,  0,  0, 0, 0, 0 };
            macierz[6] = new double[] { 31, 28, 24, 17, 15, 8,  0, 0, 0, 0 };
            macierz[7] = new double[] { 43, 40, 36, 29, 27, 20, 12,0, 0, 0 };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();

            PrivateObject accessor = new PrivateObject(algorytm);
            double[][] uzyskanaMacierz = (double[][])accessor.GetField("macierzOdległościMiędzyklastrowej");
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                    if (macierz[i][j] != uzyskanaMacierz[i][j])
                        Assert.Fail("i:{0}\tj:{1}\tA:{2}\tB:{3}", i, j, macierz[i][j].ToString(), uzyskanaMacierz[i][j].ToString());
        }


        [TestMethod]
        public void PołączGrupy_UaktualniaMacierz_Krok3()
        {
            double[][] macierz = new double[10][];
            macierz[0] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[1] = new double[] { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[2] = new double[] { 7, 4, 0, 0, 0, 0, 0, 0, 0, 0 };
            macierz[3] = new double[] { 13, 10, 6,  0, 0, 0, 0, 0, 0, 0 };
            macierz[4] = new double[] { 23, 20, 16, 7 };
            macierz[5] = new double[] { 31, 28, 24, 15, 8 };
            macierz[6] = new double[] { 43, 40, 36, 27, 20, 12 };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();

            PrivateObject accessor = new PrivateObject(algorytm);
            double[][] uzyskanaMacierz = (double[][])accessor.GetField("macierzOdległościMiędzyklastrowej");
            for (int i = 0; i < 7; ++i)
                for (int j = 0; j < i; ++j)
                    if (macierz[i][j] != uzyskanaMacierz[i][j])
                        Assert.Fail("i:{0}\tj:{1}\tA:{2}\tB:{3}", i, j, macierz[i][j].ToString(), uzyskanaMacierz[i][j].ToString());
        }


        [TestMethod]
        public void PołączGrupy_UaktualniaMacierz_Krok4()
        {
            // tak ma wyglądać macierz po tym jak utworzyła się grupa
            // więc jakby rozmiar macierzy logicznie się zmienił
            // wiersz 9, kolumna 9 - mają stare wartości
            // wiersz 8 - ma wartości te co poprzednio w 9, bez odległości do 8
            // metoda tu jeszcze nie ma znaczenia, bo grupa zawiera 2 takie same elementy
            double[][] macierz = new double[10][];
            macierz[0] = new double[] { 0 };
            macierz[1] = new double[] { 4 };
            macierz[2] = new double[] { 10, 6 };
            macierz[3] = new double[] { 20, 16, 7 };
            macierz[4] = new double[] { 28, 24, 15, 8 };
            macierz[5] = new double[] { 40, 36, 27, 20, 12 };

            var algorytm = new HierarchicznaAnalizaSkupień(dane, MetodaSkupień.PojedynczegoPołączenia);
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();
            algorytm.PołączGrupy();

            PrivateObject accessor = new PrivateObject(algorytm);
            double[][] uzyskanaMacierz = (double[][])accessor.GetField("macierzOdległościMiędzyklastrowej");
            for (int i = 0; i < 6; ++i)
                for (int j = 0; j < i; ++j)
                    if (macierz[i][j] != uzyskanaMacierz[i][j])
                        Assert.Fail("i:{0}\tj:{1}\tA:{2}\tB:{3}", i, j, macierz[i][j].ToString(), uzyskanaMacierz[i][j].ToString());
        }
    }
}
