using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFCG2Tool
{
    public partial class mainForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
 (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LogViewer Log { get; set; }
        
        private Scripter script;
        private Timer timer;

        private bool IsRunning;
        private DateTime startTime;
        public static readonly int maxSeconds = 1800;
        
        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log = new LogViewer(txtBox);
            Log.Add("Form Loaded....");

            script = new Scripter(Log);
        }

        private void PrintWindowNameByProcessId()
        {
            IntPtr hWnd = WindowHelper.GetProcessWindow(7564);
            if (hWnd != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1024);
                WindowHelper.GetWindowText(new System.Runtime.InteropServices.HandleRef(this, hWnd), sb, sb.Capacity);
                Log.Add("hWwnd name: " + sb.ToString());
            }
            else
            {
                Log.Add("Failed to get window by processId");
            }
        }

        public int tickCount = 0;
        private Random rand = new Random();
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (timer == null) {
                timer = new Timer();
                timer.Interval = 1200 + rand.Next() % 50;
                timer.Tick += Timer_Tick;

                Log.Add("Timer is setup...");
            }

            this.IsRunning = true;
            script.Init();
            timer.Start();
            tickCount = 0;
            startTime = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsRunning) {
                Log.Add("Paused. Skip this tick...");
                return;
            }
            // Check time
            DateTime now = DateTime.Now;
            TimeSpan diff = now - startTime;
            if (diff.TotalSeconds > maxSeconds)
            {
                // 先停止
                Log.Add("Exceeds the maxSeconds, force stop.");
                timer.Stop();
                return;
            }

            string msg = string.Format("\r\nTimer tick ... ({0})\r\nTotal seconds: {1}", tickCount, diff.TotalSeconds );
            Log.Add(msg);

            tickCount += 1;

            timer.Stop();
            script.RunIt();
            timer.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (timer == null)
            {
                return;
            }

            this.IsRunning = false;
            Log.Add("Pause..... \r\n");

            script.Stop();
            timer.Stop();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.IsRunning = false;
            if (timer != null) {
                timer.Stop();
            }
            
            DialogResult result = MessageBox.Show("將要離開這個程式", "警告", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK) {
                Application.Exit();
            }
        }
    }
}
