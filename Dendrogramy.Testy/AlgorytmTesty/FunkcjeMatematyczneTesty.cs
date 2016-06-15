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
        public void OdległośćEuklidesowa_PoprawnieLiczy()
        {
            double b = 4;
            double c = 6;
            double wynik = 2;

            double otrzymanyWynik = FunkcjeMatematyczne.OdległośćEuklidesowa(b, c);

            Assert.AreEqual(wynik, otrzymanyWynik);
        }

        [TestMethod]
        public void OdległośćEuklidesowa_PoprawnieLiczyDlaUjemnych()
        {
            double b = -4;
            double c = -6;
            double wynik = 2;

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
    }
}
