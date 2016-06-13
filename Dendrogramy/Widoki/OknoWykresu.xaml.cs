using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dendrogramy.Algorytm;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Widoki
{
    /// <summary>
    /// Interaction logic for OknoWykresu.xaml
    /// </summary>
    public partial class OknoWykresu : Window
    {
        private HierarchicznaAnalizaSkupień algorytm;

        public OknoWykresu(string nazwa, Enumy.MetodaSkupień metoda)
        {
            InitializeComponent();
            this.DataContext = new OknoWykresuViewModel(nazwa,metoda);
        }
    }
}
