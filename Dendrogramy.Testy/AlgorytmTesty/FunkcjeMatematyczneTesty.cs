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
    }
}
