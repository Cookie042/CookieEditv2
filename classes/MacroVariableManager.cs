using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CookieEdit2
{
    public class MacroVariableManager
    {
        public Dictionary<int, MacroVariable> variables = new Dictionary<int, MacroVariable>();

        public MacroVariableManager()
        {
            for (int i = 0; i < 500; i++)
            {
                variables.Add(i, new MacroVariable(i, getArgLetter(i), 0));
            }
        }
        enum ArgLetter
        {
            Null = 0, A = 1, B = 2, C = 3, I = 4, J = 5, K = 6,
            D = 7, E = 8, F = 9, H = 11, M = 13, Q = 17,
            R = 18, S = 19, T = 20, U = 21, V = 22, W = 23,
            X = 24, Y = 25, Z = 26
        }

        public string getArgLetter(int id)
        {
            var enumStr = ((ArgLetter)id).ToString();
            if (int.TryParse(enumStr,out int result))
                return "";
            return enumStr;
        }
    }
    
    
    public class MacroVariable
    {
        
        public int id { get; set; }
        public string name { get; set; }
        public float value { get; set; }

        public MacroVariable(int id, string name, float val)
        {
            this.id = id;
            this.name = name;
            this.value = val;
        }

        public MacroVariable() { }


    }
}
