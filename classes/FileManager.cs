using System;
using Microsoft.Win32;
using FastColoredTextBoxNS;
using System.IO;
using System.Windows;

namespace CookieEdit2
{
    public class FileManager
    {
        public string OpenFilePath { get; set; }

        public event Action<string> FileSavedEvent;
        public event Action<string> FileOpenedEvent;

        public bool Save(MainWindow mainWindow, bool showDialog )
        {

            var savePath = OpenFilePath;

            if (string.IsNullOrEmpty(OpenFilePath) || showDialog)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "All|*.*|CNC file|.NC";
                if (saveFileDialog.ShowDialog() == true)
                {
                    savePath = saveFileDialog.FileName;
                }
                else
                {
                    return false;
                }
            }
            try
            {
                File.WriteAllText(savePath, mainWindow.fctb.Text);
                OpenFilePath = savePath;
                
                FileSavedEvent?.Invoke(OpenFilePath);

                return true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return false;
        }
        
        public bool Open(MainWindow mainWindow)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                mainWindow.fctb.Text = File.ReadAllText(openFileDialog.FileName);
                OpenFilePath = openFileDialog.FileName;

                FileOpenedEvent?.Invoke(OpenFilePath);
                return true;
            }
            
            return false;
        }
    }
}
