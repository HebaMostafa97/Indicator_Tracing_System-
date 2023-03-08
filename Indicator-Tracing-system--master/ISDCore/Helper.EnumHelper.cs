using System;

namespace ISDCore
{
    public enum EnumAppSetting
    { Unknown = 0, DefaultLanguage = 3, RecordPerPage = 4 };

    public enum EnumDatatype
    { Unknown = 0, Identifier, String, Email, Url, Datetime, Date, Time, Integer, Lookup, Enum, Boolean, File, Decimal, Float, Password, Memo, RichText, Document, Image, Video };
    public enum EnumSqlCondition
    { Unknown, Equal, NotEqual, GreaterThan, LessThan, In, NotIn, Between, Like, LikePattern, Is };

    public enum EnumProcType
    { Unknown, Select, Insert, Update, Delete, Toggle, Count, Index, DeleteGeneric };

    public enum EnumPopulateSource
    { Unknown, Reset, Request, Database };
    public enum EnumFunctionAction
    { Unknown, Add, Save, Delete, Toggle, Bookmark, Navigate, NavigateToChild,Browse,Preview,Edit };

    public enum EnumLanguage
    { Unknown = 0, Arabic = 1, English = 2, All = Arabic | English };
    public enum EnumRenderMode
    { Unknown = 0, Edit = 1, Add = 2, EditAdd = 3, Preview = 4 }
    public enum EnumPermission
    { Inherit = 0, Allow, Deny };
    public enum EnumRedirct
    {Home, AccessDenied}
    public enum EnumCalendarMode
    { Unknown, Date, Time, DateTime, Monthly, };
    public enum EnumFormOpenMode { Self, Popup, New };
    public enum EnumDataTypesLength
    {
      ISD_Notes = 4000, ISD_abbreviation = 16, ISD_Attachment = 128, ISD_Name = 255, ISD_Description = 2048, ISD_Email = 255, ISD_Fax = 64, ISD_File = 255, ISD_Guid = 36, ISD_Html = 4000, ISD_LookupName = 64, ISD_ObjectName = 64, ISD_Password = 100, ISD_Phone = 64,
      ISD_Text1024 = 1024, ISD_Text16 = 16, ISD_Text2048 = 2048, ISD_Text255 = 255, ISD_Text32 = 32, ISD_Text4000 = 4000, ISD_Text512 = 512, ISD_Url = 512, ISD_Xml = 4000
    };

  public class EnumHelper
    {
        public static string Enum2String<EnumType>(EnumType value)
        {
            return Enum.GetName(typeof(EnumType), value);
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
            return (EnumType)Enum.Parse(typeof(EnumType), name);
        }
        public static EnumType MapEnum<EnumType>(string val)
        { return (EnumType)Enum.Parse(typeof(EnumType), val); }
        protected static string MapEnum<EnumType>(EnumType val)
        { return Enum.GetName(typeof(EnumType), val); }
        private static string[] _Dattypes = { "Unknown", "Identifier", "String", "Email", "Url", "Datetime", "Date", "Time", "Integer", "Lookup", "Enum", "Boolean", "File", "Decimal", "Float", "Password", "Memo", "RichText", "Document", "Image", "Video" };
        public static EnumDatatype MapDataType(string Datatype)
        { return (EnumDatatype)String2Enum(_Dattypes, Datatype, (int)EnumDatatype.Unknown); }
        public static string MapDataType(EnumDatatype DataType)
        {
            string Ret = _Dattypes[0];
            Ret = Enum2String(_Dattypes, (int)DataType, _Dattypes[0]);
            return Ret;
        }
        private static string[] _ProcTypes = { "UNK", "???", "INS", "UPD", "DEL", "???", "???", "???", "???" };
        public static string MapProcName(EnumProcType ProcType)
        {
            return Enum2String(_ProcTypes, (int)ProcType, _ProcTypes[0]);
        }

        private static string[] _FunctionActions = { "Add", "Save", "Delete", "Toggle", "Navigate", "NavigateToChild" };
        public static string MapFunctionAction(EnumFunctionAction ProcType)
        {
            return Enum2String(_FunctionActions, (int)ProcType, _FunctionActions[0]);
        }

        private static string[] _Languages = { "", "A", "E", "All" };
        private static string[] _LanguageNames = { "", "Arabic", "English", "" };
        public static string[] Languages { get { return _Languages; } }
        public static string MapLanguage(EnumLanguage Language)
        { return Enum2String(_Languages, (int)Language, _Languages[0]); }
        public static EnumLanguage MapLanguage(string Language)
        { return (EnumLanguage)String2Enum(_Languages, Language, (int)EnumLanguage.Unknown); }
        public static string MapLanguageName(EnumLanguage Language)
        { return Enum2String(_LanguageNames, (int)Language, _LanguageNames[0]); }
        public static EnumLanguage[] GetLanguages()
        {
            EnumLanguage[] Ret = new EnumLanguage[_Languages.Length - 2];
            for (int i = 1; i < _Languages.Length - 1; i++) // bypass the first Unknown and last All
                Ret[i - 1] = MapLanguage(_Languages[i]);
            return Ret;
        }

        public static string MapRenderMode(EnumRenderMode RenderMode)
        { return MapEnum<EnumRenderMode>(RenderMode); }
        public static EnumRenderMode MapRenderMode(string RenderMode)
        { return MapEnum<EnumRenderMode>(RenderMode); }

        private static string[] _SqlCondition = { "=", "=", "<>", ">=", "<=", "IN", "NOT IN", "BETWEEN", "LIKE", "LIKE", "IS" };
        public static string MapSqlCondition(EnumSqlCondition SqlCondition)
        {
            return Enum2String(_SqlCondition, (int)SqlCondition, _SqlCondition[0]);
        }
    }
}
