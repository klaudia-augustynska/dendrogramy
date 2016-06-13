using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dendrogramy.Enumy;
using Dendrogramy.Komendy;

namespace Dendrogramy.ViewModele
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ZbudujNowyDendrogramCommand = new ZbudujNowyDendrogramCommand(this);
            WybierzPlikDanych = new WybierzPlikDanychCommand(this);
        }

        public IEnumerable<Tuple<string, MetodaSkupień>> ListaMetodSkupień { get; private set; } = new List<Tuple<string, MetodaSkupień>>
        {
            new Tuple<string, MetodaSkupień>("Pojedynczego połączenia",MetodaSkupień.PojedynczegoPołączenia),
            new Tuple<string, MetodaSkupień>("Całkowitego połączenia",MetodaSkupień.CałkowitegoPołączenia),
            new Tuple<string, MetodaSkupień>("Średnich grupowych",MetodaSkupień.ŚrednichGrupowych),
            new Tuple<string, MetodaSkupień>("Centroidalnego połączenia", MetodaSkupień.CentroidalnegoPołączenia)
        };

        public ZbudujNowyDendrogramCommand ZbudujNowyDendrogramCommand { get; private set; } 

        public ICommand WybierzPlikDanych { get; private set; }

        private string _nazwaPlikuDanych = string.Empty;
        public string NazwaPlikuDanych
        {
            get { return _nazwaPlikuDanych; }
            set
            {
                _nazwaPlikuDanych = value;
                NotifyPropertyChanged("NazwaPlikuDanych");
                ZbudujNowyDendrogramCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

    }
}
