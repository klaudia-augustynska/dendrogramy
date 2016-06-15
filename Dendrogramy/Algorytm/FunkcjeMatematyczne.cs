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
            if (b < 0 && c < 0)
            {
                b *= -1;
                c *= -1;
            }
            return Math.Abs(b-c);
        }
        
        public static double MetodaPojedynczegoPołączenia(ref double[] A, ref double[] B)
        {
            double odległość = OdległośćEuklidesowa(A[0], B[0]);
            for (int i = 0; i < A.Length; ++i)
            {
                for (int j = 0; j < B.Length; ++j)
                {
                    double odległość_tymczasowa = OdległośćEuklidesowa(A[i], B[j]);
                    if (odległość_tymczasowa < odległość)
                        odległość = odległość_tymczasowa;
                }
            }
            return odległość;
        }

        public static double MetodaCałkowitegoPołączenia(ref double[] A, ref double[] B)
        {
            double odległość = OdległośćEuklidesowa(A[0], B[0]);
            for (int i = 0; i < A.Length; ++i)
            {
                for (int j = 0; j < B.Length; ++j)
                {
                    double odległość_tymczasowa = OdległośćEuklidesowa(A[i], B[j]);
                    if (odległość_tymczasowa > odległość)
                        odległość = odległość_tymczasowa;
                }
            }
            return odległość;
        }

        public static double MetodaŚrednichGrupowych(ref double[] A, ref double[] B)
        {
            double sumaOdległości = 0;
            double ilość = A.Length*B.Length;
            for (int i = 0; i < A.Length; ++i)
            {
                for (int j = 0; j < B.Length; ++j)
                {
                    double odległość_tymczasowa = OdległośćEuklidesowa(A[i], B[j]);
                    sumaOdległości += odległość_tymczasowa;
                }
            }
            return sumaOdległości / ilość;
        }

        public static double MetodaCentroidalnegoPołączenia(ref double[] A, ref double[] B)
        {
            double centroidA = 0, centroidB = 0;
            foreach (var element in A)
                centroidA += element;
            centroidA /= A.Length;
            foreach (var element in B)
                centroidB += element;
            centroidB /= B.Length;
            return OdległośćEuklidesowa(centroidA, centroidB);
        }
    }
}
