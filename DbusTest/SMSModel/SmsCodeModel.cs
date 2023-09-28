using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SMSModel
{
    public class SmsCodeModel
    {
        public bool HasCode { get; set; }
        public string CodeFrom { get; set; }
        public string CodeValue { get; set; }
    }
}
