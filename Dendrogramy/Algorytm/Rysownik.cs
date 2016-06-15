using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Size rozmiarPłótna;
        private ObservableCollection<UIElement> listaKształtówDoWykresu;

        private double odległośćMiędzyPunktamiNaOsiY = 30.0;
        private double margines = 15.0;
        private double wysokośćWykresu = 0;
        private double szerokośćObszaruNaEtykietyPunktów = 0;
        private double początekWykresu = 0;

        private List<List<Tuple<int,int>>> dendrogram = new List<List<Tuple<int, int>>>(); 

        public Rysownik(Size rozmiarPłótna, ObservableCollection<UIElement> listaKształtówDoWykresu)
        {
            this.rozmiarPłótna = rozmiarPłótna;
            this.listaKształtówDoWykresu = listaKształtówDoWykresu;
        }

        public double NarysujPunktyIZwróćWysokośćWykresu(double[] punkty)
        {
            wysokośćWykresu = punkty.Length*odległośćMiędzyPunktamiNaOsiY;
            
            UmieśćPunktyNaLiście(punkty);

            var największaDługośćLiczby = RysujEtykietyIZwróćIchSzerokość(ref punkty);

            szerokośćObszaruNaEtykietyPunktów = największaDługośćLiczby*10;
            początekWykresu = margines + szerokośćObszaruNaEtykietyPunktów;

            RysujTłoDlaWykresu();

            return wysokośćWykresu + 2*margines;
        }

        private void UmieśćPunktyNaLiście(double[] punkty)
        {
            // skoro mam narysowane punkty to mam też "zerowy" poziom wykresu
            dendrogram.Add(new List<Tuple<int, int>>());
            for (int i = 0; i < punkty.Length; ++i)
                dendrogram[0].Add(new Tuple<int, int>(i, i));
        }

        private void RysujTłoDlaWykresu()
        {
            Line l = new Line()
            {
                X1 = początekWykresu,
                Y1 = margines,
                X2 = początekWykresu,
                Y2 = wysokośćWykresu+margines,
                StrokeThickness = 1.0,
                Stroke = Brushes.Black
            };
            listaKształtówDoWykresu.Add(l);

            Rectangle r = new Rectangle()
            {
                Fill = Brushes.GhostWhite,
                Width = rozmiarPłótna.Width - margines - początekWykresu,
                Height = wysokośćWykresu
            };
            r.SetValue(Canvas.LeftProperty, początekWykresu);
            r.SetValue(Canvas.TopProperty, margines);
            listaKształtówDoWykresu.Add(r);
        }

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
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.DeepPink
                };
                t.SetValue(Canvas.TopProperty, i * odległośćMiędzyPunktamiNaOsiY + margines);
                t.SetValue(Canvas.LeftProperty, margines);
                listaKształtówDoWykresu.Add(t);
            }
            return największaDługośćLiczby;
        }

        public void RysujPołączenie(JednoPołączenie połączenie)
        {
            UmieśćPołączenieNaLiście(połączenie);

            throw new NotImplementedException();
        }

        private void UmieśćPołączenieNaLiście(JednoPołączenie połączenie)
        {
            if (połączenie.PoziomZagłębienia > dendrogram.Count)
            {
                for (int i = dendrogram.Count; i <= połączenie.PoziomZagłębienia; ++i)
                {
                    dendrogram.Add(new List<Tuple<int, int>>());
                }
            }
            dendrogram[połączenie.PoziomZagłębienia].Add(new Tuple<int, int>(połączenie.IndeksOd, połączenie.IndeksDo));
        }
    }
}
