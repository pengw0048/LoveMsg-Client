using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LoveMsg
{
    class Settings
    {
        private Dictionary<string, string> dict;
        private string filename;
        public Settings(string filename)
        {
            Load(filename);
        }
        public void Load(string filename)
        {
            this.filename = filename;
            dict = new Dictionary<string, string>();
            using (var sr = new StreamReader(filename))
            {
                try
                {
                    while (!sr.EndOfStream)
                    {
                        var ts = sr.ReadLine().Split('=');
                        if (ts.Length == 2) dict.Add(ts[0], ts[1]);
                    }
                }
                catch (Exception) { }
            }
        }
        public void Save()
        {
            using (var sw = new StreamWriter(filename))
            {
                try
                {
                    foreach (var pair in dict)
                    {
                        sw.WriteLine(pair.Key + "=" + pair.Value);
                    }
                }
                catch (Exception) { }
            }
        }
        public string Get(string key,string def)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return def;
        }
        public void Set(string key,string value,bool save=true)
        {
            dict[key] = value;
            if (save) Save();
        }
    }
}
