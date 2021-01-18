using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// Contains all information related to the knowledgebase
    /// Includes connectives, symbols and clause classes and
    /// others.
    /// </summary>
    public enum Connective //Links two symbols together
    {
        Implication,   // =>
        Biconditional, //<=>
        Conjunction,   // &
        Disjunction,   // ||
        Negation,      // ~
        None           // -
    }

    public class Symbol //symbols for logical representation
    {
        public string symbol;
        public bool isTrue = false; 
        public bool partOfConclusion = false;
        public Connective leftConnective;
        public Connective rightConnective;
        public bool value;

        public Symbol(string symbol)
        {
            //default connective state
            leftConnective = Connective.None;
            rightConnective = Connective.None;

            //for negatation
            if (symbol.Contains("~"))
            {
                this.symbol = symbol.ToLower().Replace("~", "");
                this.leftConnective = Connective.Negation;
            }
            else
            {
                this.symbol = symbol.ToLower();
            }
        }
    }

    public class Clause //for logic clauses
    {
        public List<Symbol> symbols = new List<Symbol>();
        public bool value;
        public Connective rightConnective;

        public Clause(List<Symbol> symbols)
        {
            //default connective
            rightConnective = Connective.None;

            this.symbols = symbols;
        }
        public Clause(Symbol symbol)
        {
            //default connective
            rightConnective = Connective.None;

            this.symbols.Add(symbol);
        }

        //premise
        public Dictionary<string, Symbol> Premise()
        {
            Dictionary<string, Symbol> premise = new Dictionary<string, Symbol>();
            foreach(Symbol s in symbols)
            {
                if(!s.partOfConclusion && !s.isTrue)
                {
                    premise.Add(s.symbol, s);
                }
            }
            return premise;
        }

        //premise in list for BackChaining
        public List<Symbol> PremiseList()
        {
            List<Symbol> premise = new List<Symbol>();
            foreach (Symbol s in symbols)
            {
                if (!s.partOfConclusion && !s.isTrue)
                {
                    premise.Add(s);
                }
            }
            return premise;
        }

        //Conclusion
        public Dictionary<string, Symbol> Conclusion()
        {
            Dictionary<string, Symbol> conclusion = new Dictionary<string, Symbol>();
            foreach(Symbol s in symbols)
            {
                if(s.partOfConclusion)
                {
                    conclusion.Add(s.symbol, s);
                }
            }
            return conclusion;
        }

        //Facts 
        public Symbol Facts()
        {
            Symbol result = null;
            foreach(Symbol s in symbols)
            {
                if(s.isTrue)
                {
                    result = s;
                }
            }
            return result;
        }

    }

    public class KnowledgeBase
    {
        //key info
        public List<Clause> clauses;
        public List<Symbol> symbols = new List<Symbol>();
        public List<Symbol> facts = new List<Symbol>();

        public KnowledgeBase(List<Clause> percepts)
        {
            this.clauses = percepts;
        }

        //join clauses in KB for TT
        public List<Clause> JoinClauses()
        {
            List<Clause> joinedClauses = new List<Clause>();
            for(int i = 0; i < clauses.Count; i++)
            {
                Clause newClause = clauses[i];
                if(i < clauses.Count - 1)
                {
                    newClause.rightConnective = Connective.Conjunction;
                }
                joinedClauses.Add(newClause);
            }
            return joinedClauses;
        }


        public List<Symbol> Facts()
        {
            foreach(Clause c in clauses)
            {
                if(c.Facts() != null)
                {
                    facts.Add(c.Facts());
                }
            }
            return facts;
        }

        public List<Symbol> Symbols()
        {
            foreach(Clause c in clauses)
            {
                symbols.AddRange(c.symbols);
            }
            return symbols;
        }
    }
}
