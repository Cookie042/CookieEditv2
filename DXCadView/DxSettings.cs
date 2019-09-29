using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CookieEdit2.DXCadView
{
    public class DxSettings
    {
        private string SavePath => Path.Combine(Environment.CurrentDirectory, "DXSettings.json");

        private Dictionary<string, object> SettingsDictionary = new Dictionary<string, object>();

        public T GetSetting<T>(string key) => SettingsDictionary[key] is T ? (T) SettingsDictionary[key] : default;
        public void SetSetting<T>(string key, T obj)
        {
            if (!SettingsDictionary.ContainsKey(key))
                SettingsDictionary[key] = obj;
            else
                SettingsDictionary.Add(key, obj);
        }

        public void SaveSettings()
        {
            var path = SavePath;

            var dataString = JsonConvert.SerializeObject(SettingsDictionary, Formatting.Indented);

            File.WriteAllText(path, dataString);
        }
        public void LoadSettings()
        {
            var path = SavePath;

            if (!File.Exists(path)) return;

            var dataString = File.ReadAllText(path);
            SettingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataString);
        }
    }
}