using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SERP.Utilities.SERPHelper
{
    public static class PaymentFor
    {
        public static string GetPaymentFor(string paymentFor, string paymentType)
        {
            switch (paymentType)
            {
                case "O":
                    return "One Time";
                case "Y":
                    return "Yearly";
                case "M":
                    return GetMonthNames(paymentFor);
                case "H":
                    break;
                default:
                    return Quaterly(paymentFor);
            }
            return string.Empty;
        }

        private static string GetMonthNames(string paymentFor)
        {
            string[] paymentForArray = paymentFor.Split(',');
            List<string> paymentDataList = new List<string>();

            for (int i = 0; i < paymentForArray.Length; i++)
            {
                var name = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(paymentForArray[i]));
                paymentDataList.Add(name);
            }

            return String.Join(",", paymentDataList);
        }

        private static string Quaterly(string paymentFor)
        {
            string[] paymentForArray = paymentFor.Split(',');
            List<string> paymentDataList = new List<string>();
            for (int i = 0; i < paymentForArray.Length; i++)
            {
                paymentDataList.Add(paymentForArray[i]);
            }
            return String.Join(",", paymentDataList);
        }
    }
}
