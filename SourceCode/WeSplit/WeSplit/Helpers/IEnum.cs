using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Helpers
{
    interface IEnum
    {
        public enum StatusFilter
        {
            ALL,
            PROGRESS,
            FINISH
        }
    }
}
