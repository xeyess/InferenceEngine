using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //For testing
            //string[] args = new string[2];
            //args = new string[2];
            //args[0] = "TT";
            //args[1] = "test_HornKB.txt";
            Console.Clear();
            string fileName = "";
            string method = "";
            Console.WriteLine("[Inference Engine]");

            //if two paramaters give (batch format : .exe method filename.txt)
            if(args.Length == 2)
            {
                if (File.Exists(args[1]))
                {
                    ReadFile rf = new ReadFile("");
                    method = args[0];
                    fileName = args[1];

                    try
                    {
                        //try to parse file
                        rf = new ReadFile(fileName);
                        BeginMethod(rf, method);
                    }
                    catch
                    {
                        Console.WriteLine("An error occured.");
                        Console.ReadLine();
                        Close();
                    }
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                    Console.ReadLine();
                    Close();
                }
            }
            else
            {
                Console.WriteLine("Argument format should be *.exe method fileName.");
                Console.ReadLine();
                Close();
            }
            
        }

        private static void BeginMethod(ReadFile rf, string method) //choose and run method
        {
            if (method == "TT")
            {
                TruthTable tt = new TruthTable(rf.KB, rf.query);
            }
            else if(method == "FC")
            {
                ForwardChaining fc = new ForwardChaining(rf.KB, rf.query);
            }
            else if(method == "BC")
            {
                BackwardChaining bc = new BackwardChaining(rf.KB, rf.query);
            }
            else
            {
                Console.WriteLine("Invalid method.");
                Console.ReadLine();
                Close();
            }
            Console.ReadLine();
        }

        private static void Close()
        {
            Environment.Exit(0);
        }
    }
}
