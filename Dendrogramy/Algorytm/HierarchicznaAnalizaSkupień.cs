using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dendrogramy.Enumy;

namespace Dendrogramy.Algorytm
{
    class HierarchicznaAnalizaSkupień
    {
        private MetodaSkupień metoda;
        private string nazwa;

        public HierarchicznaAnalizaSkupień(string nazwa, MetodaSkupień metoda)
        {
            this.nazwa = nazwa;
            this.metoda = metoda;
        }
    }
}
