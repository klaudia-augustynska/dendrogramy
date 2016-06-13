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

            Co�JestMielone = true;
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Co�JestMielone = false;
                }));
            });
        }
    }
}