using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    public interface IAktivierung
    {
        void BerechneWert(Neuron neuron);
    }
}
