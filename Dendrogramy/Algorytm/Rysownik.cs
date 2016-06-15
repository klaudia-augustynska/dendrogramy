using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dendrogramy.Algorytm
{
    /// <summary>
    /// Klasa odpowiedzialna za przekształcanie wyników algorytmu na elementy wizualne. Wyniki są spisywane w kolekcji, która jest referencją do kolekcji elementów wizualnych używanych we ViewModelu.
    /// </summary>
    public class Rysownik
    {
        private struct GrupaNaLiście
        {
            public int IndeksOd;
            public int IndeksDo;
            public double MiejsceOd;
            public double MiejsceDo;
            public int Poziom;
            public override string ToString()
            {
                return String.Format("Od:{0}\tDo:{1}\tMiejsceOd:{2}\tMiejsceDo:{3}\tPoziom:{4}", IndeksOd, IndeksDo, MiejsceOd,
                    MiejsceDo,Poziom);
            }
        }

        private Size rozmiarPłótna;
        private ObservableCollection<UIElement> listaKształtówDoWykresu;

        private double odległośćMiędzyPunktamiNaOsiY = 30.0;
        private double odległośćMiędzyGrupamiNaOsiX = 60.0;
        private double margines = 15.0;
        private double wysokośćWykresu = 0;
        private double szerokośćObszaruNaEtykietyPunktów = 0;
        private double początekWykresuOdLewej = 0;
        private double początekWykresuOdGóry = 0;

        private int krok = -20;

        private List<List<GrupaNaLiście>> dendrogram = new List<List<GrupaNaLiście>>(); 

        public Rysownik(Size rozmiarPłótna, ObservableCollection<UIElement> listaKształtówDoWykresu)
        {
            this.rozmiarPłótna = rozmiarPłótna;
            this.listaKształtówDoWykresu = listaKształtówDoWykresu;
        }

        /// <summary>
        /// Tworzy początek wykresu: tło, etykiety osi Y, pierwsze klastry.
        /// </summary>
        /// <param name="punkty">Punkty na oś Y</param>
        /// <returns>Wysokość wykresu jaki powstał</returns>
        public double NarysujPunktyIZwróćWysokośćWykresu(double[] punkty)
        {
            wysokośćWykresu = punkty.Length*odległośćMiędzyPunktamiNaOsiY;
            
            początekWykresuOdGóry = margines;
            var największaDługośćLiczby = RysujEtykietyIZwróćIchSzerokość(ref punkty);

            szerokośćObszaruNaEtykietyPunktów = największaDługośćLiczby*10;
            początekWykresuOdLewej = margines + szerokośćObszaruNaEtykietyPunktów;

            RysujTłoDlaWykresu();

            UmieśćPunktyNaLiście(punkty);

            return wysokośćWykresu + 2*margines;
        }

        /// <summary>
        /// Wstępna obsługa klastrów, które się tworzą na samym początku zanim cokolwiek zostanie połączone.
        /// </summary>
        /// <param name="punkty">Punkty reprezentujące klastry.</param>
        private void UmieśćPunktyNaLiście(double[] punkty)
        {
            // skoro mam narysowane punkty to mam też "zerowy" poziom wykresu
            dendrogram.Add(new List<GrupaNaLiście>());
            for (int i = 0; i < punkty.Length; ++i)
            {
                RysujPołączenie(new JednoPołączenie()
                {
                    IndeksOd = i,
                    IndeksDo = i,
                    PoziomZagłębienia = 0
                });
            }
        }

        private void RysujTłoDlaWykresu()
        {
            Rectangle r = new Rectangle()
            {
                Fill = Brushes.GhostWhite,
                Width = rozmiarPłótna.Width - margines*2 - początekWykresuOdLewej,
                Height = wysokośćWykresu
            };
            r.SetValue(Canvas.LeftProperty, początekWykresuOdLewej);
            r.SetValue(Canvas.TopProperty, początekWykresuOdGóry);
            listaKształtówDoWykresu.Add(r);
        }

        /// <summary>
        /// Tworzy etykiety osi Y na wykresie
        /// </summary>
        /// <param name="punkty">Punkty określające etykiety</param>
        /// <returns>Wysokość wykresu jaki się narysował</returns>
        private int RysujEtykietyIZwróćIchSzerokość(ref double[] punkty)
        {
            int największaDługośćLiczby = 0;
            for (int i = 0; i < punkty.Length; ++i)
            {
                var stringZLiczby = punkty[i].ToString();
                if (stringZLiczby.Length > największaDługośćLiczby)
                    największaDługośćLiczby = stringZLiczby.Length;
                TextBlock t = new TextBlock
                {
                    Text = stringZLiczby,
                    FontSize = 14,
                    Height = odległośćMiędzyPunktamiNaOsiY,
                    Foreground = Brushes.DeepPink
                };
                t.SetValue(Canvas.TopProperty, i * odległośćMiędzyPunktamiNaOsiY + początekWykresuOdGóry + 6);
                t.SetValue(Canvas.LeftProperty, margines);
                listaKształtówDoWykresu.Add(t);
            }
            return największaDługośćLiczby;
        }

        /// <summary>
        /// Rysuje jedno połączenie na dendrogramie.
        /// </summary>
        /// <param name="połączenie">Struktura gdzie jest napisane odkąd dokąd i w którym miejscu.</param>
        public void RysujPołączenie(JednoPołączenie połączenie)
        {
            ++krok;
            double x1, x2, x3, y1, y2;
            if (połączenie.PoziomZagłębienia == 0)
            {
                x1 = połączenie.PoziomZagłębienia*odległośćMiędzyGrupamiNaOsiX + początekWykresuOdLewej;
                x2 = x1 + odległośćMiędzyGrupamiNaOsiX;
                x3 = x1;
                y1 = początekWykresuOdGóry + połączenie.IndeksOd*odległośćMiędzyPunktamiNaOsiY;
                y2 = początekWykresuOdGóry + (połączenie.IndeksDo + 1) * odległośćMiędzyPunktamiNaOsiY;
            }
            else
            {
                GrupaNaLiście grupa1 = ZnajdźGrupęPierwszą(połączenie);
                GrupaNaLiście grupa2 = ZnajdźGrupęDrugą(połączenie);

                x1 = (grupa1.Poziom+1) * odległośćMiędzyGrupamiNaOsiX + początekWykresuOdLewej;
                x3 = (grupa2.Poziom+1) * odległośćMiędzyGrupamiNaOsiX + początekWykresuOdLewej;
                x2 = (połączenie.PoziomZagłębienia + 1) * odległośćMiędzyGrupamiNaOsiX + początekWykresuOdLewej;
                y1 = (grupa1.MiejsceOd + grupa1.MiejsceDo)/2;
                y2 = (grupa2.MiejsceOd + grupa2.MiejsceDo) / 2;

                //DUPA

                Debug.WriteLine("Nowe połączenie: {0}", połączenie);
                Debug.WriteLine("Grupa1: {0}", grupa1);
                Debug.WriteLine("grupa2: {0}", grupa2);
                Debug.WriteLine("x1:{0}, x2: {1}, y1:{2}, y2:{3}", x1, x2, y1, y2);
            }

            RysujLinięTąUGóry(x1, x2, y1);
            RysujLinięTąZGóryNaDół(x2, y1, y2);
            RysujLinięTąNaDole(x3, x2, y2);

            UmieśćPołączenieNaLiście(połączenie, y1, y2);
        }

        private GrupaNaLiście ZnajdźGrupęDrugą(JednoPołączenie połączenie)
        {
            // przeszukuj kolejne poziomy od wyższego do niższego
            for (int i = połączenie.PoziomZagłębienia - 1; i >= 0; --i)
            {
                for (int j = 0; j < dendrogram[i].Count; ++j)
                {
                    if (dendrogram[i][j].IndeksDo == połączenie.IndeksDo)
                    {
                        return dendrogram[i][j];
                    }
                }
            }
            throw new ApplicationException();
        }

        private GrupaNaLiście ZnajdźGrupęPierwszą(JednoPołączenie połączenie)
        {
            // przeszukuj kolejne poziomy od wyższego do niższego
            for (int i = połączenie.PoziomZagłębienia - 1; i >= 0; --i)
            {
                for (int j = 0; j < dendrogram[i].Count; ++j)
                {
                    if (dendrogram[i][j].IndeksOd == połączenie.IndeksOd)
                    {
                        return dendrogram[i][j];
                    }
                }
            }
            throw new ApplicationException();
        }

        private void RysujLinięTąUGóry(double x1, double x2, double y1)
        {
            Line l = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y1,
                StrokeThickness = 1.0,
                Stroke = Brushes.Black
            };
            listaKształtówDoWykresu.Add(l);
        }

        private void RysujLinięTąZGóryNaDół(double x2, double y1, double y2)
        {
            Line l = new Line()
            {
                X1 = x2,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeThickness = 1.0,
                Stroke = Brushes.Black
            };
            listaKształtówDoWykresu.Add(l);
        }

        private void RysujLinięTąNaDole(double x3, double x2, double y2)
        {
            Line l = new Line()
            {
                X1 = x3,
                Y1 = y2,
                X2 = x2,
                Y2 = y2,
                StrokeThickness = 1.0,
                Stroke = Brushes.Black
            };
            listaKształtówDoWykresu.Add(l);
        }

        /// <summary>
        /// Umieszcza połączenie na liście aby kolejne połączenia mogły na podstawie tego wyliczyć
        /// gdzie mają się narysować.
        /// </summary>
        /// <param name="połączenie"></param>
        private void UmieśćPołączenieNaLiście(JednoPołączenie połączenie, double y1, double y2)
        {
            if (połączenie.PoziomZagłębienia > dendrogram.Count-1)
            {
                for (int i = dendrogram.Count; i <= połączenie.PoziomZagłębienia; ++i)
                {
                    dendrogram.Add(new List<GrupaNaLiście>());
                }
            }
            dendrogram[połączenie.PoziomZagłębienia].Add(new GrupaNaLiście
            {
                IndeksOd = połączenie.IndeksOd,
                IndeksDo = połączenie.IndeksDo,
                MiejsceOd = y1,
                MiejsceDo = y2,
                Poziom = połączenie.PoziomZagłębienia
            });
        }

    }
}
