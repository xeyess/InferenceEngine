using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{ 
    public class Model
    {
        //model for TT 
        public Dictionary<string, bool> assignments = new Dictionary<string, bool>();
        public Model() { }

        //duplicate and assign value for entails
        public Model Union(Symbol s, bool b)
        {
            Model m = new Model();


           foreach(var assign in assignments)
            {
                m.assignments.Add(assign.Key, assign.Value);
            }

            m.assignments[s.symbol] = b;

            return m;
        }

        public bool ClauseValue(List<Symbol> symbols)
        {
            bool result = symbols[0].value;
            Connective c = symbols[0].rightConnective;
            if (c == Connective.None)
            {
                return result;
            }

            while (symbols.Count > 1)
            {
                symbols.RemoveAt(0);
                bool temp = symbols[0].value;
                if (c == Connective.Conjunction)
                {
                    result = result && temp;
                }
                else
                {
                    result = result || temp;
                }
            }

            return result;
        }

        public bool KnowledgeBaseValue(List<Clause> clauses)
        {
            bool result = clauses[0].value;
            Connective c = clauses[0].rightConnective;

            if (c == Connective.None)
            {
                return result;
            }

            while (clauses.Count > 1)
            {
                clauses.RemoveAt(0);
                bool temp = clauses[0].value;
                result = result && temp;
            }

            return result;
        }

        public bool isSymbolTrue(string symbol)
        {
            /*
            bool result;
            if (assignments.ContainsKey(symbol))
            {
                result = assignments[symbol];
            }
            return result;
            */
            return assignments[symbol];
        }

        //check if the clause is true
        public bool isClauseTrue(Clause clause)
        {
            bool result;
            bool isUnary = false; //non binary
            if(clause.symbols.Count == 1) //if only one symbol
            {
                isUnary = true;
            }
            if(isUnary)
            {
                Symbol s = clause.symbols[0];
                if (s.leftConnective == Connective.None)
                {
                    result = this.isSymbolTrue(s.symbol);
                }
                else
                {
                    result = !this.isSymbolTrue(s.symbol);
                }
                clause.value = result;
                return result;
            }
            else
            {
                List<Symbol> premiseSwM = new List<Symbol>(); //Premise symbols with model
                List<Symbol> conclusionSwM = new List<Symbol>(); //Conclustion symbols with model

                for (int i = 0; i < clause.symbols.Count; i++)
                {
                    Symbol temp = clause.symbols[i];

                    if (!temp.partOfConclusion)
                    {
                        if (temp.leftConnective == Connective.None)
                        {
                            temp.value = this.isSymbolTrue(temp.symbol);
                        }
                        else
                        {
                            temp.value = !this.isSymbolTrue(temp.symbol);
                        }

                        if (temp.rightConnective != Connective.Implication)
                        {}
                        else
                        {
                            temp.rightConnective = Connective.None;
                        }
                        premiseSwM.Add(temp);
                    }

                    else
                    {
                        if (temp.leftConnective == Connective.None)
                        {
                            temp.value = this.isSymbolTrue(temp.symbol);
                        }
                        else
                        {
                            temp.value = !this.isSymbolTrue(temp.symbol);
                        }

                        if (temp.rightConnective != Connective.None)
                        { }
                        conclusionSwM.Add(temp);
                    }
                }

                //assign values and get implication (binary) clause for value
                //bidirection is changed to implication 
                bool premiseVal = ClauseValue(premiseSwM);
                bool conclusionVal = ClauseValue(conclusionSwM);
                result = !(premiseVal && !conclusionVal);
                clause.value = result;
                return result;
            }
        }

        //check KB values
        public bool isKBTrue(List<Clause> clauses)
        {
            foreach (Clause c in clauses)
            {
                this.isClauseTrue(c);
            }
            return KnowledgeBaseValue(clauses);
        }

    }
}
