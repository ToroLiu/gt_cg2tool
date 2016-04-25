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
    public enum DIR {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT,
    }

    public enum DIR_STATEGY {
        UP_DOWN,
        LEFT_RIGHT,
        CIRCLE,    
        LT_RB,
        RT_LB,
    }
    
    public class BaseStrategy
    {
        private int _curIndex = 0;
        private static readonly List<DIR> list1 = new List<DIR> { DIR.UP, DIR.DOWN };
        private static readonly List<DIR> list2 = new List<DIR> { DIR.LEFT, DIR.RIGHT };
        private static readonly List<DIR> list3 = new List<DIR> { DIR.UP, DIR.RIGHT, DIR.DOWN, DIR.LEFT };

        private List<DIR> dirList = list1;

        public virtual DIR NextDir() {
            _curIndex = (_curIndex + 1) % dirList.Count;
            return dirList[_curIndex];
        }

        private int _loopMax = 3;
        private int _randomFactor = 1;
        public virtual int LoopMax() {
            return Math.Max(3, _loopMax);
        }
        public virtual int RandomFactor() {
            return Math.Max(1, _randomFactor);
        }

        public BaseStrategy(int loopMax, int randomFactor)
        {
            _loopMax = loopMax;
            _randomFactor = randomFactor;
        }
        
        private DIR_STATEGY _strategy;
        public BaseStrategy(int loopMax, int randomFactor, DIR_STATEGY strategy) : this(loopMax, randomFactor)
        {
            _strategy = strategy;
            
            switch (strategy) {
                default:
                case DIR_STATEGY.UP_DOWN:
                    dirList = list1;
                    break;

                case DIR_STATEGY.LEFT_RIGHT:
                    dirList = list2;
                    break;

                case DIR_STATEGY.CIRCLE:
                    dirList = list3;
                    break;
            }
        }

        public static readonly Dictionary<DIR, VirtualKeyCode> dirMap = new Dictionary<DIR, VirtualKeyCode>() {
                {DIR.LEFT, VirtualKeyCode.LEFT },
                {DIR.RIGHT, VirtualKeyCode.RIGHT },
                {DIR.UP, VirtualKeyCode.UP },
                {DIR.DOWN, VirtualKeyCode.DOWN },
        };

        private DIR currentDir = DIR.LEFT;
        /// <summary>
        /// 準備下一個動作。
        /// </summary>
        public virtual void PrepareNextAction() {
            DIR dir = NextDir();
            currentDir = dir;
        }

        /// <summary>
        /// 執行按鈕的命令
        /// </summary>
        private Random rand = new Random();
        public virtual void DoAction() {
            VirtualKeyCode key = dirMap[currentDir];

            Native.keybd_event((byte)key, 0, 0, 0);
            Application.DoEvents();

            int mod = RandomFactor() * 50;
            Thread.Sleep(100 + rand.Next() % mod);

            Native.keybd_event((byte)key, 0, (int)KEYEVENTF.KEYUP, 0);
            Application.DoEvents();
        }
    }
}
