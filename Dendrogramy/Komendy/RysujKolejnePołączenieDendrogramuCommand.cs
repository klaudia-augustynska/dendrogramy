using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dendrogramy.Algorytm;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Komendy
{
    public class RysujKolejnePołączenieDendrogramuCommand : ICommand
    {
        private OknoWykresuViewModel vm;
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
                if (!algorytm.możnaŁączyćSkupiska)
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

            algorytm.PołączGrupy();
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
