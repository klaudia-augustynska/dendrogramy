using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrogramy.Algorytm
{
    public static class FunkcjeMatematyczne
    {
        public static double OdległośćEuklidesowa(double b, double c)
        {
            b = Math.Abs(b);
            c = Math.Abs(c);
            return Math.Abs(b-c);
        }
        
        public static double MetodaPojedynczegoPołączenia(ref double[] A, ref double[] B)
        {
            double odległość = FunkcjeMatematyczne.OdległośćEuklidesowa(A[0], B[0]);
            for (int i = 0; i < A.Length; ++i)
            {
                for (int j = 0; j < B.Length; ++j)
                {
                    double odległość_tymczasowa = FunkcjeMatematyczne.OdległośćEuklidesowa(A[i], B[j]);
                    if (odległość_tymczasowa < odległość)
                        odległość = odległość_tymczasowa;
                }
            }
            return odległość;
        }
    }
}
