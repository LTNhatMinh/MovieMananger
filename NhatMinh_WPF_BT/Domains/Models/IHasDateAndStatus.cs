using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public interface IHasDateAndStatus
    {
        DateTime GetDateToCheck();
        bool Status { get; set; }
    }
}
