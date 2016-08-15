using System.Windows;
using System.Windows.Controls;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Widoki
{
    /// <summary>
    /// Interaction logic for OknoWykresu.xaml
    /// </summary>
    public partial class OknoWykresu : Window
    {
        private OknoWykresuViewModel vm;
        private readonly string nazwa;

        public OknoWykresu(string nazwa, Enumy.MetodaSkupień metoda)
        {
            InitializeComponent();
            this.nazwa = nazwa;
            this.DataContext = new OknoWykresuViewModel(nazwa,metoda);
            vm = (OknoWykresuViewModel) DataContext;
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            var rozmiarPłótna = ((ScrollViewer) sender).RenderSize;
            vm.RozmiarPłótna = rozmiarPłótna;
        }
    }
}
