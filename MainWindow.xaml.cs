using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using SlimDX;

using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using Flaxen.SlimDXControlLib;
using System.Windows.Input;
using System.Windows.Forms.Integration;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;
using Clipboard = System.Windows.Clipboard;
using ContextMenu = System.Windows.Controls.ContextMenu;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace CookieEdit2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private variables
        private bool unsavedChanges = false;

        //Public
        public FileManager fileManager = new FileManager();

        public SlimDXControl SDXControl;

        public static MainWindow instance = null;

        private static string windowTitle = "CookieEdit v2.0";

        private SyleManager _syleManager = SyleManager.GetInstance();

        private bool Show3dView = true;
        private GridLength GraphicsGridWidth = new GridLength();

        public bool IsHomePc => Environment.MachineName == "MONSTERCOOKE";

        public MacroVariableManager macroVariableManager = new MacroVariableManager();
        public GCodeCommandReference commandReference = new GCodeCommandReference();

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            
            instance = this;

            SDXControl = new SlimDXControl();
            SDXControl.KeyDown += SdxCtrl_KeyDown;

            Flaxen.SlimDXControlLib.RenderEngine re1 = new RenderEngine(true, SDXControl);
            SDXControl.RegisterRenderer(re1);

            dp.Children.Add(SDXControl);

            // link to events
            fileManager.FileSavedEvent += FileManagerOnFileChangedEvent;
            fileManager.FileOpenedEvent += FileManagerOnFileChangedEvent;

            fctb.KeyPressed += Fctb_KeyPressed;
            fctb.VisibleRangeChanged += Fctb_VisibleRangeChanged;
            fctb.ToolTipNeeded += Fctb_ToolTipNeeded;
            fctb.ToolTipDelay = 1;

        }

        private void Fctb_ToolTipNeeded(object sender, ToolTipNeededEventArgs e)
        {

            var lineText = fctb.GetLine(e.Place.iLine).Text.Insert(e.Place.iChar, "||");
            var match = Regex.Match(lineText, @"(?<1>G|#)(?<2>\d*(?:\|{2})\d*)|(?:\|{2})(?<1>G|#)(?<2>\d*)");

            var word = match.Groups[2].Value.Replace("||", "");

            if (int.TryParse(word, out int result))
            {
                e.ToolTipText = match.Groups[1] + word;
                if (match.Groups[1].Value != "#")
                {
                    if (commandReference.codeDict.ContainsKey(match.Groups[1] + word))
                    {
                        e.ToolTipText = commandReference.codeDict[match.Groups[1] + word].information;
                    }


                } else if (macroVariableManager.variables.ContainsKey(result))
                {
                    var name = macroVariableManager.variables[result].name;
                    if (!string.IsNullOrEmpty(name))
                        name = " (" + name + ")";
                    else
                        name = "";
                    e.ToolTipText = "#" + word + name + " = " + macroVariableManager.variables[result].value;
                }
            }
        }

        private void Fctb_Hover(object sender, EventArgs e)
        {

        }

        internal void FileManagerOnFileChangedEvent(string s)
        {
            FctbClearChangedMarkers();
            SetTitleFilename(s, false);
            unsavedChanges = false;
        }

        private void SetTitleFilename(string filepath, bool astrisk)
        {
            if (!string.IsNullOrEmpty(filepath))
                Title = windowTitle + " - " + filepath + (astrisk ? "*" : "");
            else
            {
                Title = windowTitle;
            }
        }

        //LOAD EVENT
        private void MainWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            // window startPosition & size
            Width = 1000;
            Left = 700;
            Top = IsHomePc ? 200 : -900;
            Height = 600;


            CalcWindow calc = new CalcWindow();
            calc.Top = Top;
            calc.Left = Left;
            calc.Show();

            FctbDefaultText();
            FctbClearChangedMarkers();
            fctb.ClearUndo();

            FctbColorVisibleRangeWithStyles();

            // initialize sdx control object
        }

        private void FctbDefaultText()
        {

            fctb.Text = @"
G54
G0 X0 Y0 Z.25
G1 Z-0 F4
(CCW HELIX)
G3 I2.0 Z-1 F25
G3 I2.25 Z-2
G3 I2.75 Z-3
G1 X-2
(CW HELIX)
G2 I-2.0 Z-2
G2 I-2.25 Z-1
G2 I-2.75 Z-0
G1 Y0
G0 Z0
G28 X0 Y0 Z0
";


            fctb.AddHint(new Range(MainWindow.instance.fctb, 2, 0, 5, 2), "Test");
        }

        internal void InitializeFastColoredTextbox()
        {
            //fctb.AddStyle(style);

            fctb.Zoom = 150;

            FctbColorVisibleRangeWithStyles();
        }

        //window Methods
        internal void FctbColorVisibleRangeWithStyles()
        {
            var range = fctb.VisibleRange;
            // convert range to include full lines
            range = new Range(fctb,
                0,
                range.Start.iLine,
                fctb.Lines[range.End.iLine].Length,
                range.End.iLine);

            _syleManager.SetStyles(range);

            range.SetFoldingMarkers(@"G8(3|4|5|6|7|8|9)|G7(3|4)", @"G80", RegexOptions.IgnoreCase);
        }

        internal void FctbClearChangedMarkers()
        {
            for (int i = 0; i < fctb.LinesCount; i++)
            {
                fctb[i].IsChanged = false;
                fctb.Invalidate();
            }
        }
        
        
        //FCTB Events

        internal void Fctb_Resize(object sender, EventArgs e) => FctbColorVisibleRangeWithStyles();
        internal void Fctb_VisibleRangeChanged(object sender, EventArgs eventArgs) => FctbColorVisibleRangeWithStyles();
        internal void Fctb_KeyPressed(object sender, KeyPressEventArgs keyPressEventArgs)
        {
            if (!unsavedChanges)
            {
                SetTitleFilename(fileManager.OpenFilePath, true);
                unsavedChanges = true;
            }
        }

        //--------CAN EXEC---------
        internal void CanExecute_True(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        internal void CanExecute_IsTextSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = fctb.SelectedText.Length > 0;
        }

        //Mouse Events
        internal void fctb_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //ShowContextMenu
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
                cm.PlacementTarget = sender as WindowsFormsHost;
                cm.IsOpen = true;
            }

            if (e.Button == MouseButtons.Middle)
            {
            }
        }

        //KeyDown Events
        internal void SdxCtrl_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key.ToString());
        }


        //Command Events
        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fileManager.Save(this, true);
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fileManager.Save(this, false);
            //SaveAs();//Implementation of saveAs
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("New File!");
            fctb.Clear();
            fileManager.OpenFilePath = "";
            SetTitleFilename("", false);
        }

        internal void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e) => fctb.SelectedText = Clipboard.GetText();

        internal void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e) => Clipboard.SetText(fctb.SelectedText);

        internal void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(fctb.SelectedText);
            fctb.SelectedText = "";
        }

        internal void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fileManager.Open(this);
        }

        internal void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.Redo();
        }

        internal void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.Undo();
        }

        internal void FindCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.findForm.Show();
        }


        //Button Click Events
        private void OpenMacroGUIMenuItem_Click(object sender, RoutedEventArgs e) => MacroWindow.Spawn(this);

        private void CalcToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            CalcWindow calc = new CalcWindow();
            calc.Owner = this;
            calc.Show();
        }

        internal void Toggle3DToolbarButton_Click(object sender, RoutedEventArgs e)
        {

            if (!Show3dView)
            {
                grid_right.Width = GraphicsGridWidth;

                SDXControl.IsEnabled = true;
                Show3dView = true;
            }
            else
            {
                SDXControl.IsEnabled = false;
                GraphicsGridWidth = grid_right.Width;
                var w = new GridLength(1, GridUnitType.Pixel);
                grid_right.Width = w;
                Show3dView = false;
            }

        }

        internal void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            fileManager.Open(this);
        }

        internal void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            fileManager.Save(this, false);
        }

        internal void ExpToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.SerialIO gui = new Windows.SerialIO();
            gui.Owner = this;
            gui.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Dictionary<string, Dictionary<string, List<string>>> languages = new Dictionary<string, Dictionary<string, List<string>>>();

            languages.Add("english", new Dictionary< string, List<string>>());
            languages.Add("spanish", new Dictionary< string, List<string>>());
            languages.Add("french", new Dictionary< string, List<string>>());

            foreach (var language in languages)
            {
                language.Value.Add("greetings", new List<string>());
                language.Value.Add("responses", new List<string>());
            }

            languages["english"]["greetings"].Add("Hello");
            languages["english"]["greetings"].Add("Hello2");
            languages["english"]["greetings"].Add("Hello3");
            languages["english"]["responses"].Add("WHAT?");
            languages["english"]["responses"].Add("greetings old <gender2>");
            languages["english"]["responses"].Add("Sup?");

            // spanish
            languages["spanish"]["greetings"].Add("Hola");
            languages["spanish"]["greetings"].Add("Hola2");
            languages["spanish"]["greetings"].Add("Hola3");

            // greetings
            languages["french"]["greetings"].Add("Bonjour");
            languages["french"]["greetings"].Add("Bonjour2");
            languages["french"]["greetings"].Add("Bonjour3");

            foreach (KeyValuePair<string, Dictionary<string, List<string>>> language in languages)
            {
                var jsonString = JsonConvert.SerializeObject(language.Value, Formatting.Indented);

                File.WriteAllText(Environment.CurrentDirectory + "//Languages//" + language.Key + ".json", jsonString);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Dictionary<string, Dictionary<string, List<string>>> dialogData = new Dictionary<string, Dictionary<string, List<string>>>();

            var dir = new DirectoryInfo(System.Environment.CurrentDirectory + "//Languages//");

            foreach (var fileInfo in dir.GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                var languageName = fileInfo.Name.Substring(0, fileInfo.Name.Length - 5);

                var jsonText = File.ReadAllText(fileInfo.FullName);


                dialogData.Add(languageName, JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonText));
            }

            var s = "";
            //looping all the language data
            foreach (var lang in dialogData)
            {
                Console.WriteLine(lang.Key);
                s += lang.Key + "\n";
                foreach (var phraseGroup in lang.Value)
                {
                    s += "  " + phraseGroup.Key + "\n";
                    Console.WriteLine( "  " + phraseGroup.Key);
                    if (phraseGroup.Value == null || phraseGroup.Value.Count == 0)
                    {

                        Console.WriteLine("    (none)");
                    }
                    foreach (var phrase in phraseGroup.Value)
                    {
                        Console.WriteLine("    " + phrase);
                    }
                }
            }

            //english
            //  greetings
            //      Hello
            //      Hello2
            //      Hello3
            //  responses
            //      WHAT?
            //      greetings old<gender2>
            //      Sup?
            //french
            //  greetings
            //      Bonjour
            //      Bonjour2
            //      Bonjour3
            //  responses
            //      (none)
            //spanish
            //  greetings
            //      Hola
            //      Hola2
            //      Hola3
            //  responses
            //      (none)
        }
    }
}
