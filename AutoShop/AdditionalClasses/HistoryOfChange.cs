using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShop.AdditionalClasses
{
    public class HistoryOfChange
    {
        public string ManagerWhoChanged { get; set; }
        public string ManagerWhoWasChanged { get; set; }
        public string Info { get; set; }
        public DateTime DateChange { get; set; }
    }
}
