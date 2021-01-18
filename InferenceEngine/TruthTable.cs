using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable
    {
        public KnowledgeBase KB;
        public bool isEntails = false; //YES condition 
        public Clause query;
        public int count = 0;

        public TruthTable(KnowledgeBase KB, string alpha) //KB entails a statement alpha; KB => alpha
        {
            this.KB = KB;
            Symbol s = new Symbol(alpha);
            this.query = new Clause(s);
            Output(); //show message
        }

        //loop to check for alpha in KB
        public bool TTEntails(KnowledgeBase KB, Clause alpha)
        {
            List<Symbol> symbols = new List<Symbol>();

            HashSet<string> uniqueIdentities = new HashSet<string>();
            foreach(Symbol s in KB.Symbols())
            {
                uniqueIdentities.Add(s.symbol);
            }
            foreach(string str in uniqueIdentities)
            {
                symbols.Add(new Symbol(str));
            }
            return TTCheckAll(KB, alpha, symbols, new Model());
        }

        //check for KB and if true- add to counter and loop till end
        public bool TTCheckAll(KnowledgeBase KB, Clause alpha, List<Symbol> symbols, Model m)
        {
            if (symbols.Count == 0)
            {
                if(m.isKBTrue(KB.JoinClauses()))
                { 
                    if(m.isClauseTrue(alpha))
                    {
                        isEntails = m.isClauseTrue(alpha);
                        count++;
                    }
                    return isEntails;
                }
                else
                {
                    return true;
                }
            }

            Symbol s = symbols[0];
            List<Symbol> rest = symbols.GetRange(1, symbols.Count - 1);
            return TTCheckAll(KB, alpha, rest, m.Union(s, true)) && TTCheckAll(KB, alpha, rest, m.Union(s, false));
        }

        public void Output()
        {
            string result = "";
            if(this.TTEntails(this.KB, this.query) && count > 0)
            {
                result = "YES: " + count;
            }
            else
            {
                result = "NO";
            }
            Console.WriteLine(result);
        }
    }
}
