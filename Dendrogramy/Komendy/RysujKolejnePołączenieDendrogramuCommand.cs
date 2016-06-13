using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Komendy
{
    public class RysujKolejnePołączenieDendrogramuCommand : ICommand
    {
        private OknoWykresuViewModel vm;

        public RysujKolejnePołączenieDendrogramuCommand(OknoWykresuViewModel oknoWykresuViewModel)
        {
            this.vm = oknoWykresuViewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (vm.CośJestMielone)
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
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
