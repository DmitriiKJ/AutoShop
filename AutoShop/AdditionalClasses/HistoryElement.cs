using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShop.AdditionalClasses
{
    public class HistoryElement
    {
        public string ManagerFullName { get; set; }
        public string CarInfo {  get; set; }
        public string ClientFullName {  get; set; }
        public DateTime DateSell { get; set; }
        public decimal Price { get; set; }
    }
}
