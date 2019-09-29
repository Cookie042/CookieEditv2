using System;
using System.Drawing;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Newtonsoft.Json;

namespace CookieEdit2.classes
{

    [JsonObject(MemberSerialization.OptIn)]
    public class WordStyle
    {

        public enum WordValuetype
        {
            Float, Int, None
        }
        
        private const string F = @"\s*-?\d*\.?\d*";
        private const string I = @"\s*(\d*|\[)+";
        private const string NoComm = @"(?<!\(.*)";
        
        [JsonProperty] private string name = "a name";
        [JsonProperty] private Color textColor;
        [JsonProperty] private Nullable<Color> backColor = new Color();
        [JsonProperty] private bool isBold;
        [JsonProperty] private bool isItalic;
        [JsonProperty] private bool isIgnoreCase = true;
        [JsonProperty] private string regex = "";


        private Style style;
        public Style Style => GetStyle();

        //private TextStyle _textStyle;

        public WordStyle() { }

        public WordStyle(string name, bool notInComment, WordValuetype decimalValue, string letters, bool ignoreCase, bool isBold, bool isItalic, Color foreColor, Nullable<Color> backColor)
        {
            textColor = foreColor;
            this.isBold = isBold;
            this.isItalic = isItalic;
            isIgnoreCase = ignoreCase;

            this.name = name;

            string v = "";
            switch (decimalValue)
            {
                case WordValuetype.Float:
                    v = F;
                    break;
                case WordValuetype.Int:
                    v = I;
                    break;
                case WordValuetype.None:
                    v = "";
                    break;
                default:
                    break;
            }

            regex = (notInComment ? NoComm : "" ) + letters + @"((?=\s?\[)|" + v + @")";
            
        }

        public WordStyle(string name, string regex, Color foreColor, Nullable<Color> backColor) : this(name, false, WordValuetype.None, "", true, false, true, foreColor, backColor)
        {
            this.regex = regex;
        }

        public void SetStyle(Range range)
        {
            range.SetStyle(Style, regex, (isIgnoreCase?RegexOptions.IgnoreCase : RegexOptions.None));
        }


        public Style GetStyle()
        {
            if (style == null)
            {
                style = new TextStyle(new SolidBrush(textColor), (backColor == null) ? null : new SolidBrush(backColor.Value),
                    (isBold ? FontStyle.Bold : 0) & (isItalic ? FontStyle.Italic : 0));
            }

            return style;
        }
    }
}