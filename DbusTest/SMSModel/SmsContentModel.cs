using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SMSModel
{
    public class SmsContentModel
    {
        public string TelNumber { get; set; }
        public string SmsContent { get; set; }
        public string SmsDate { get; set; }
    }
}
