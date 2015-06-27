using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public virtual int LoopMax() {
            return Math.Max(3, _loopMax);
        }

        public BaseStrategy(int loopMax)
        {
            _loopMax = loopMax;
        }
        public BaseStrategy(int loopMax, DIR_STATEGY strategy) : this(loopMax)
        {
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
    }
}
