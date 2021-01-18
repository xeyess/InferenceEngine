using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// Contains methods used to extract symbols
    /// </summary>
    public class ExtractSymbols
    {
        public string clause;
        private bool isBinary = false; //differ from unary

        public List<Symbol> symbols = new List<Symbol>();

        //begin parse
        public ExtractSymbols(string clause)
        {
            this.clause = clause;
            if(this.clause.Contains("=>"))
            {
                this.isBinary = true;
            }
            else
            {
                this.isBinary = false;
            }
            Parse();
        }

        public void Parse()
        {
            if(isBinary)
            {
                //split implication
                Regex rx = new Regex("=>");
                string[] temp = rx.Split(clause);

                //body parse
                string body = temp[0];
                string[] tokens = body.Split('&', '|');
                
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i] != "")
                    {
                        Symbol s = new Symbol(tokens[i]);
                        if (i < ConnectiveSplit(body).Count)
                        {
                            s.rightConnective = ConnectiveSplit(body)[i];
                        }
                        else if (i == tokens.Length - 1)
                        {
                            s.rightConnective = Connective.Implication;
                        }
                        symbols.Add(s);
                    }
                }

                //head parse
                string head = temp[1];
                tokens = head.Split('&', '|');
                
                for (int j = 0; j < tokens.Length; j++)
                {
                    if (tokens[j] != "")
                    {
                        Symbol s = new Symbol(tokens[j]);
                        s.partOfConclusion = true;
                        if (j < ConnectiveSplit(head).Count)
                        {
                            s.rightConnective = ConnectiveSplit(head)[j];
                        }
                        symbols.Add(s);
                    }
                }

            }
            else
            {
                //for non binary
                string head = clause;
                Symbol temp = new Symbol(head);
                temp.isTrue = true;
                symbols.Add(temp);
            }
        }

        //split connective and assign in obejct
        public List<Connective> ConnectiveSplit(string s)
        {
            List<Connective> result = new List<Connective>();
            string rawConnectives = Regex.Replace(s, "[a-zA-Z0-9]", "");
            string[] connectives = rawConnectives.Split(' ');
            foreach(string connective in connectives)
            {
                Connective temp;
                if (connective == "&")
                {
                    temp = Connective.Conjunction;
                }
                else if (connective == "|")
                {
                    temp = Connective.Disjunction;
                }
                else
                {
                    temp = Connective.None;
                }
                result.Add(temp);
            }
            return result;
        }
    }
}
