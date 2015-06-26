using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsInput;
using WindowsInput.Native;

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

            hHwnd = WindowHelper.FindWindow(null, "魔力寶貝II");
            if (hHwnd == IntPtr.Zero)
            {
                logViewer.Add("Failed to get window.");
                initOK = false;
                return;
            }

            pause = false;
            logViewer.Add("Get window success.");
            WindowHelper.SetForegroundWindow(hHwnd);
        }

        private static readonly Random rand = new Random();
        InputSimulator aIS = new InputSimulator();

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

            aIS.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
            for (int i = 0; i < loopMax && !pause; i++)
            {
                aIS.Keyboard.KeyDown(key);
                
                int ms = 400 + rand.Next() % 100;
                Thread.Sleep(ms);

                aIS.Keyboard.KeyUp(key);
            }
            aIS.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);

            /*
            Dictionary<int, string> dirMap = new Dictionary<int, string>() {
                {0, @"^{up}" },
                {1, @"^{down}" },
                {2, @"^{left}" },
                {3, @"^{right}" },
            };
            int dir = rand.Next() % 4;
            String cmd = dirMap[dir];
            logViewer.Add("cmd: " + cmd);
            SendKeys.SendWait(cmd);
            */
            
            /*
            Dictionary<int, int> dirMap = new Dictionary<int, int>() {
                {0, WindowHelper.VK_UP },
                {1, WindowHelper.VK_LEFT },
                {2, WindowHelper.VK_RIGHT },
                {3, WindowHelper.VK_DOWN },
            };

            int dir = rand.Next() % 4;
            IntPtr curDir = (IntPtr)dirMap[dir];
            lastDir = curDir;

            logViewer.Add("KEY_DOWN: " + dir);
            WindowHelper.PostMessage(hHwnd, WindowHelper.WM_KEYDOWN, (IntPtr)VirtualKeyCode.LCONTROL, (IntPtr)0x001D0001);
            WindowHelper.PostMessage(hHwnd, WindowHelper.WM_KEYDOWN, curDir, (IntPtr)0x00170001);

            Thread.Sleep(500 + rand.Next() % 50);

            logViewer.Add("KEY_UP: " + dir);
            WindowHelper.PostMessage(hHwnd, WindowHelper.WM_KEYUP, curDir, (IntPtr)0x00170001);
            WindowHelper.PostMessage(hHwnd, WindowHelper.WM_KEYUP, (IntPtr)VirtualKeyCode.LCONTROL, (IntPtr)0x001D0001); 
            */
        }

        public void Stop() {
            pause = true;
            aIS.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);

            /*
            WindowHelper.SendMessage(hHwnd, WindowHelper.WM_KEYUP, (IntPtr)WindowHelper.VK_CTRL, IntPtr.Zero);
            WindowHelper.SendMessage(hHwnd, WindowHelper.WM_KEYUP, lastDir, IntPtr.Zero);
            */
        }
    }
}
