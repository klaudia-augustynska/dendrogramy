using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Dendrogramy.Algorytm;
using Dendrogramy.Enumy;
using Dendrogramy.Komendy;
using Dendrogramy.Widoki;

namespace Dendrogramy.ViewModele
{
    /// <summary>
    /// ViewModel dla okna z wykresem. Komunikuje klas� licz�c� algorytm z klas� zajmuj�c� si� rysowaniem. 
    /// Wyniki zapisuje do kolekcji przekazywanej dalej do widoku.
    /// </summary>
    public class OknoWykresuViewModel : ViewModelBase
    {
        private string nazwa;
        public MetodaSkupie� metoda;
        public double[] punkty;
        public Rysownik rysownik;

        private bool _co�JestMielone = false;
        /// <summary>
        /// Na podstawie tej w�a�ciwo�ci wiemy, czy program jest w czasie wykonywania 
        /// potencjalnie d�ugiego zadania czy nie.
        /// </summary>
        public bool Co�JestMielone
        {
            get { return _co�JestMielone; }
            set
            {
                _co�JestMielone = value;
                RysujKolejnePo��czenieDendrogramu.RaiseCanExecuteChanged();
            }
        }

        public RysujKolejnePo��czenieDendrogramuCommand RysujKolejnePo��czenieDendrogramu { get; private set; }

        public OknoWykresuViewModel(string nazwa, MetodaSkupie� metoda)
        {
            this.nazwa = nazwa;
            this.metoda = metoda;
            RysujKolejnePo��czenieDendrogramu = new RysujKolejnePo��czenieDendrogramuCommand(this);
        }

        /// <summary>
        /// Automatyczne wywo�anie logiki odpowiedzialnej za zmian� interfejsu na czas wykonywania d�ugiego zadania.
        /// </summary>
        /// <param name="action">Kod, kt�ry mo�e d�u�ej zaj��.</param>
        public void WykonajJak��D�u�sz�Operacj�(Action action)
        {
            Co�JestMielone = true;
            Task.Run(() =>
            {
                action.Invoke();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Co�JestMielone = false;
                }));
            });
        }

        private ObservableCollection<UIElement> _listaKszta�t�wDoWykresu = new ObservableCollection<UIElement>();

        /// <summary>
        /// Lista kszta�t�w, kt�re mog� automatycznie wyrysowa� si� w widoku. 
        /// Dlatego klasa od rysowania posiada referencj� do tego pola.
        /// </summary>
        public ObservableCollection<UIElement> ListaKszta�t�wDoWykresu
        {
            get { return _listaKszta�t�wDoWykresu; }
            set { _listaKszta�t�wDoWykresu = value; }
        }

        private Size _rozmiarP��tna;
        /// <summary>
        /// Takie rozwi�zanie troch� na okr�tk�, chodzi o to �eby zacz�� rysowanie czegokolwiek 
        /// jak znam rozmiary okna. A te znam dopiero po utworzeniu klasy widoku i ViewModelu, 
        /// wi�c jak ta w�a�ciwo�� jest jednorazowo ustawiana to w�a�nie wtedy dowiaduj� si� o rozmiarach.
        /// </summary>
        public Size RozmiarP��tna
        {
            get { return _rozmiarP��tna; }
            set
            {
                _rozmiarP��tna = value;

                WykonajJak��D�u�sz�Operacj�(async () =>
                {
                    var parser = new ParserDanych(nazwa);
                    punkty = await parser.WczytajDane();
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    { 
                        rysownik = new Rysownik(_rozmiarP��tna, _listaKszta�t�wDoWykresu);
                        double wysoko�� = rysownik.NarysujPunktyIZwr��Wysoko��Wykresu(punkty);
                        Wysoko��Wykresu = wysoko��;
                    }));
                });
            }
        }

        private double _wysoko��Wykresu;

        /// <summary>
        /// Zapisana wysoko�� wykresu, gdy� zmienia si� dynamicznie w zale�no�ci 
        /// od ilo�ci punkt�w wykrytych w pliku.
        /// </summary>
        public double Wysoko��Wykresu
        {
            get { return _wysoko��Wykresu; }
            set
            {
                _wysoko��Wykresu = value;
                NotifyPropertyChanged("Wysoko��Wykresu");
            }
        }
    }
}