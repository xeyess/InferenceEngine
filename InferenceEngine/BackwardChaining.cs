using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class BackwardChaining
    {
        //set of entailed 
        public HashSet<string> entailed = new HashSet<string>();
        public bool isEntails = false;
       
        public BackwardChaining(KnowledgeBase KB, string query)
        {
            Start(KB, query);
            Output();
        }

        public void Start(KnowledgeBase KB, string query)
        {
            //get info from KB
            List<Symbol> symbols = KB.Symbols();
            List<Clause> clauses = KB.clauses;

            //init premise from opposite of FC
            Dictionary<Clause, int> premiseCount = new Dictionary<Clause, int>();
            foreach (Clause c in clauses)
            {
                premiseCount.Add(c, c.Conclusion().Count);
            }

            //init infer (false)
            Dictionary<Symbol, bool> inferred = new Dictionary<Symbol, bool>();

            //init agenda (true)
            List<Symbol> agenda = new List<Symbol>();
            foreach (Symbol s in symbols)
            {
                inferred.Add(s, false);
                if (s.symbol == query)
                {
                    agenda.Add(s);
                }
            }

            while (agenda.Count > 0)
            {
                int index = agenda.Count - 1; //index to pop
                Symbol temp = agenda[index];
                agenda.RemoveAt(index);

                foreach (Symbol s in symbols)
                {
                    if (temp.symbol == s.symbol) //if query found in agenda
                    {
                        isEntails = true;
                        break;
                    }
                }

                if (!inferred[temp])
                {
                    inferred[temp] = true; //change infer from false to true
                    entailed.Add(temp.symbol); //add symbol to entailed

                    foreach (Clause c in clauses)
                    {
                        if (c.Conclusion().ContainsKey(temp.symbol))
                        {
                            premiseCount[c] = premiseCount[c] - 1;
                            if (premiseCount[c] == 0)
                            {
                                agenda.Add(c.PremiseList()[0]);  //push
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
                List<string> reverse = entailed.ToList(); //get list
                reverse.Reverse(); //and reverse the set
                foreach (string s in reverse) //then store into string to output
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
