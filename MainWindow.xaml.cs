using System;
using System.IO;
using System.Windows;
using SlimDX;

using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using Flaxen.SlimDXControlLib;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Diagnostics;

namespace CookieEdit2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public SlimDXControl SDXControl;

        public static MainWindow instance = null;

        private static string windowTitle = "CookieEdit v2.0";

        private string openFilePath;

        private SyleManager _syleManager = SyleManager.GetInstance();

        private bool Show3dView = true;
        private GridLength GraphicsGridWidth = new GridLength();

        public MacroVariableManager macroVariableManager = new MacroVariableManager();

        //CTOR
        public MainWindow()
        {
            InitializeComponent();

            instance = this;

            SDXControl = new SlimDXControl();
            SDXControl.KeyDown += x_contentControl1_KeyDown;

            Flaxen.SlimDXControlLib.RenderEngine re1 = new RenderEngine(true, SDXControl);
            SDXControl.RegisterRenderer(re1);

            dp.Children.Add(SDXControl);
        }

        //LOAD EVENT
        private void MainWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            Width = 1300;
            Height = 800;

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
            FctbClearChangedMarkers();
            fctb.ClearUndo();

            FctbColorVisibleRangeWithStyles();

            // initialize sdx control object
        }

        private void InitializeFastColoredTextbox()
        {
            //fctb.AddStyle(style);

            fctb.Zoom = 120;

            FctbColorVisibleRangeWithStyles();
        }

        //window Methods
        private void FctbColorVisibleRangeWithStyles()
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

        public void FctbClearChangedMarkers()
        {
            for (int i = 0; i < fctb.LinesCount; i++)
            {
                fctb[i].IsChanged = false;
                fctb.Invalidate();
            }
        }

        private void Fctb_VisibleChanged(object sender, EventArgs e) => FctbColorVisibleRangeWithStyles();
        private void Fctb_Resize(object sender, EventArgs e) => FctbColorVisibleRangeWithStyles();
        private void Fctb_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e) { } //fctbColorVisibleRangeWithStyles();

        private bool OpenFileDlg()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fctb.Text = File.ReadAllText(openFileDialog.FileName);
                openFilePath = openFileDialog.FileName;
                FctbColorVisibleRangeWithStyles();
                FctbClearChangedMarkers();
                UpdateTitle();
                return true;
            }

            return false;
        }

        private void UpdateTitle()
        {
            if (openFilePath != null && openFilePath != "")
            {
                Title = windowTitle + " - " + openFilePath;

            }
            else
            {
                Title = windowTitle;
            }
        }

        private void OpenMacroGUIMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MacroWindow.Spawn(this);
        }

        private void CalcToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            CalcWindow calc = new CalcWindow();
            calc.Owner = this;
            calc.Show();
        }


        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("SaveAs!");
            //SaveAs();//Implementation of saveAs
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("SaveAs!");
            //SaveAs();//Implementation of saveAs
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("New File!");
            fctb.Clear();
            openFilePath = "";
            UpdateTitle();
        }


        //-------------FCTB EVENTS--------------

        private void fctb_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            FctbColorVisibleRangeWithStyles();
        }

        private void fctb_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //ShowContextMenu
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
                cm.PlacementTarget = sender as WindowsFormsHost;
                cm.IsOpen = true;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
            }
        }

        //--------CAN EXEC---------
        private void CanExecute_True(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanExecute_IsTextSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = fctb.SelectedText.Length > 0;
        }

        //---------CUT COPY PASTE COMMANDS----------
        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e) => fctb.SelectedText = Clipboard.GetText();
        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e) => Clipboard.SetText(fctb.SelectedText);
        private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(fctb.SelectedText);
            fctb.SelectedText = "";
        }

        private void expToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.SerialIO gui = new Windows.SerialIO();
            gui.Owner = this;
            gui.Show();
        }

        private void x_contentControl1_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key.ToString());
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDlg();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDlg();
        }

        private void Toggle3DToolbarButton_Click(object sender, RoutedEventArgs e)
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


        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.Redo();
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.Undo();
        }

        private void FindReplaceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void FormatCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void FindCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fctb.findForm.Show();
        }
    }
}
