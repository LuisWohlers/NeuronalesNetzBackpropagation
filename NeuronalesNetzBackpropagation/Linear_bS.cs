using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    class Linear_bS : IAktivierung
    {
        public void BerechneWert(Neuron neuron)
        {
            double netinput = neuron.NettoInput;
            if (netinput >= -1 && netinput <= 1) neuron.Aktivierung = netinput;
            else if (netinput < -1) neuron.Aktivierung = -1.0;
            else if (netinput > 1) neuron.Aktivierung = 1.0;
        }
    }
}
