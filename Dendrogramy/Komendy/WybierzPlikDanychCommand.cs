using System;
using System.Windows.Input;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Komendy
{
    /// <summary>
    /// Obsługa przycisku "wybierz plik" w oknie głównym programu (MainWindow).
    /// </summary>
    public class WybierzPlikDanychCommand : ICommand
    {
        private readonly MainWindowViewModel vm;

        public WybierzPlikDanychCommand(MainWindowViewModel vm)
        {
            this.vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Microsoft.Win32.OpenFileDialog okienko = new Microsoft.Win32.OpenFileDialog();
            okienko.DefaultExt = ".txt";
            okienko.Filter = "Text Files (*.txt)|*.txt";
            bool? wynik = okienko.ShowDialog();
            if (wynik != true) return;
            string nazwaPliku = okienko.FileName;
            vm.NazwaPlikuDanych = nazwaPliku;
        }

        public event EventHandler CanExecuteChanged;
    }
}
