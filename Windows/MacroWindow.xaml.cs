using System;
using System.Windows;

namespace CookieEdit2.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MacroWindow : Window
    {

        private static MacroWindow _instance;

        public static MacroWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MacroWindow();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public MacroWindow()
        {
            InitializeComponent();
            macroListBox.ItemsSource = MainWindow.instance.macroVariableManager.variables;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight - 200;

            SizeToContent = SizeToContent.Width;
        }

        public static void Spawn(Window parent)
        {
            Instance.Owner = parent;
            Instance.WindowState = WindowState.Normal;
            Instance.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instance = null;
        }
    }


}
