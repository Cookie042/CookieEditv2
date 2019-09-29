using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FastColoredTextBoxNS;
using Newtonsoft.Json;
using Env = System.Environment;

namespace CookieEdit2.classes
{
    public class SyleManager
    {
        private static List<WordStyle> WordStyles = new List<WordStyle>();

        private string path = Env.CurrentDirectory + "//Settings//ColorStyles.json";
        private static SyleManager _instance;

        public void SetStyles(Range range)
        {
            range.ClearStyle(GetStylesArray());

            foreach (var wordStyle in WordStyles)
            {
                wordStyle.SetStyle(range);
            }
        }

        public static SyleManager GetInstance()
        {
            return _instance ?? (_instance = new SyleManager());
        }

        private SyleManager()
        {

            if (WordStyles == null)
                return;

            //_textStyles.Clear();
            //_textStyles.Add("DarkRedStyle", new TextStyle(Brushes.DarkRed, null, FontStyle.Bold));

            if (!Directory.Exists(Env.CurrentDirectory + "//Settings"))
            {
                Directory.CreateDirectory(Env.CurrentDirectory + "//Settings//");
            }
            
            if (!File.Exists(path))
            {
                //-------------------DEFAULT SETTINGS-------------------
                WordStyles.Add(new WordStyle("Keywords", true, WordStyle.WordValuetype.None, @"(IF|SQRT|ABS|FIX|GT|LT|LE|GE|EQ|GOTO|THEN)", true, true, false, Color.Aqua, null));
                WordStyles.Add(new WordStyle("G Word", true, WordStyle.WordValuetype.Float, @"G", true, true, false, Color.FromArgb(255, 237, 139, 37), null));
                WordStyles.Add(new WordStyle("X Word", true, WordStyle.WordValuetype.Float, @"X", true, true, false, Color.FromArgb(255, 195, 0, 0), null));
                WordStyles.Add(new WordStyle("Y Word", true, WordStyle.WordValuetype.Float, @"Y", true, true, false, Color.FromArgb(255, 5, 176, 69), null));
                WordStyles.Add(new WordStyle("Z Word", true, WordStyle.WordValuetype.Float, @"Z", true, true, false, Color.FromArgb(255, 8, 148, 215), null));
                WordStyles.Add(new WordStyle("I Word", true, WordStyle.WordValuetype.Float, @"(I|U|A)", true, true, false, Color.FromArgb(255, 224, 113, 113), null));
                WordStyles.Add(new WordStyle("J Word", true, WordStyle.WordValuetype.Float, @"(J|V|B)", true, true, false, Color.FromArgb(255, 83, 227, 136), null));
                WordStyles.Add(new WordStyle("K Word", true, WordStyle.WordValuetype.Float, @"(K|W|C)", true, true, false, Color.FromArgb(255, 127, 209, 248), null));
                WordStyles.Add(new WordStyle("Other Word", true, WordStyle.WordValuetype.Float, @"(F|E)", true, true, false, Color.FromArgb(255, 232, 220, 6), null));
                WordStyles.Add(new WordStyle("Tool Word", true, WordStyle.WordValuetype.Int, @"(S|T|H)", true, true, false, Color.FromArgb(255, 133, 69, 212), null));
                WordStyles.Add(new WordStyle("M Word", true, WordStyle.WordValuetype.Int, @"(M|#)", true, true, false, Color.FromArgb(255, 186, 87, 142), null));
                WordStyles.Add(new WordStyle("N Word", true, WordStyle.WordValuetype.Int, @"(N|D)", true, true, false, Color.FromArgb(255, 255, 255, 231), null));
                WordStyles.Add(new WordStyle("Misc1 Word", true, WordStyle.WordValuetype.Int, @"(L|O)", true, true, false, Color.FromArgb(255, 214, 47, 151), null));
                WordStyles.Add(new WordStyle("Misc2 Word", true, WordStyle.WordValuetype.Float, @"(R|Q)", true, true, false, Color.FromArgb(255, 193, 129, 32), null));
                WordStyles.Add(new WordStyle("Misc3 Word", true, WordStyle.WordValuetype.Int, @"P", true, true, false, Color.FromArgb(255, 71, 214, 208), null));

                WordStyles.Add(new WordStyle("Comment Block", @"\(+.*\)+", Color.MediumSpringGreen, null));

                WriteStyleJson();
            }
            else
            {
                ReadStyleJson();
            }


        }

        ~SyleManager()
        {
            //since this can erase xml data on a crash, and it's not needed cause there is no editor (yet)
            //DataContractSerializeXml();
        }

        private void WriteStyleJson()
        {
            string jsonString = JsonConvert.SerializeObject(WordStyles, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(path, jsonString);
        }
        private void ReadStyleJson()
        {
            string jsonString = File.ReadAllText(path);

            WordStyles = JsonConvert.DeserializeObject<List<WordStyle>>(jsonString);
        }
        
        private Style[] GetStylesArray()
        {
            Style[] array = new Style[WordStyles.Count];
            for (int i = 0; i < WordStyles.Count; i++)
            {
                array[i] = WordStyles[i].GetStyle();
            }
            return array;
        }
    }
}
