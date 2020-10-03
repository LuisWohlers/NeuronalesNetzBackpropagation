using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    class Sigmoid : IAktivierung
    {
        public void BerechneWert(Neuron neuron)
        {
            neuron.Aktivierung = 1 / (1 + Math.Exp(-neuron.NettoInput));
        }
    }
}
