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
        }

        private static readonly Random rand = new Random();
        private static int invertDir = 0;
        private static int circlrDir = 0;

        private BaseStrategy strategy = new BaseStrategy(10, DIR_STATEGY.UP_DOWN);

        public void RunIt() {
            if (pause)
                return;

            Dictionary<DIR, VirtualKeyCode> dirMap = new Dictionary<DIR, VirtualKeyCode>() {
                {DIR.LEFT, VirtualKeyCode.LEFT },
                {DIR.RIGHT, VirtualKeyCode.RIGHT },
                {DIR.UP, VirtualKeyCode.UP },
                {DIR.DOWN, VirtualKeyCode.DOWN },
            };

            DIR dir = strategy.NextDir();
            VirtualKeyCode key = dirMap[dir];
            
            logViewer.Add(key.ToString());
            Application.DoEvents();

            Native.SetActiveWindow(hHwnd);
            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, 0, 0);

            for (int i = 0; i < strategy.LoopMax(); ++i) {
                Native.keybd_event((byte)key, 0, 0, 0);
                Application.DoEvents();

                Thread.Sleep(400 + rand.Next() % 100);

                Native.keybd_event((byte)key, 0, (int)KEYEVENTF.KEYUP, 0);
                Application.DoEvents();

                Thread.Sleep(100 + rand.Next() % 50);
            }

            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, (int)KEYEVENTF.KEYUP, 0);
            Application.DoEvents();
            Thread.Sleep(50 + rand.Next() % 50);
        }

        public void Stop() {
            pause = true;

            Native.SendMessage(hHwnd, (uint)MSG.WM_KEYUP, (IntPtr)VirtualKeyCode.LCONTROL, IntPtr.Zero);
        }

        public void Resume() {
            pause = false;

            Native.SetForegroundWindow(hHwnd);
        }
    }
}
