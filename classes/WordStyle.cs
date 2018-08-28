using System.Drawing;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace CookieEdit2
{
    [DataContract]
    public class WordStyle
    {

        public enum WordValuetype
        {
            Float, Int, None
        }

        //regex for float
        private const string F = @"\s*-?\d*\.?\d*";
        //regex for float
        private const string I = @"\s*(\d*|\[)+";

        private const string NoComm = @"(?<!\(.*)";

        [DataMember(Name = "Style_Name")]
        private string _name = "a name";

        [DataMember(Name = "Red")]
        private int _red;
        [DataMember(Name = "Green")]
        private int _green;
        [DataMember(Name = "Blue")]
        private int _blue;

        private Style _style;

        public Style Style {
            get { return GetStyle(); }
        }


        public Color TextColor
        {
            get { return Color.FromArgb(_red, _green, _blue); }
            set
            {
                _red = value.R;
                _green = value.G;
                _blue = value.B;
            }
        }

        //private Color _backColor = new Color();
        


        [DataMember(Name = "Is_Bold")]
        private bool _isBold;
        [DataMember(Name = "Is_Italic")]
        private bool _isItalic;
        [DataMember(Name = "Do_Ignore_Case")]
        private bool _isIgnoreCase = true;

        //private TextStyle _textStyle;
        [DataMember(Name = "Regex")]
        private string _regex = "";

        public WordStyle() { }

        public WordStyle(string name, bool notInComment, WordValuetype decimalValue, string letters, bool ignoreCase, bool isBold, bool isItalic, Color color)
        {
            _red = color.R;
            _green = color.G;
            _blue = color.B;
            _isBold = isBold;
            _isItalic = isItalic;
            _isIgnoreCase = ignoreCase;
            TextColor = color;

            _name = name;

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

            _regex = (notInComment ? NoComm : "" ) + letters + @"((?=\s?\[)|" + v + @")";

            _style = GetStyle();
        }

        public WordStyle(string name, string regex, Color foreColor) : this(name, false, WordValuetype.None, "", true, false, true, foreColor)
        {
            _regex = regex;
        }

        public void SetStyle(Range range)
        {
            range.SetStyle(_style, _regex, (_isIgnoreCase?RegexOptions.IgnoreCase : RegexOptions.None));

            _style = GetStyle();
        }


        public Style GetStyle()
        {
            if (_style == null)
            {
                return new TextStyle(new SolidBrush(TextColor), null,
                    (_isBold ? FontStyle.Bold : 0) & (_isItalic ? FontStyle.Italic : 0));
            }
            else
            {
                return _style;
            }
        }
    }
}