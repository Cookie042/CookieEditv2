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

        public string getArgLetter(int id)
        {
            switch (id)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "I";
                case 5:
                    return "J";
                case 6:
                    return "K";
                case 7:
                    return "D";
                case 8:
                    return "E";
                case 9:
                    return "F";
                case 11:
                    return "H";
                case 13:
                    return "M";
                case 17:
                    return "Q";
                case 18:
                    return "R";
                case 19:
                    return "S";
                case 20:
                    return "T";
                case 21:
                    return "U";
                case 22:
                    return "V";
                case 23:
                    return "W";
                case 24:
                    return "X";
                case 25:
                    return "Y";
                case 26:
                    return "Z";
                default:
                    return "";
            }

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
