using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ReadFile
    {
        public KnowledgeBase KB;
        public List<String> clauses = new List<string>();
        public string query;
        public string fileUsed; //fileUsed

        public ReadFile(string fileUsedInput) 
        {
            fileUsed = "test_HornKB.txt"; //default filename
            if (fileUsedInput != String.Empty)
            {
                this.fileUsed = fileUsedInput;
            }
            CastKB();
            InterpretFile();
        }
        
        public void CastKB() //begin reading file to generate KB
        {
            int fileLength = File.ReadLines(fileUsed).Count();
            StreamReader sr = new StreamReader(fileUsed);
            string KB = null; // temp var 
            string q = null; // temp var
            for (int i = 0; i < fileLength; i++)
            {
                string line = sr.ReadLine();
                if (line == "TELL")
                {
                    KB = sr.ReadLine().Trim(); //Line after TELL
                }
                else if (line == "ASK")
                {
                    q = sr.ReadLine().Trim(); //Line after ASK
                }
            }
            sr.Close();
            if (KB != null && q != null)
            {
                KB = KB.Replace(" ", "");
                string[] KBArray = KB.Split(';');

                for (int j = 0; j < KBArray.Length; j++)
                {
                    if (KBArray[j] != "") //for space (?)
                    {
                        clauses.Add(KBArray[j]);
                    }
                }
                query = q;
            }
        }

        public void InterpretFile() //interpret clauses 
        {
            List<string> temp = new List<string>();
            if (this.clauses.Count > 0)
            {
                foreach (string s in this.clauses)
                {
                    //for bidirectional
                    if (s.Contains("<=>"))
                    {
                        string[] tempArray = s.Split(new string[] { "<=>" }, StringSplitOptions.None);

                        string premise = tempArray[0];
                        string conclusion = tempArray[1];

                        string forwardClause = premise + "=>" + conclusion;
                        string backwardClause = conclusion + "=>" + premise;

                        List<string> fbclauses = new List<string>();
                        fbclauses.Add(forwardClause);
                        fbclauses.Add(backwardClause);

                        temp.AddRange(fbclauses);
                    }
                    else
                    {
                        temp.Add(s);
                    }
                }
            }

            List<Clause> tempClauses = new List<Clause>();
            foreach (string s in temp)
            {
                ExtractSymbols p = new ExtractSymbols(s);
                Clause tempClause = new Clause(p.symbols);
                tempClauses.Add(tempClause);
            }
            KB = new KnowledgeBase(tempClauses); //create KB
        }
         
    }
}
