using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WFCG2Tool.Code;
using WFCG2Tool.Strategies;

namespace WFCG2Tool
{
    // 可被pause
    // 可以把要做的事情，整在一起。
    public class Scripter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
  (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private LogViewer logViewer;
        private IntPtr hHwnd { get; set; }
        
        public bool pause { get; set; }
        public bool initOK { get; set; }
        public IntPtr lastDir { get; set; }

        public Scripter(LogViewer log)
        {
            logViewer = log;
        }
        
        public void Init()
        {
            logViewer.Add("Scripter Init()");
            initOK = true;

            hHwnd = Native.FindWindow(null, "魔力寶貝II");
            if (hHwnd == IntPtr.Zero)
            {
                logViewer.Add("Failed to get window.");
                initOK = false;
                return;
            }

            pause = false;
            logViewer.Add("Get window success.");
            Native.SetForegroundWindow(hHwnd);
            Native.SetActiveWindow(hHwnd);

            LoadConf();
        }

        public void SetAppFocus()
        {
            Native.SetForegroundWindow(hHwnd);
            Native.SetActiveWindow(hHwnd);

            logViewer.Add("SetAppFocus()");
        }

        private static readonly Random rand = new Random();
        private static int invertDir = 0;
        private static int circlrDir = 0;

        private BaseStrategy strategy = new BaseStrategy(10, 1, DIR_STATEGY.UP_DOWN);

        public void LoadConf() {
            MySetting conf = MySetting.Instance;

            switch (conf.Strategy) {
                case DIR_STATEGY.UP_DOWN:
                case DIR_STATEGY.LEFT_RIGHT:
                case DIR_STATEGY.CIRCLE:
                    strategy = new BaseStrategy(conf.LoopMax, conf.RandomFactor, conf.Strategy);
                    break;

                case DIR_STATEGY.RT_LB:
                    strategy = new RightTopStrategy(conf.LoopMax, conf.RandomFactor);
                    break;

                case DIR_STATEGY.LT_RB:
                    strategy = new TopLeftStrategy(conf.LoopMax, conf.RandomFactor);
                    break;
            }

            String msg = String.Format("步數: {0}  方向: {1}\r\n", conf.LoopMax, conf.Strategy.ToString());
            logViewer.Add(msg);
        }

        private int SLEEP_BASE = 10;

        public void RunIt() {
            if (pause)
                return;

            logViewer.Add("PrepareNextAction");
            strategy.PrepareNextAction();
            Application.DoEvents();

            Native.SetActiveWindow(hHwnd);

            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, 0, 0);
            for (int i = 0; i < strategy.LoopMax() && !pause; ++i)
            {
                Thread.Sleep(SLEEP_BASE);
                strategy.DoAction();

                Thread.Sleep(SLEEP_BASE + rand.Next() % 10);
                Application.DoEvents();
            }

            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, (int)KEYEVENTF.KEYUP, 0);
            Application.DoEvents();
            Thread.Sleep(SLEEP_BASE);
        }

        public void Stop() {
            pause = true;
            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, (int)KEYEVENTF.KEYUP, 0);
        }

        public void Resume() {
            pause = false;

            LoadConf();
            logViewer.Add("繼續執行");
            Native.SetForegroundWindow(hHwnd);
        }

        public void QuitTeam()
        {
            // 先檢查
            bool needQuit = MySetting.Instance.QuitTeam;
            if (!needQuit) {
                return;
            }

            logViewer.Add("解散團隊");

            VirtualKeyCode alt = VirtualKeyCode.LMENU;

            // ALT的vkCode, 是VK_MENU...
            Native.keybd_event((byte)alt, 0, 0, 0);
            for (int i = 0; i < 300 && !pause; ++i)
            {
                Thread.Sleep(100);

                logViewer.Add("Try " + i);
                Native.keybd_event((byte)VirtualKeyCode.VK_Q, 0, 0, 0);
                Application.DoEvents();

                Thread.Sleep(300 + rand.Next() % 200);
                Native.keybd_event((byte)VirtualKeyCode.VK_Q, 0, (int)KEYEVENTF.KEYUP, 0);

                //! 隔一段時間執行一次…因為可能在戰鬥中。
                Thread.Sleep(50 + rand.Next() % 50);
            }

            Native.keybd_event((byte)alt, 0, (int)KEYEVENTF.KEYUP, 0);
            Application.DoEvents();
        }
    }
}
