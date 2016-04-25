using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace WFCG2Tool
{
    public class LogViewer
    {
        public TextBox LogTextBox { get; set; }
        public List<String> Logs { get; set; }

        public readonly static int MaximumLogCount = 20;

        public LogViewer(TextBox viewer)
        {
            LogTextBox = viewer;
            LogTextBox.Multiline = true;
            Logs = new List<string>();
        }

        public void Add(String log) {
            if (this.Logs.Count() >= MaximumLogCount) {
                Logs.RemoveAt(0);
            }
            Logs.Add(log);

            UpdateLog();
        }

        public void UpdateLog() {
            StringBuilder sb = new StringBuilder();
            List<string> msg = new List<string>(Logs);
            msg.Reverse();

            foreach (String s in msg) {
                sb.AppendLine(s);                
            }
            
            // For thread-safe update
            LogTextBox.Invoke((MethodInvoker)delegate{
                LogTextBox.Text = sb.ToString();
            });
        }
    }
}
