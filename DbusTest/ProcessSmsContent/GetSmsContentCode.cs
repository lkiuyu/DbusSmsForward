using DbusSmsForward.Helper;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using System.Text.RegularExpressions;

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
            string newsmscontent=string.Empty;

            if (JudgeSmsContentHasCode(smscontent,out newsmscontent))
            {
                string smscode= GetCode(newsmscontent).Trim();
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
        public static bool JudgeSmsContentHasCode(string smscontent,out string newsmscontent)
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string codeKeyStr = result.appSettings.SmsCodeKey;
            result = null;
            string[] flagStrList = {};
            try
            {
                flagStrList = codeKeyStr.Split("±");
            }
            catch (Exception ex)
            {
                Console.WriteLine("配置文件中自定义验证码关键词格式有误，已使用默认关键词“验证码”");
                flagStrList.Append("验证码");
            }

            foreach (string flag in flagStrList)
            {
                if (smscontent.IndexOf(flag) > -1)
                {
                    newsmscontent = smscontent.Replace(flag, " "+flag+" ");
                    return true;
                }
            }
            newsmscontent=string.Empty;
            return false;
        }
        public static string GetCode(string smscontent)
        {
            string pattern = @"\b[A-Za-z0-9]{4,7}\b";
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
            else
            {
                string pattern1 = @"【([^【】]+)】$";
                Match match1 = Regex.Match(smscontent, pattern1);
                if (match1.Success)
                {
                    return match1.Value;
                }
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
