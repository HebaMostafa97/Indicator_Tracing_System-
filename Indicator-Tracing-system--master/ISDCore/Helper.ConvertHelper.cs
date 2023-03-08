using System;
using System.Text;

namespace ISDCore
{
    public enum EnumFormatNumber
    { Unknown, Percentage, };

    public static class ConvertHelper
    {
        public static VarType Convert<VarType>(object Value, VarType def) //where VarType : Type
        {
            VarType Ret = def;
            if (null != Value && !(Value is DBNull))
            {
                try
                {
                    Ret = (VarType)Value;
                }
                catch
                {
                    //this is a specialhandle for casting enum values
                    try
                    {
                        Ret = (VarType)System.Convert.ChangeType(Value, typeof(int));
                    }
                    catch
                    {
                        ;
                    }
                }
            }
            return Ret;
        }
        public static int StringToInt(string val)
        {
            int Ret = 0;
            //Ret = System.Convert.ToInt32(val);
            int.TryParse(val, out Ret);
            return Ret;
        }

        public static byte[] EncryptValue(string value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(value));
            return hashedDataBytes;
        }

        //AbdElHaliem
        public static string ArrayByte2String(byte[] value)
        {
            return System.Convert.ToBase64String(value);
            //return Encoding.UTF8.GetString(value);
            //return Encoding.ASCII.GetString(value);
        }
        public static byte[] String2ArrayByte(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return System.Convert.FromBase64String(value);
            return
              new byte[0];
            //return Encoding.UTF8.GetBytes(value);
            //return Encoding.ASCII.GetBytes(value);
        }

        //Lamiaa
        public static bool CompareByte(Byte[] value1, Byte[] value2)
        {

            bool Ret = true;
            if (value1.Length == value2.Length)
            {
                for (int i = 0; i < value1.Length; i++)
                {
                    if (value1[i] != value2[i])
                    {
                        Ret = false;
                        break;
                    }
                }

            }
            else
                Ret = false;
            return Ret;
        }

        public static string Enum2String(string[] array, int Enum, string Default)
        {
            string Ret = Default;
            if (Enum >= 0 && Enum < array.Length)
                Ret = array[Enum];

            return Ret;
        }
        public static int String2Enum(string[] array, string Value, int Default)
        {
            int Ret = Default;
            char[] spliter = { ',', '|' };
            string[] Values = Value.Split(spliter);

            for (int i = 0; i < Values.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (Values[i].Equals(array[j]))
                    {
                        Ret |= j;
                    }
                }
            }
            return Ret;
        }
        public static EnumType String2Enum<EnumType>(string name)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), name, true);
        }
        public static string Enum2String<EnumType>(EnumType value)
        {
            return Enum.GetName(typeof(EnumType), value);
        }

        public static string FormatValue(decimal value, EnumFormatNumber format)
        {
            string Ret = "";
            string stringFormat = "";

            switch (format)
            {
                case EnumFormatNumber.Unknown:
                    break;
                case EnumFormatNumber.Percentage:
                    stringFormat = "P1";  //, new CultureInfo("en-us"));  value.ToString("#.## %");
                    break;
                default:
                    break;
            }

            Ret = value.ToString(stringFormat);

            return Ret;
        }

        public static string ConvertToEasternArabicNumerals(string input)
        {
            System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
            System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
            System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
            char[] convertedChar = new char[1];
            byte[] bytes = new byte[] { 217, 160 };
            char[] inputCharArray = input.ToCharArray();
            string convertedStr = NullHelper.String;
            //if ("A" == ConvertHelper.Convert<string>(Settings.ReadAppSettings("defaultLanguage"), NullHelper.String))
            //if (EnumLanguage.Arabic == Languages.Language)
            //{
            //  foreach (char c in inputCharArray)
            //  {
            //    if (char.IsDigit(c))
            //    {
            //      bytes[1] = System.Convert.ToByte(160 + char.GetNumericValue(c));
            //      utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
            //      convertedChars.Append(convertedChar[0]);
            //    }
            //    else
            //    {
            //      convertedChars.Append(c);
            //    }
            //  }
            //  convertedStr = convertedChars.ToString().Replace(".", ",");
            //}
            //else
            //{
            convertedStr = input;
            // }
            return convertedStr;


        }

    }
}
