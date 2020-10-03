using NeuronaleNetzeInterfaces.Funktionen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    public class Neuron
    {
        //Index dient zur Nummerierung der Neuronen in einem Netz
        int _index;
        public int Index
        {
            get => _index;
            set => _index = value;
        }

        double _nettoInput;
        public double NettoInput
        {
            get => _nettoInput;
            set => _nettoInput = value;
        }

        double _aktivierung;
        public double Aktivierung
        {
            get => _aktivierung;
            set => _aktivierung = value;
        }

        double _ausgabe;
        public double Ausgabe
        {
            get => _ausgabe;
            set => _ausgabe = value;
        }

        private IAktivierung AktivierungsF{ get; set; }
        private IAusgabe AusgabeF { get; set; }

        public Neuron(int index, int nettoinput, IAktivierung aktivf, IAusgabe ausf)
        {
            _nettoInput = NettoInput;
            _index = index;
            AktivierungsF = aktivf;
            AusgabeF = ausf;
        }

        public void AktivierungsFunktion()
        {
            AktivierungsF.BerechneWert(this);
        }

        public void AusgabeFunktion()
        {
            AusgabeF.BerechneWert(this);
        }


    }
}
