using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ForwardChaining
    {
        //set of entailed
        public HashSet<string> entailed = new HashSet<string>();
        public bool isEntails = false;

        public ForwardChaining(KnowledgeBase KB, string query)
        {
            Start(KB, query);
            Output();
        }

        public void Start(KnowledgeBase KB, string query)
        {
            //get info from KB
            List<Symbol> symbols = KB.Symbols();
            List<Clause> clauses = KB.clauses;

            //init premise set
            Dictionary<Clause, int> premiseCount = new Dictionary<Clause, int>();
            foreach (Clause c in clauses)
            {
                premiseCount.Add(c, c.Premise().Count);
            }

            //init infer set (false)
            Dictionary<Symbol, bool> inferred = new Dictionary<Symbol, bool>();
            foreach (Symbol s in symbols)
            {
                inferred.Add(s, false);
            }

            //get facts from KB (true)
            List<Symbol> agenda = KB.Facts();

            while (agenda.Count > 0)
            {
                Symbol temp = agenda[0]; //pop / remove
                agenda.RemoveAt(0);
                if (temp.symbol == query) //if query found in agenda
                {
                    isEntails = true;
                    entailed.Add(temp.symbol);
                    break;
                }

                if (!inferred[temp])
                {
                    inferred[temp] = true; //change infer value to true from false
                    entailed.Add(temp.symbol); //add entailed

                    foreach (Clause c in clauses)
                    {
                        if (c.Premise().ContainsKey(temp.symbol))
                        {
                            premiseCount[c] = premiseCount[c] - 1;
                            if (premiseCount[c] == 0)
                            {
                                foreach (Symbol s in c.Conclusion().Values)
                                {
                                    agenda.Add(s); //push
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Output()
        {
            string result;
            if(isEntails)
            {
                string entailList = ""; 
                foreach(string s in entailed) //get all entailed values stored in set
                {
                    entailList += s + ", ";
                }
                entailList = entailList.Substring(0, entailList.Length - 2); //remove last ,
                

                result = "YES: " + entailList;
            }
            else
            {
                result = "NO";
            }
            Console.WriteLine(result);
        }

    }
}
