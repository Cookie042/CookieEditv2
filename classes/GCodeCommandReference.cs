using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace CookieEdit2
{
    public class GCodeCommandReference
    {
        private string filepath = "//Settings//Commands.json";

        public Dictionary<string, CodeDetails> codeDict = new Dictionary<string, CodeDetails>();

        public GCodeCommandReference()
        {
            if (File.Exists(System.Environment.CurrentDirectory + filepath))
            {
                LoadJson();
            } else {
                BuildDict();
                SaveJson();
            }
        }

        public void BuildDict()
        {
            codeDict.Add("G0", CodeDetails.New("Rapid Move (modal)", "X,Y,Z"));
            codeDict.Add("G1", CodeDetails.New("Feed Move (modal)", "X,Y,Z,F"));
            codeDict.Add("G2", CodeDetails.New("CW Radial Move (modal)", "X,Y,Z,I,J,K,F,R"));
            codeDict.Add("G3", CodeDetails.New("CCW Radial Move (modal)", "X,Y,Z,I,J,K,F,R"));
        }

        public void SaveJson()
        {

            var jsonText = JsonConvert.SerializeObject(codeDict, Formatting.Indented);

            File.WriteAllText(System.Environment.CurrentDirectory + filepath , jsonText);

        }

        public void LoadJson()
        {
            var jsonText = File.ReadAllText(System.Environment.CurrentDirectory + filepath);

            codeDict = JsonConvert.DeserializeObject<Dictionary<string, CodeDetails>>(jsonText);
        }

    }



    public class CodeDetails
    {
        public string information;
        public List<string> parameters = new List<string>();

        public static CodeDetails New(string information, string csv)
        {
            var parameters = csv.Trim().Split(',').ToList();
            return new CodeDetails(information, parameters);
        }

        public CodeDetails(string information, List<string> list)
        {
            parameters = list;

            this.information = information;
        }
    }
}
