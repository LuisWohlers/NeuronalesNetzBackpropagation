using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    public interface INeuronalesNetz
    {
        List<List<Neuron>> NetzNeuronen { get; set; }
        Gewichtsmatrix Gewichtsmatrix { get; set; }
        double[] AusgabeErzeugen(double[] eingabevektor);
        int Trainieren(int anzahlLernschritte, double lernrate, double toleranz, List<ITrainingsMuster> trainingsMuster);
        void GeneriereNetz(int anzahlschichten, int[] neuronenProSchicht);
    }
}
