using System;
using System.Windows.Input;
using Dendrogramy.Algorytm;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Komendy
{
    /// <summary>
    /// Obsługa przycisku powodującego pokazanie się nowego klastra na dendrogramie.
    /// </summary>
    public class RysujKolejnePołączenieDendrogramuCommand : ICommand
    {
        private readonly OknoWykresuViewModel vm;
        private HierarchicznaAnalizaSkupień algorytm;

        public RysujKolejnePołączenieDendrogramuCommand(OknoWykresuViewModel oknoWykresuViewModel)
        {
            this.vm = oknoWykresuViewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (vm.CośJestMielone)
                return false;
            if (algorytm != null)
                if (!algorytm.MożnaŁączyćSkupiska)
                    return false;
            return true;
        }

        public void Execute(object parameter)
        {
            if (algorytm == null)
            {
                if (vm.punkty == null)
                    throw new ApplicationException("Coś poszło niezgodnie z naturą");
                algorytm = new HierarchicznaAnalizaSkupień(vm.punkty,vm.metoda);
            }

            vm.CośJestMielone = true;
            JednoPołączenie połączenie = algorytm.PołączGrupy();
            if (vm.rysownik == null)
                throw new ApplicationException("Coś poszło niezgodnie z naturą");
            vm.rysownik.RysujPołączenie(połączenie);
            vm.CośJestMielone = false;
        }

        public event EventHandler CanExecuteChanged;
        
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
