using NeuronaleNetzeInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz
{
    [Serializable]
    public class Gewichtsmatrix : IGewichtsmatrix
    {
        int _spalten;
        int _zeilen;
        public int Spalten
        {
            get => _spalten;
            set => _spalten = value;
        }
        public int Zeilen
        {
            get => _zeilen;
            set => _zeilen = value;
        }
        public Enums.GewichtsmatrixModus Modus { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public double this[int x, int y] {
            get
            {
                return _matrix[x, y];
            }
            set
            {
                _matrix[x, y] = value;
            }
        }

        private double[,] _matrix;

        public string Speichern(string dateiname)
        {
            try
            {
                BinaryFormatter writer = new BinaryFormatter();
                FileStream fs =
                new FileStream(dateiname, FileMode.Create);
                writer.Serialize(fs, this);
                fs.Flush();
                fs.Close();
                return "gespeichert";
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: ", e.ToString());
                return "failed";
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public static Gewichtsmatrix Create(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);
            BinaryFormatter reader = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.Open);
            Gewichtsmatrix instanz =
            (Gewichtsmatrix)reader.Deserialize(fs);
            fs.Close();
            return instanz;
        }

        public Gewichtsmatrix(int zeilen, int spalten)
        {
            _zeilen = zeilen;
            _spalten = spalten;
            _matrix = new double[zeilen, spalten];
            for (int i = 0; i < zeilen; i++)
            {
                for (int j = 0; j < spalten; j++)
                {
                    _matrix[i, j] = 0;
                }
            }
        }
    }
}
