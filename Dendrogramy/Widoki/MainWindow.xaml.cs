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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dendrogramy.ViewModele;

namespace Dendrogramy.Widoki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void WybierzPlikDanych_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog okienko = new Microsoft.Win32.OpenFileDialog();
            okienko.DefaultExt = ".txt";
            okienko.Filter = "Text Files (*.txt)|*.txt";
            bool? wynik = okienko.ShowDialog();
            if (wynik != true) return;
            string nazwaPliku = okienko.FileName;
            NazwaPlikuDanych.Text = nazwaPliku;
        }
    }
}
