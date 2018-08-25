﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using FastColoredTextBoxNS;

namespace CookieEdit2
{
    public class SyleManager
    {
        private static List<WordStyle> WordStyles = new List<WordStyle>();

        private string path = Environment.CurrentDirectory + "//Styles//ColorStyles.xml";
        private static SyleManager __instance;

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
            return __instance ?? (__instance = new SyleManager());
        }

        private SyleManager()
        {

            if (WordStyles == null)
                return;

            //_textStyles.Clear();
            //_textStyles.Add("DarkRedStyle", new TextStyle(Brushes.DarkRed, null, FontStyle.Bold));

            if (!Directory.Exists(Environment.CurrentDirectory + "//Styles"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "//Styles//");
            }
            
            if (!File.Exists(path))
            {
                //-------------------DEFAULT SETTINGS-------------------
                WordStyles.Add(new WordStyle("G Word", true, true, @"G", true, true, false, Color.FromArgb(255, 237, 139, 37)));
                WordStyles.Add(new WordStyle("X Word", true, true, @"X", true, true, false, Color.FromArgb(255, 195, 0, 0)));
                WordStyles.Add(new WordStyle("Y Word", true, true, @"Y", true, true, false, Color.FromArgb(255, 5, 176, 69)));
                WordStyles.Add(new WordStyle("Z Word", true, true, @"Z", true, true, false, Color.FromArgb(255, 8, 148, 215)));
                WordStyles.Add(new WordStyle("I Word", true, true, @"(I|U|A)", true, true, false, Color.FromArgb(255, 224, 113, 113)));
                WordStyles.Add(new WordStyle("J Word", true, true, @"(J|V|B)", true, true, false, Color.FromArgb(255, 83, 227, 136)));
                WordStyles.Add(new WordStyle("K Word", true, true, @"(K|W|C)", true, true, false, Color.FromArgb(255, 127, 209, 248)));
                WordStyles.Add(new WordStyle("Other Word", true, true, @"(F|E)", true, true, false, Color.FromArgb(255, 232, 220, 6)));
                WordStyles.Add(new WordStyle("Tool Word", true, true, @"(S|T|H)", true, true, false, Color.FromArgb(255, 133, 69, 212)));
                WordStyles.Add(new WordStyle("M Word", true, true, @"M", true, true, false, Color.FromArgb(255, 186, 87, 142)));
                WordStyles.Add(new WordStyle("N Word", true, true, @"(N|D)", true, true, false, Color.FromArgb(255, 231, 231, 231)));
                WordStyles.Add(new WordStyle("Misc1 Word", true, true, @"(L|O)", true, true, false, Color.FromArgb(255, 214, 47, 151)));
                WordStyles.Add(new WordStyle("Misc2 Word", true, true, @"(R|Q)", true, true, false, Color.FromArgb(255, 193, 129, 32)));
                WordStyles.Add(new WordStyle("Misc3 Word", true, true, @"P", true, true, false, Color.FromArgb(255, 71, 214, 208)));

                WordStyles.Add(new WordStyle("Comment Block", @"\(+.*\)+", Color.MediumSpringGreen));
                DataContractSerializeXml();
            }
            else
            {
                ReadDataSerializedXml();
            }


        }

        ~SyleManager()
        {
            //since this can erase xml data on a crash, and it's not needed cause there is no editor (yet)
            //DataContractSerializeXml();
        }

        private void DataContractSerializeXml()
        {
            var ser = new DataContractSerializer(typeof(WordStyle[]));
            var obj = WordStyles.ToArray();

            var xw = XmlWriter.Create(File.Create(path));

            ser.WriteObject(xw, obj);

            xw.Close();

        }

        public void ReadDataSerializedXml()
        {
            var ser = new DataContractSerializer(typeof(WordStyle[]));

            try
            {
                var xr = XmlReader.Create(File.Open(path, FileMode.Open));

                WordStyles.Clear();
                WordStyle[] wordStlyes = (WordStyle[])ser.ReadObject(xr);

                WordStyles.AddRange(wordStlyes);

                xr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
