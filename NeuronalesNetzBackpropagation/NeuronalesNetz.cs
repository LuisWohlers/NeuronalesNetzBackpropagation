using System;
using System.Collections.Generic;
using NeuronaleNetzeInterfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace NeuronalesNetz
{
    public class NeuronalesNetz : INeuronalesNetz
    {
        //Liste von Listen von Neuronen: Das Netz
        List<List<Neuron>> _netzneuronen;
        public List<List<Neuron>> NetzNeuronen {
            get => _netzneuronen;
            set => _netzneuronen = value; 
        }

        //Die Gewichtsmatrix (Typ: Gesamtmatrix)
        Gewichtsmatrix _gewichtsmatrix;
        public Gewichtsmatrix Gewichtsmatrix {
            get => _gewichtsmatrix;
            set => _gewichtsmatrix = value;
        }

        //"Kopie" der Gewichtsmatrix, von der Initialisierung in "GeneriereNetz" verwendet
        Gewichtsmatrix _gewichtsmatrixKopie;
        public Gewichtsmatrix GewichtsmatrixKopie
        {
            get => _gewichtsmatrixKopie;
            set => _gewichtsmatrixKopie = value;
        }

        //Ausgabe des Netzes aus Eingangsvektor von Doubles erzeugen (Vorwärts propagieren)
        public double[] AusgabeErzeugen(double[] eingabevektor)
        {
            double[] ausgabe = new double[_netzneuronen[_netzneuronen.Count()-1].Count()];
            foreach(List<Neuron> y in _netzneuronen)
            {
                foreach(Neuron x in y)
                {
                    x.NettoInput = 0;
                }
            }
            //NettoInput + Ausgabe der Eingabeneuronen setzen;
            for(int i=0; i <_netzneuronen[0].Count(); i++)
            {
                _netzneuronen[0][i].NettoInput = eingabevektor[i];
                _netzneuronen[0][i].AktivierungsFunktion();
                _netzneuronen[0][i].AusgabeFunktion();
            }

            //Weitere Schichten bearbeiten
            for(int i = 1; i < _netzneuronen.Count(); i++)
            {
                for(int j = 0; j < _netzneuronen[i].Count(); j++)
                {
                    for(int a = 0; a < _netzneuronen[i - 1].Count(); a++)
                    {
                        _netzneuronen[i][j].NettoInput += _netzneuronen[i - 1][a].Ausgabe 
                            * _gewichtsmatrix[ _netzneuronen[i-1][a].Index, _netzneuronen[i][j].Index];
                    }
                    _netzneuronen[i][j].AktivierungsFunktion();
                    _netzneuronen[i][j].AusgabeFunktion();
                    if (i == _netzneuronen.Count()-1)
                    {
                        ausgabe[j] = _netzneuronen[i][j].Ausgabe;
                    }
                }
            }

            return ausgabe;

        }

        //Trainieren des Netzes durch Backpropagation (Fehlerrückführung)
        //gibt 1 bei Erfolg zurück (Erfolg heißt: Training beendet unter maximal-Anzahl von Schritten)
        public int Trainieren(int anzahlLernschritte, double lernrate, double toleranz, List<ITrainingsMuster> trainingsMuster)
        {
            int count = 0;

            bool trainiert = false;//false: es wird/wurde nicht trainiert, true: es wird/wurde trainiert
            for(int schritte = 0; schritte<anzahlLernschritte; schritte++)
            {
                count++;
                //List<List<double>> netzdeltas = new List<List<double>>();
                trainiert = false;
                for(int tm = 0; tm < trainingsMuster.Count(); tm++)
                {
                    List<List<double>> netzdeltas = new List<List<double>>();
                    double[] ausgabe = AusgabeErzeugen(trainingsMuster[tm].EingabeVektor);
                    //Ausgabe vergleichen
                    int aus_s = _netzneuronen.Count() - 1;//"Index" der Ausgabeschicht
                    int aus_n = _netzneuronen[aus_s].Count();//Anzahl Ausgabeneuronen
                    List<double> deltas = new List<double>();

                    for(int aus = 0; aus < aus_n; aus++)
                    {
                        //Ableitung der Aktivierungsfunktion nutzen (o*(1-o))
                        double diff = _netzneuronen[aus_s][aus].Aktivierung*(1 - _netzneuronen[aus_s][aus].Aktivierung);
                        double this_delta = diff*(trainingsMuster[tm].Targetvektor[aus] - _netzneuronen[aus_s][aus].Aktivierung);
                        deltas.Add(this_delta);
                        if (Math.Abs(this_delta) > toleranz)
                        {
                            trainiert = true;
                        }
                    }
                    netzdeltas.Add(deltas);
                    //wenn ein Delta der Ausgabeschicht ausserhalb der Toleranz war, wird jetzt trainiert
                    //=>Fehlerrückführung
                    if (trainiert)
                    {
                        //Gewichtsänderung: /\W|u->p = Lernrate * Delta|u * out|p MIT u: betrachtetes Neuron, p: predecessor

                        //Fehlerrückübertragung: Delta|u = SUMME(Delta|s * W|s->u) * Lambda|u MIT s: successor
                        int p= 0;
                        for(int i = _netzneuronen.Count()-2; i>0; i--)
                        {
                            List<double> deltas_i = new List<double>();
                            for(int j = 0; j < _netzneuronen[i].Count(); j++)
                            {
                                double abl = _netzneuronen[i][j].Ausgabe * (1 - _netzneuronen[i][j].Ausgabe);
                                double sum = 0.0;
                                int zeile = _netzneuronen[i][j].Index;
                                for (int z = 0; z < _netzneuronen[i + 1].Count(); z++)
                                {
                                    int spalte = _netzneuronen[i + 1][z].Index;
                                    sum += netzdeltas[p][z] * _gewichtsmatrix[zeile,spalte];
                                }
                                deltas_i.Add(abl * sum);                               
                            }
                            netzdeltas.Add(deltas_i);
                            p++;
                        }

                        //Deltsa-Liste umkehren, um selbe Struktur zu haben wie Neuronen-Liste
                        netzdeltas.Reverse();

                        //Gewichtswert-Deltas
                        
                        for(int i = _netzneuronen.Count()-1; i>0;  i--)
                        {
                            for(int j = 0; j < _netzneuronen[i].Count(); j++)
                            {
                                //jeweiliger Fehler aus umgekehrter Deltaliste
                                double delta = netzdeltas[i-1][j];

                                int zeile = _netzneuronen[i][j].Index;

                                for(int z = 0; z < _netzneuronen[i - 1].Count(); z++)
                                {
                                    int spalte = _netzneuronen[i - 1][z].Index;
                                    //jeweiliger Gewichtsmatrixeintrag
                                    double gme = _gewichtsmatrix[spalte,zeile];
                                    double gewichtsdelta = lernrate * delta * _netzneuronen[i - 1][z].Ausgabe;
                                    _gewichtsmatrix[spalte, zeile] = gme + gewichtsdelta;
                                }

                            }
                        }
                    }

                }
                if (!trainiert) break;
            }
            if (!trainiert) return count; //Training erfolgreich
            else return -1; //-1: Training nicht erfolgreich
        }

        //Gibt auf der Konsole die Gewichtsmatrix aus (habe ich genutzt, um Initialisierung der Matrix
        //zu überprüfen, Hilfsmethode
        public void TestMatrixAusgeben()
        {
            for(int i=0; i < _gewichtsmatrix.Zeilen; i++)
            {
                for(int j = 0; j < _gewichtsmatrix.Spalten; j++)
                {
                    Console.Write("\t"+ String.Format("{0:0.00}", _gewichtsmatrix[i, j]));
                }
                Console.Write("\n");
            }
        }

        //Gibt die NettoInputs der Neuronen auf der Konsole aus (ebenfalls nur fürs Debugging verwendet)
        //Hilfsmethode
        public void NettoInputs()
        {
            for(int i = 0; i < _netzneuronen.Count(); i++)
            {
                for(int j = 0; j < _netzneuronen[i].Count(); j++)
                {
                    Console.Write(_netzneuronen[i][j].NettoInput + " ");
                }
                Console.Write("\n");
            }
        }

        //Generiert ein Netz aus einer variablen Anzahl von Schichten
        //anzahlschichten: Anzahl der Schichten (muss mindestens 2 sein)
        //neuronenProSchicht: Anzahl der Neuronen pro Schicht, in Vektor gespeichert
        //Anzahl der Neuronen pro Schicht darf nicht null sein
        public void GeneriereNetz(int anzahlschichten, int[] neuronenProSchicht)
        {
            //Neuronen erstellen, in Liste einfügen
            _netzneuronen = new List<List<Neuron>>();
            int index = 0;
            for(int s = 0; s < anzahlschichten; s++)
            {
                List<Neuron> neuronen = new List<Neuron>();
                for(int ps = 0; ps<neuronenProSchicht[s]; ps++)
                {
                    if (s == anzahlschichten - 1)
                    {
                        //Ausgabeschicht: Neuronen geben 0 aus für Aktivierung unter 0,5 , sonst 1
                        //-> Schwellenwertfunktion für binäre Ausgabe
                        neuronen.Add(new Neuron(index++, 0, new Sigmoid(), new Schwellenwert()));
                    }
                    else neuronen.Add(new Neuron(index++, 0, new Sigmoid(), new AusgabeNormal()));
                }
                _netzneuronen.Add(neuronen);
            }

            //Gewichtsmatrix + "Kopie" erzeugen
            _gewichtsmatrix = new Gewichtsmatrix(index, index);
            _gewichtsmatrixKopie = new Gewichtsmatrix(index, index);

            //"Kopie" der Matrix mit 1 initialisieren, falls Wert von 0 abweichen darf
            for(int a = 0; a < anzahlschichten-1; a++)
            {
                for (int b = 0; b < neuronenProSchicht[a]; b++)
                {
                    for (int c = 0; c < neuronenProSchicht[a+1]; c++)
                    {
                        int x = _netzneuronen[a][b].Index;
                        int y = _netzneuronen[a + 1][c].Index;
                        _gewichtsmatrixKopie[x, y] = 1;
                    }
         
                }
            }

            //wo Kopie 1 ist, Gewichtsmatrix zufällig zwischen -0 und 1 initialisieren(0.01-Schritte)
            Random rand = new Random();
            for (int y = 0; y < index; y++)
            {
                for (int x = 0; x < index; x++)
                {
                    if (_gewichtsmatrixKopie[x, y] == 1)
                    {
                        while (_gewichtsmatrix[x, y] == 0)
                        {
                            _gewichtsmatrix[x, y] = (double)rand.Next(-1000,1000)/1000;
                        }
                    }
                }
            }
        }

    }
}
