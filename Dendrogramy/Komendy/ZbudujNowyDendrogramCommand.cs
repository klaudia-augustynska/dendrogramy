using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dendrogramy.ViewModele;
using Dendrogramy.Widoki;

namespace Dendrogramy.Komendy
{
    /// <summary>
    /// Obsługa przycisku rozpoczynającego budowanie nowego dendrogramu. 
    /// Wywołuje okno rysowania wykresu z odpowiednimi parametrami.
    /// </summary>
    public class ZbudujNowyDendrogramCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainWindowViewModel vm;

        public ZbudujNowyDendrogramCommand(MainWindowViewModel vm)
        {
            this.vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            string nazwa = string.Empty;
            var values = (object[])parameter;
            if (values != null)
                nazwa = (string)values[0];
            if (nazwa != null)
            {
                if (nazwa.Length > 4)
                    return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            string nazwa = (string)values[0];
            Enumy.MetodaSkupień metoda = (Enumy.MetodaSkupień)values[1];
            vm.IsEnabled = false;
            OknoWykresu okno = new OknoWykresu(nazwa, metoda);
            okno.ShowDialog();
            vm.IsEnabled = true;
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
