using System.Globalization;

namespace NorthSouthSystems.Globalization;

public static class CultureInfoX
{
    public static void WithCulture(string name, Action action)
    {
        Throw.IfNull(action);

        var currentCulture = CurrentCulture;

        try
        {
            CurrentCulture = GetCultureInfo(name);
            action();
        }
        finally
        {
            CurrentCulture = currentCulture;
        }
    }
}
