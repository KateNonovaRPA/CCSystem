using System.Collections.Generic;
using System.Linq;


namespace Models
{
    /// <summary>
    /// Глобални константи
    /// </summary>
    public class GlobalConstants
    {
        public const int LoginTimeout = 15; // in min

        public const string DefaultLang = "bg";
        public const string TextLangEN = "en";
        public const string DateFormat = "dd.MM.yyyy";
        public const int BulgariaCountryId = 155;
        public const int LangBG = 1;
        public const int LangEN = 2;
        public static int GetLanguageId(string lang)
        {
            switch ((lang ?? DefaultLang).ToLower())
            {
                case TextLangEN:
                    return LangEN;
                default:
                    return LangBG;
            }
        }
    }



}

