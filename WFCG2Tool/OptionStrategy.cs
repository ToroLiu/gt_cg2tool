using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFCG2Tool.Strategies;

namespace WFCG2Tool
{
    public class OptionStrategy
    {
        public DIR_STATEGY Strategy { get; set; }
        public string Description { get; set; }

        public OptionStrategy(DIR_STATEGY dir, String desc)
        {
            this.Strategy = dir;
            this.Description = desc;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
