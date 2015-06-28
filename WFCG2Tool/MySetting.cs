using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using WFCG2Tool.Properties;
using WFCG2Tool.Strategies;

using System.Reflection;

namespace WFCG2Tool
{
    public class MySetting
    {
        public static readonly MySetting Instance = new MySetting();

        public int LoopMax { get; set; }
        public int MaxSeconds { get; set; }
        public DIR_STATEGY Strategy { get; set; }
        public bool QuitTeam { get; set; }

        private MySetting()
        {
            LoopMax = 10;
            MaxSeconds = 3600;
            Strategy = DIR_STATEGY.UP_DOWN;
            QuitTeam = false;
        }

        public void Load() {
            String json = Settings.Default.JsonValue;

            MySetting loadObj = JsonConvert.DeserializeObject<MySetting>(json);
            if (loadObj == null)
                return;

            Type t = this.GetType();
            PropertyInfo[] fields = t.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
            foreach (PropertyInfo p in fields) {
                var val = p.GetValue(loadObj);
                p.SetValue(this, val);
            }
        }

        public void Save() {
            String Json = JsonConvert.SerializeObject(this);
            Settings.Default.JsonValue = Json;
            Settings.Default.Save();
        }
    }
}
