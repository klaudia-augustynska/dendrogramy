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
    public class OknoWykresuViewModel : ViewModelBase
    {
        private string nazwa;
        public MetodaSkupie� metoda;
        public double[] punkty;
        public Rysownik rysownik;

        private bool _co�JestMielone = false;

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

        public ObservableCollection<UIElement> ListaKszta�t�wDoWykresu
        {
            get { return _listaKszta�t�wDoWykresu; }
            set { _listaKszta�t�wDoWykresu = value; }
        }

        private Size _rozmiarP��tna;
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