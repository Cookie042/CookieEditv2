using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System;
using System.IO;
using CookieEdit2.classes;

namespace CookieEdit2.Windows
{
    /// <summary>
    /// Interaction logic for SerialIO.xaml
    /// </summary>
    public partial class SerialIO : Window
    {
        public List<SerialIoSettings> settings = new List<SerialIoSettings>();

        public string SettingsJsonPath => Environment.CurrentDirectory + "//Settings//IOSettings.json";

        public SerialIO()
        {
            InitializeComponent();
           
            if (File.Exists(SettingsJsonPath))
            {
                LoadSettings_Json();
                UpdateSettingsListBox();

            } else
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "//Settings"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "//Settings//");

                settings.Add(new SerialIoSettings("Mori NL", "COM1", 9600, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Handshake.XOnXOff));
                settings.Add(new SerialIoSettings("OKK", "COM1", 2400, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Handshake.XOnXOff));
                settings.Add(new SerialIoSettings("Toshiba", "COM1", 19200, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Handshake.XOnXOff));

                SaveSettings_Json();
            }
        }

        void UpdateSettingsListBox()
        {
            machineList.Items.Clear();
            foreach (var setting in settings)
                machineList.Items.Add(setting);
                    //machineList.Items.Add(setting.name);
        }

        void SaveSettings_Json()
        {
            var s = JsonConvert.SerializeObject(settings, Formatting.Indented);

            File.WriteAllText(Environment.CurrentDirectory + "//Settings//IOSettings.json", s);
        }

        bool LoadSettings_Json()
        {
            if (File.Exists(SettingsJsonPath))
            {

                var s = File.ReadAllText(SettingsJsonPath);

                settings = JsonConvert.DeserializeObject<List<SerialIoSettings>>(s);
                return true;
            }
            return false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SerialIOEditWindow editWindow = new SerialIOEditWindow();
            editWindow.Top = Top;
            editWindow.Left = Left;
            editWindow.settingsObject = new SerialIoSettings();
            if (editWindow.ShowDialog() == true)
            {
                settings.Add(editWindow.settingsObject);
                UpdateSettingsListBox();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SerialIoSettings s = machineList.SelectedItem as SerialIoSettings;

            if (s == null)
                return;

            SerialIOEditWindow editWindow = new SerialIOEditWindow();
            editWindow.Top = Top;
            editWindow.Left = Left;
            editWindow.settingsObject = s;

            if (editWindow.ShowDialog() == true)
            {
                machineList.Items[machineList.SelectedIndex] = editWindow.settingsObject;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings_Json();
        }
    }
}
