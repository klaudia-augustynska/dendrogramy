using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dendrogramy.Algorytm;
using Dendrogramy.Enumy;
using Dendrogramy.Komendy;

namespace Dendrogramy.ViewModele
{
    public class OknoWykresuViewModel : ViewModelBase
    {
        private string nazwa;
        private MetodaSkupie� metoda;
        private HierarchicznaAnalizaSkupie� algorytm;

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

            WykonajJak��D�u�sz�Operacj�(async () =>
            {
                algorytm = new HierarchicznaAnalizaSkupie�(nazwa,metoda);
                await algorytm.WczytajDane();
            });

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
    }
}