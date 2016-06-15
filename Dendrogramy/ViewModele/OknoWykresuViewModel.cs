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
        public MetodaSkupieñ metoda;
        public double[] punkty;
        public Rysownik rysownik;

        private bool _coœJestMielone = false;

        public bool CoœJestMielone
        {
            get { return _coœJestMielone; }
            set
            {
                _coœJestMielone = value;
                RysujKolejnePo³¹czenieDendrogramu.RaiseCanExecuteChanged();
            }
        }

        public RysujKolejnePo³¹czenieDendrogramuCommand RysujKolejnePo³¹czenieDendrogramu { get; private set; }

        public OknoWykresuViewModel(string nazwa, MetodaSkupieñ metoda)
        {
            this.nazwa = nazwa;
            this.metoda = metoda;
            RysujKolejnePo³¹czenieDendrogramu = new RysujKolejnePo³¹czenieDendrogramuCommand(this);
        }

        public void WykonajJak¹œD³u¿sz¹Operacjê(Action action)
        {
            CoœJestMielone = true;
            Task.Run(() =>
            {
                action.Invoke();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CoœJestMielone = false;
                }));
            });
        }

        private ObservableCollection<UIElement> _listaKszta³tówDoWykresu = new ObservableCollection<UIElement>();

        public ObservableCollection<UIElement> ListaKszta³tówDoWykresu
        {
            get { return _listaKszta³tówDoWykresu; }
            set { _listaKszta³tówDoWykresu = value; }
        }

        private Size _rozmiarP³ótna;
        public Size RozmiarP³ótna
        {
            get { return _rozmiarP³ótna; }
            set
            {
                _rozmiarP³ótna = value;

                WykonajJak¹œD³u¿sz¹Operacjê(async () =>
                {
                    var parser = new ParserDanych(nazwa);
                    punkty = await parser.WczytajDane();
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    { 
                        rysownik = new Rysownik(_rozmiarP³ótna, _listaKszta³tówDoWykresu);
                        double wysokoœæ = rysownik.NarysujPunktyIZwróæWysokoœæWykresu(punkty);
                        WysokoœæWykresu = wysokoœæ;
                    }));
                });
            }
        }

        private double _wysokoœæWykresu;

        public double WysokoœæWykresu
        {
            get { return _wysokoœæWykresu; }
            set
            {
                _wysokoœæWykresu = value;
                NotifyPropertyChanged("WysokoœæWykresu");
            }
        }
    }
}