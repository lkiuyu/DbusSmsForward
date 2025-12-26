using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SMSModel
{
    public class SmsCodeModel
    {
        public bool HasCode { get; set; }=false;
        public string CodeFrom { get; set; }=string.Empty;
        public string CodeValue { get; set; } = string.Empty;
    }
}
