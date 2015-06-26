using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WFCG2Tool.Code;

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
        
        public void RunIt() {

            if (pause)
                return;
            
            Dictionary<int, VirtualKeyCode> dirMap = new Dictionary<int, VirtualKeyCode>() {
                {0, VirtualKeyCode.LEFT },
                {1, VirtualKeyCode.RIGHT },
                {2, VirtualKeyCode.UP },
                {3, VirtualKeyCode.DOWN },
            };
            
            // Strategy 1: Left & Right
            //invertDir = (invertDir + 1) % 2;
            //int dir = invertDir + 2; //< UP & DOWN
            //int loopMax = 10;

            // Strategy 2: Random walk
            int dir = rand.Next() % 4;
            int loopMax = 3;
                 
            VirtualKeyCode key = dirMap[dir];
            
            logViewer.Add(key.ToString());
            Application.DoEvents();

            Native.SetActiveWindow(hHwnd);
            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, 0, 0);

            for (int i = 0; i < loopMax; ++i) {
                Native.keybd_event((byte)key, 0, 0, 0);
                Application.DoEvents();

                Thread.Sleep(500);

                Native.keybd_event((byte)key, 0, (int)KEYEVENTF.KEYUP, 0);
                Application.DoEvents();

                Thread.Sleep(100);
            }

            Native.keybd_event((byte)VirtualKeyCode.LCONTROL, 0, (int)KEYEVENTF.KEYUP, 0);
            Application.DoEvents();
        }

        public void Stop() {
            pause = true;

            Native.SendMessage(hHwnd, (uint)MSG.WM_KEYUP, (IntPtr)VirtualKeyCode.LCONTROL, IntPtr.Zero);

        }
    }
}
