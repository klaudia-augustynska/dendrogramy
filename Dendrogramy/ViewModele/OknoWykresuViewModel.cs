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
        private MetodaSkupieñ metoda;
        private HierarchicznaAnalizaSkupieñ algorytm;

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

            CoœJestMielone = true;
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CoœJestMielone = false;
                }));
            });
        }
    }
}