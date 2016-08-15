using System.ComponentModel;

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
