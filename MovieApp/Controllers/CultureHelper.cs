using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Controllers
{
    public class CultureHelper
    {
        public static Dictionary<string, string> CountryList()
        {
            Dictionary<string, string> cultureList = new Dictionary<string, string>();

            CultureInfo[] cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultureInfo)
            {
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                if (!(cultureList.ContainsKey(regionInfo.Name)))
                {
                    cultureList.Add(regionInfo.Name, regionInfo.EnglishName);
                }
            }
            return cultureList;
        }

        public static Dictionary<string, string> LanguageList()
        {
            Dictionary<string, string> languageList = new Dictionary<string, string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultures)
            {
                var cultureIdentifier = culture.Name.Substring(0, 2);
                if (!languageList.ContainsKey(cultureIdentifier))
                {
                    languageList.Add(cultureIdentifier, culture.EnglishName);
                }
            }
            return languageList;
        }

        public static string GetLanguageByIdentifier(string language)
        {
            return language != null ? new CultureInfo(language).EnglishName : "";
        }

        public static string GetCountryByIdentifier(string country)
        {
            return country != null ? new RegionInfo(country).EnglishName : "";
        }
    }
}
