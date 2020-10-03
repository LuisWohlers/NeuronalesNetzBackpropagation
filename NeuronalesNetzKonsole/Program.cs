using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NeuronalesNetz;

namespace NeuronalesNetzKonsole
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuronalesNetz.NeuronalesNetz netz = new NeuronalesNetz.NeuronalesNetz();
            int[] nps = new int[] { 7,15,4 };
            netz.GeneriereNetz(3, nps);
            //netz.TestMatrixAusgeben();

            //netz.Gewichtsmatrix = Gewichtsmatrix.Create("matrix1234");

            List<ITrainingsMuster> listetm = new List<ITrainingsMuster>();

            //AND, OR, XOR //Funktioniert z.B. mit 2,X,3 -Netz
            /*double[] ev = new double[2] { 1,0 };
            double[] tv = new double[3] { 0,1,1 };
            Trainingsmuster tm = new Trainingsmuster(ev,tv);
            listetm.Add(tm);

            
            double[] ev1 = new double[2] { 0,1 };
            double[] tv1 = new double[3] { 0,1,1 };
            Trainingsmuster tm1 = new Trainingsmuster(ev1, tv1);
            listetm.Add(tm1);

            double[] ev3 = new double[2] { 1,1 };
            double[] tv3 = new double[3] { 1,1,0 };
            Trainingsmuster tm3 = new Trainingsmuster(ev3, tv3);
            listetm.Add(tm3);

            double[] ev2 = new double[2] { 0,0 };
            double[] tv2 = new double[3] { 0,0,0 };
            Trainingsmuster tm2 = new Trainingsmuster(ev2, tv2);
            listetm.Add(tm2);

            netz.Trainieren(100000,0.3,0.001,listetm);

            double[] eee = new double[2] { 1,0 };
            double[] aus = netz.AusgabeErzeugen(eee);
            double[] eee1 = new double[2] { 1,1 };
            double[] aus1 = netz.AusgabeErzeugen(eee1);

            double[] eee2 = new double[2] { 0, 1 };
            double[] aus2 = netz.AusgabeErzeugen(eee2);
            double[] eee3 = new double[2] { 0, 0 };
            double[] aus3 = netz.AusgabeErzeugen(eee3);

            Console.WriteLine(aus[0]+" "+aus[1]+" "+aus[2]+"\n"+aus1[0]+" "+aus1[1]+" "+aus1[2]);
            Console.WriteLine(aus2[0] + " " + aus2[1] + " " + aus2[2]+"\n" + aus3[0] + " " + aus3[1]+" "+aus3[2]);*/


            //netz.TestMatrixAusgeben();
            //A: links G:rechts
            double[] ev0 = new double[7] {0,0,0,0,0,0,0};//0
            double[] ev1 = new double[7] {0,1,1,0,0,0,0};//1
            double[] ev2 = new double[7] {1,1,0,1,1,0,1};//2
            double[] ev3 = new double[7] {1,1,1,1,0,0,1};//3
            double[] ev4 = new double[7] {0,1,1,0,0,1,1};//4
            double[] ev5 = new double[7] {1,0,1,1,0,1,1};//5
            double[] ev6 = new double[7] {1,0,1,1,1,1,1};//6
            double[] ev7 = new double[7] {1,1,1,0,0,0,0};//7
            double[] ev8 = new double[7] {1,1,1,1,1,1,1};//8
            double[] ev9 = new double[7] {1,1,1,1,0,1,1};//9

            double[] tv0 = new double[4] { 0, 0, 0, 0 };
            double[] tv1 = new double[4] { 0, 0, 0, 1 };
            double[] tv2 = new double[4] { 0, 0, 1, 0 };
            double[] tv3 = new double[4] { 0, 0, 1, 1 };
            double[] tv4 = new double[4] { 0, 1, 0, 0 };
            double[] tv5 = new double[4] { 0, 1, 0, 1 };
            double[] tv6 = new double[4] { 0, 1, 1, 0 };
            double[] tv7 = new double[4] { 0, 1, 1, 1 };
            double[] tv8 = new double[4] { 1, 0, 0, 0 };
            double[] tv9 = new double[4] { 1, 0, 0, 1 };

            Trainingsmuster tm0 = new Trainingsmuster(ev0, tv0);
            Trainingsmuster tm1 = new Trainingsmuster(ev1, tv1);
            Trainingsmuster tm2 = new Trainingsmuster(ev2, tv2);
            Trainingsmuster tm3 = new Trainingsmuster(ev3, tv3);
            Trainingsmuster tm4 = new Trainingsmuster(ev4, tv4);
            Trainingsmuster tm5 = new Trainingsmuster(ev5, tv5);
            Trainingsmuster tm6 = new Trainingsmuster(ev6, tv6);
            Trainingsmuster tm7 = new Trainingsmuster(ev7, tv7);
            Trainingsmuster tm8 = new Trainingsmuster(ev8, tv8);
            Trainingsmuster tm9 = new Trainingsmuster(ev9, tv9);

            listetm.Add(tm0);
            listetm.Add(tm1);
            listetm.Add(tm2);
            listetm.Add(tm3);
            listetm.Add(tm4);
            listetm.Add(tm5);
            listetm.Add(tm6);
            listetm.Add(tm7);
            listetm.Add(tm8);
            listetm.Add(tm9);


            Console.WriteLine(netz.Trainieren(1000000, 0.3, 0.001, listetm));

            List<double[]> ausgabelist = new List<double[]>();

            foreach(Trainingsmuster tm in listetm)
            {
                double[] ausgabe = netz.AusgabeErzeugen(tm.EingabeVektor);
                ausgabelist.Add(ausgabe);
            }

            foreach (double[] d in ausgabelist)
            {
                double aus = 0.0;
                for (int i = 0; i < d.Count(); i++)
                {
                    aus += d[i] * Math.Pow(2.0, (double)(d.Count() - 1 - i));
                    Console.Write(d[i] + " ");
                }
                Console.WriteLine(" : " + aus +"\n");

            }
            Console.ReadLine();

            //Console.WriteLine(netz.Gewichtsmatrix.Speichern("matrix1234"));

           // Console.WriteLine(Trainingsmuster.Speichern("Musterliste", listetm));

            Console.ReadLine();
        }
    }
}
