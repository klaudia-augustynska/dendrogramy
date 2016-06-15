using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrogramy.Algorytm
{
    /// <summary>
    /// Struktura zwracana przez algorytm celem zakomunikowania, które grupy z poprzedniego poziomu należy połączyć na którym poziomie.
    /// </summary>
    public struct JednoPołączenie
    {
        public int IndeksOd;
        public int IndeksDo;
        public int PoziomZagłębienia;
    }
}
