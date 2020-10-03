using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    class Schwellenwert : IAusgabe
    {
        public void BerechneWert(Neuron neuron)
        {
            if (neuron.Aktivierung > 0.5) neuron.Ausgabe = 1.0;
            else neuron.Ausgabe = 0.0;
        }
    }
}
