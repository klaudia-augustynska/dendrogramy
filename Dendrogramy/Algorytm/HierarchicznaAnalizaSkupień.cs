using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dendrogramy.Enumy;

namespace Dendrogramy.Algorytm
{
    class HierarchicznaAnalizaSkupień
    {
        private MetodaSkupień metoda;
        private double[] liczby;
        private double[] klastry;
        public bool możnaŁączyćSkupiska = true;

        public HierarchicznaAnalizaSkupień(double[] liczby, MetodaSkupień metoda)
        {
            this.liczby = liczby;
            this.metoda = metoda;

            UmieśćKażdyObiektWOsobnymKlastrze();
            //SkonstruujMacierzOdległościMiędzyklastrowej();
        }

        private void UmieśćKażdyObiektWOsobnymKlastrze()
        {
            klastry = new double[liczby.Length];
            for (int i = 0; i < liczby.Length; ++i)
                klastry[i] = i;
        }

        private void SkonstruujMacierzOdległościMiędzyklastrowej()
        {
            throw new NotImplementedException();
        }

        public JednoPołączenie PołączGrupy()
        {
            //ZnajdźNajbliższąParęKlastrówIPołączJe();
            //UaktualnijMacierzOdległościMiędzyklastrowych();
            //if (WszystkieGrupyNależąDoJednegoKlastra())
            //    możnaŁączyćSkupiska = false;
            //return;
            return new JednoPołączenie();
        }

        private void ZnajdźNajbliższąParęKlastrówIPołączJe()
        {
            throw new NotImplementedException();
        }

        private void UaktualnijMacierzOdległościMiędzyklastrowych()
        {
            throw new NotImplementedException();
        }

        private bool WszystkieGrupyNależąDoJednegoKlastra()
        {
            throw new NotImplementedException();
        }
    }
}
