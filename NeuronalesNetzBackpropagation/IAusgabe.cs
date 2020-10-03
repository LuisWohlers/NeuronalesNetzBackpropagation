using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    public interface IAusgabe
    {
        void BerechneWert(Neuron neuron);
    }
}
