using DbusSmsForward.SMSModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbusSmsForward.ProcessSmsContent
{
    public static class GetSmsContentCode
    {
        public static string GetSmsCodeStr(string smscontent)
        {
            SmsCodeModel smsCodeModel = new SmsCodeModel();
            smsCodeModel = GetSmsCodeModel(smscontent);
            if (smsCodeModel.HasCode)
            {
                return smsCodeModel.CodeFrom + smsCodeModel.CodeValue;
            }
            else
            {
                return "";
            }
        }
        public static SmsCodeModel GetSmsCodeModel(string smscontent)
        {
            SmsCodeModel smsCodeModel = new SmsCodeModel();
            if (JudgeSmsContentHasCode(smscontent))
            {
                string smscode= GetCode(smscontent).Trim();
                if (string.IsNullOrEmpty(smscode))
                {
                    smsCodeModel.HasCode = false;
                }
                else
                {
                    smsCodeModel.HasCode = true;
                    smsCodeModel.CodeValue = smscode;
                    smsCodeModel.CodeFrom = GetCodeSmsFrom(smscontent);
                }
                
            }
            else
            {
                smsCodeModel.HasCode = false;
            }
            return smsCodeModel;
        }
        public static bool JudgeSmsContentHasCode(string smscontent)
        {
            string[] flagStrList = { "验证码", "verification", "code", "인증", "代码" };
            foreach (string flag in flagStrList)
            {
                if (smscontent.IndexOf(flag) > -1)
                {
                    return true;
                }
            }
            return false;
        }
        public static string GetCode(string smscontent)
        {
            string pattern = @"(?<=\b[\p{L}\p{N}]{0,1000})[A-Za-z0-9]{4,7}\b";
            MatchCollection SmsCodeMatches = Regex.Matches(smscontent, pattern);
            if (SmsCodeMatches.Count()> 1)
            {
                int maxDigits = 0;
                string maxDigitsString = "";
                foreach (Match match in SmsCodeMatches)
                {
                    string currentString = match.Value;
                    int digitCount = CountDigits(currentString);
                    if (digitCount > maxDigits)
                    {
                        maxDigits = digitCount;
                        maxDigitsString = currentString;
                    }
                }
                return maxDigitsString;
            }
            else if (SmsCodeMatches.Count()==1)
            {
                return SmsCodeMatches[0].Value;
            }
            else
            {
                return "";
            }
        }
        public static string GetCodeSmsFrom(string smscontent)
        {
            string pattern = @"^\【(.*?)\】";
            Match match = Regex.Match(smscontent, pattern);
            if (match.Success)
            {
                return match.Value;
            }
            return "";
        }
        public static int CountDigits(string input)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
