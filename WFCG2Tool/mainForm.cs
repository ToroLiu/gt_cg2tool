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
        public int maxSeconds;
        
        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log = new LogViewer(txtBox);

            MySetting conf = MySetting.Instance;
            conf.Load();

            maxSeconds = conf.MaxSeconds;
            Log.Add("MaxSeconds: " + maxSeconds.ToString());
            Log.Add("程式開始");

            script = new Scripter(Log);
        }

        private void PrintWindowNameByProcessId()
        {
            IntPtr hWnd = Native.GetProcessWindow(7564);
            if (hWnd != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1024);
                Native.GetWindowText(new System.Runtime.InteropServices.HandleRef(this, hWnd), sb, sb.Capacity);
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
                timer.Interval = 300;
                timer.Tick += Timer_Tick;

                Log.Add("Timer is setup...");
            }

            this.IsRunning = true;
            btnPause.Text = "暫停";
            
            script.Init();
            timer.Start();
            tickCount = 0;
            startTime = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsRunning) {
                Log.Add("Paused. Skip this tick...");
                timer.Stop();
                return;
            }
            // Check time
            DateTime now = DateTime.Now;
            TimeSpan diff = now - startTime;
            DateTime to = startTime.AddSeconds(maxSeconds);

            if (diff.TotalSeconds > maxSeconds)
            {
                // 先停止
                Log.Add("Exceeds the maxSeconds, force stop.");
                timer.Stop();
                script.QuitTeam();
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Tick ({0}): ({2})   total seconds: {1}\r\n", tickCount, diff.TotalSeconds.ToString("0.##"), maxSeconds);
            sb.AppendFormat("停止時間: {0} \r\n", to.ToShortTimeString());
            
            Log.Add(sb.ToString());

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

            if (this.IsRunning)
            {
                this.IsRunning = false;
                Log.Add("Pause..... \r\n");

                script.Stop();
                timer.Stop();

                btnPause.Text = "繼續";
            }
            else {
                this.IsRunning = true;
                Log.Add("Resume .... \r\n");

                script.Resume();
                timer.Start();

                btnPause.Text = "暫停";
            }

            
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

        private void button1_Click(object sender, EventArgs e)
        {
            FormOption child = new FormOption();
            if (DialogResult.OK == child.ShowDialog()) {
                MySetting conf = MySetting.Instance;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("設定參數:");
                sb.AppendFormat("時間: {0}\r\n", conf.MaxSeconds);
                sb.AppendFormat("步數: {0}\r\n", conf.LoopMax);
                sb.AppendFormat("策略: {0}\r\n", conf.Strategy.ToString());

                maxSeconds = conf.MaxSeconds;

                Log.Add(sb.ToString());     
            }
        }
    }
}
