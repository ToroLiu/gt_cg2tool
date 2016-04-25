using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFCG2Tool.Code;


namespace WFCG2Tool.Strategies
{
    public class TopLeftStrategy : BaseStrategy
    {
        public TopLeftStrategy(int loopMax, int randomFactor) : base(loopMax, randomFactor)
        {

        }
        private int currentIndex = 0;
        public override void PrepareNextAction()
        {
            currentIndex = (currentIndex + 1) % 2;
        }

        private Random rand = new Random();
        public override void DoAction()
        {
            VirtualKeyCode vk1, vk2;

            switch (currentIndex) {
                default:
                case 0:
                    vk1 = VirtualKeyCode.LEFT;
                    vk2 = VirtualKeyCode.UP;
                    break;

                case 1:
                    vk1 = VirtualKeyCode.RIGHT;
                    vk2 = VirtualKeyCode.DOWN;
                    break;
            }

            Native.keybd_event((byte)vk1, 0, 0, 0);
            Thread.Sleep(1 + rand.Next() % 5);
            Native.keybd_event((byte)vk2, 0, 0, 0);
            Application.DoEvents();

            int mod = RandomFactor() * 50;
            Thread.Sleep(100 + rand.Next() % mod);

            Native.keybd_event((byte)vk1, 0, (int)KEYEVENTF.KEYUP, 0);
            Thread.Sleep(1 + rand.Next() % 5);
            Native.keybd_event((byte)vk2, 0, (int)KEYEVENTF.KEYUP, 0);

            Application.DoEvents();
        }
        
        
    }
}
