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
    /// ViewModel dla okna z wykresem. Komunikuje klasê licz¹c¹ algorytm z klas¹ zajmuj¹c¹ siê rysowaniem. 
    /// Wyniki zapisuje do kolekcji przekazywanej dalej do widoku.
    /// </summary>
    public class OknoWykresuViewModel : ViewModelBase
    {
        private string nazwa;
        public MetodaSkupieñ metoda;
        public double[] punkty;
        public Rysownik rysownik;

        private bool _coœJestMielone = false;
        /// <summary>
        /// Na podstawie tej w³aœciwoœci wiemy, czy program jest w czasie wykonywania 
        /// potencjalnie d³ugiego zadania czy nie.
        /// </summary>
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

        /// <summary>
        /// Automatyczne wywo³anie logiki odpowiedzialnej za zmianê interfejsu na czas wykonywania d³ugiego zadania.
        /// </summary>
        /// <param name="action">Kod, który mo¿e d³u¿ej zaj¹æ.</param>
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

        /// <summary>
        /// Lista kszta³tów, które mog¹ automatycznie wyrysowaæ siê w widoku. 
        /// Dlatego klasa od rysowania posiada referencjê do tego pola.
        /// </summary>
        public ObservableCollection<UIElement> ListaKszta³tówDoWykresu
        {
            get { return _listaKszta³tówDoWykresu; }
            set { _listaKszta³tówDoWykresu = value; }
        }

        private Size _rozmiarP³ótna;
        /// <summary>
        /// Takie rozwi¹zanie trochê na okrêtkê, chodzi o to ¿eby zacz¹æ rysowanie czegokolwiek 
        /// jak znam rozmiary okna. A te znam dopiero po utworzeniu klasy widoku i ViewModelu, 
        /// wiêc jak ta w³aœciwoœæ jest jednorazowo ustawiana to w³aœnie wtedy dowiadujê siê o rozmiarach.
        /// </summary>
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

        /// <summary>
        /// Zapisana wysokoœæ wykresu, gdy¿ zmienia siê dynamicznie w zale¿noœci 
        /// od iloœci punktów wykrytych w pliku.
        /// </summary>
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