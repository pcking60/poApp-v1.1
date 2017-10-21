using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Common
{
    public class Export_Info_Template
    {
        public string FunctionName { get; set; }
        public string District { get; set; }
        public string Unit { get; set; }
        public string user { get; set; }
        public string Service { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string CreatedBy { get; set; }
    }
}
