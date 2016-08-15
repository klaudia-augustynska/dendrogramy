using System.Collections.Generic;
using System.Windows.Input;
using Dendrogramy.Enumy;
using Dendrogramy.Komendy;

namespace Dendrogramy.ViewModele
{
    /// <summary>
    /// ViewModel okna głównego. Pozwala wybrać plik i metodę, a także kontroluje poprawność danych i widoczność przycisków.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ZbudujNowyDendrogramCommand = new ZbudujNowyDendrogramCommand(this);
            WybierzPlikDanych = new WybierzPlikDanychCommand(this);
        }

        public Dictionary<MetodaSkupień, string> ListaMetodSkupień { get; private set; } = new Dictionary<MetodaSkupień, string>
        {
            { MetodaSkupień.PojedynczegoPołączenia, "Pojedynczego połączenia" },
            { MetodaSkupień.CałkowitegoPołączenia, "Całkowitego połączenia" },
            { MetodaSkupień.ŚrednichGrupowych, "Średnich grupowych" },
            { MetodaSkupień.CentroidalnegoPołączenia, "Centroidalnego połączenia" }
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
