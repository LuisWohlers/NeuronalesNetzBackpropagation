using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    class AusgabeNormal : IAusgabe
    {
        public void BerechneWert(Neuron neuron)
        {
            neuron.Ausgabe = neuron.Aktivierung;
        }
    }
}
