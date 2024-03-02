using System.Globalization;
using System.Resources;
using Versus.Core.Resources.Localization;

namespace Versus.Core.Services.Localization;

public class LanguageManager
{
    private static IEnumerable<CultureInfo> s_cultures = Enumerable.Empty<CultureInfo>();

    public static IEnumerable<CultureInfo> GetAvailableCultures()
    {
        if (s_cultures.Any())
        {
            return s_cultures;
        }

        List<CultureInfo> result = new List<CultureInfo>();
        ResourceManager rm = new(typeof(AppStrings));
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (CultureInfo culture in cultures)
        {
            try
            {
                if (culture.Equals(CultureInfo.InvariantCulture))
                {
                    continue;
                }

                if (rm.GetResourceSet(culture, true, false) != null)
                {
                    result.Add(culture);
                }
            }
            catch (CultureNotFoundException)
            {
            }
        }

        s_cultures = result;
        return result;
    }

    public static void ChangeCulture(CultureInfo cultureInfo)
    {
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}
