using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Helper
{
    public class ValidationHelper
    {
        public static bool CheckPersonalIdNumber(string value)
        {
            if (value.Length != 10)
            {
                return false;
            }
            foreach (char digit in value)
            {
                if (!Char.IsDigit(digit))
                {
                    return false;
                }
            }
            int month = int.Parse(value.Substring(2, 2));
            int year = 0;
            if (month < 13)
            {
                year = int.Parse("19" + value.Substring(0, 2));
            }
            else if (month < 33)
            {
                month -= 20;
                year = int.Parse("18" + value.Substring(0, 2));
            }
            else
            {
                month -= 40;
                year = int.Parse("20" + value.Substring(0, 2));
            }
            int day = int.Parse(value.Substring(4, 2));

            try
            {
                var dateOfBirth = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            int[] weights = new int[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            int totalControlSum = 0;
            for (int i = 0; i < 9; i++)
            {
                totalControlSum += weights[i] * (value[i] - '0');
            }
            int controlDigit = 0;
            int reminder = totalControlSum % 11;
            if (reminder < 10)
            {
                controlDigit = reminder;
            }
            int lastDigitFromIDNumber = int.Parse(value.Substring(9));
            if (lastDigitFromIDNumber != controlDigit)
            {
                return false;
            }
            return true;
        }

        public static bool CheckCompanyIdNumber(string value)
        {
            if (value.Length != 9 && value.Length != 13)
            {
                return false;
            }
            int checkSum1 = 0;
            int checkSum2 = 0;
            for (int i = 0; i < 8; i++)
            {
                char currentDigit = value[i];
                if (!Char.IsDigit(currentDigit))
                {
                    return false;
                }
                checkSum1 += (currentDigit - 48) * (i + 1);
                checkSum2 += (currentDigit - 48) * (i + 3);
            }
            int controlDigit = checkSum1 % 11;
            if (controlDigit == 10)
                controlDigit = checkSum2 % 11;
            if (controlDigit == 10)
                controlDigit = 0;
            if (Convert.ToInt16(value[8]) != controlDigit + 48)
            {
                return false;
            }
            if (value.Length == 13)
            {
                int[] weight1 = { 2, 7, 3, 5 };
                int[] weight2 = { 4, 9, 5, 7 };
                checkSum1 = 0;
                checkSum2 = 0;
                for (int i = 8; i < 13; i++)
                {
                    char currentDigit = value[i];
                    if (!Char.IsDigit(currentDigit))
                    {
                        return false;
                    }
                    checkSum1 += (currentDigit - 48) * weight1[i - 8];
                    checkSum2 += (currentDigit - 48) * weight2[i - 8];
                }
                controlDigit = checkSum1 % 11;
                if (controlDigit == 10)
                    controlDigit = checkSum2 % 11;
                if (controlDigit == 10)
                    controlDigit = 0;
                if (controlDigit + 48 != Convert.ToInt16(value[12]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
