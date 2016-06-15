using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrogramy.ViewModele
{
    /// <summary>
    /// Klasa bazowa dla ViewModeli właściwych, aby nie pisać w kółko tego samego.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
    new PropertyChangedEventArgs(propertyName));
        }
    }
}
