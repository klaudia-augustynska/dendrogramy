using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dendrogramy.Algorytm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dendrogramy.Testy.AlgorytmTesty
{
    [TestClass]
    public class FunkcjeMatematyczneTesty
    {
        [TestMethod]
        public void OdległośćEuklidesowa_PoprawnieLiczy_DlaDodatnich()
        {
            double b = 4;
            double c = 6;
            double wynik = 2;

            double otrzymanyWynik = FunkcjeMatematyczne.OdległośćEuklidesowa(b, c);

            Assert.AreEqual(wynik, otrzymanyWynik);
        }

        [TestMethod]
        public void OdległośćEuklidesowa_PoprawnieLiczy_DlaUjemnych()
        {
            double b = -4;
            double c = -6;
            double wynik = 2;

            double otrzymanyWynik = FunkcjeMatematyczne.OdległośćEuklidesowa(b, c);

            Assert.AreEqual(wynik, otrzymanyWynik);
        }

        [TestMethod]
        public void OdległośćEuklidesowa_PoprawnieLiczy_JednaDodatniaDrugaUjemna()
        {
            double b = 4;
            double c = -6;
            double wynik = 10;

            double otrzymanyWynik = FunkcjeMatematyczne.OdległośćEuklidesowa(b, c);

            Assert.AreEqual(wynik, otrzymanyWynik);
        }

        [TestMethod]
        public void OdległośćEuklidesowa_PoprawnieLiczy_JednaUjemnaDrugaDodatnia()
        {
            double b = -4;
            double c = 6;
            double wynik = 10;

            double otrzymanyWynik = FunkcjeMatematyczne.OdległośćEuklidesowa(b, c);

            Assert.AreEqual(wynik, otrzymanyWynik);
        }

        [TestMethod]
        public void MetodaPojedynczegoPołączenia_PoprawnieLiczy()
        {
            double[] A = { -5, 1, 2, 4 };
            double[] B = { 7, -6, 9 };
            double odpowiedź = 1;

            double uzyskanaOdpowiedź = FunkcjeMatematyczne.MetodaPojedynczegoPołączenia(ref A, ref B);

            Assert.AreEqual(odpowiedź, uzyskanaOdpowiedź);
        }

        [TestMethod]
        public void MetodaCałkowitegoPołączenia_PoprawnieLiczy()
        {
            double[] A = { -5, 1, 2, 4 };
            double[] B = { 7, -6, 9 };
            double odpowiedź = 14;

            double uzyskanaOdpowiedź = FunkcjeMatematyczne.MetodaCałkowitegoPołączenia(ref A, ref B);

            Assert.AreEqual(odpowiedź, uzyskanaOdpowiedź);
        }

        [TestMethod]
        public void MetodaŚrednichGrupowych_PoprawnieLiczy()
        {
            double[] A = { -5, 1, 2, 4 };
            double[] B = { 7, -6, 9 };
            double odpowiedź = 7.167;

            double uzyskanaOdpowiedź = FunkcjeMatematyczne.MetodaŚrednichGrupowych(ref A, ref B);

            uzyskanaOdpowiedź = Math.Round(uzyskanaOdpowiedź,3);
            Assert.AreEqual(odpowiedź, uzyskanaOdpowiedź);
        }

        [TestMethod]
        public void MetodaCentroidalnegoPołączenia_PoprawnieLiczy()
        {
            double[] A = { -5, 1, 2, 4 }; // Σ = 2, 2/4 = 0.5
            double[] B = { 7, -6, 9 }; // Σ = 10, 10/3 = 3.3333
            double odpowiedź = 2.833;

            double uzyskanaOdpowiedź = FunkcjeMatematyczne.MetodaCentroidalnegoPołączenia(ref A, ref B);

            uzyskanaOdpowiedź = Math.Round(uzyskanaOdpowiedź, 3);
            Assert.AreEqual(odpowiedź, uzyskanaOdpowiedź);
        }
    }
}
