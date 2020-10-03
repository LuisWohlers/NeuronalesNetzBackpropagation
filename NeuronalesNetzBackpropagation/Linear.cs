using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    class Linear : IAktivierung
    {
        public void BerechneWert(Neuron neuron)
        {
            neuron.Aktivierung = neuron.NettoInput;
        }
    }
}
