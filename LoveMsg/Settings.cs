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
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var ts = sr.ReadLine().Split('=');
                        if (ts.Length == 2) dict.Add(ts[0], ts[1]);
                    }
                }
            }
            catch (Exception e) { }
        }
        public void Save()
        {
            try
            {
                using (var sw = new StreamWriter(filename))
                {
                    foreach (var pair in dict)
                    {
                        sw.WriteLine(pair.Key + "=" + pair.Value);
                    }
                }
            }
            catch (Exception e) { }
        }
        public string Get(string key,string def)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return def;
        }
        public int GetInt(string key, int def)
        {
            var str = Get(key, "NaN");
            int ret;
            if (str == "NaN" || !int.TryParse(str, out ret)) return def;
            return ret;
        }
        public float GetFloat(string key, float def)
        {
            var str = Get(key, "NaN");
            float ret;
            if (str == "NaN" || !float.TryParse(str, out ret)) return def;
            return ret;
        }
        public double GetDouble(string key, double def)
        {
            var str = Get(key, "NaN");
            double ret;
            if (str == "NaN" || !double.TryParse(str, out ret)) return def;
            return ret;
        }
        public void Set(string key,string value,bool save=true)
        {
            dict[key] = value;
            if (save) Save();
        }
        public void Set(string key, object value, bool save = true)
        {
            Set(key, value.ToString(), save);
        }
    }
}
