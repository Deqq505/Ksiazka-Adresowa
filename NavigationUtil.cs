using System;
using Microsoft.Maui.Controls;

namespace Ksiazka_Adresowa;

public static class NavigationUtil
{
    public static T GetQueryParameter<T>(Page page, string key)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

        
        if (Shell.Current?.CurrentState?.Location is Uri uri)
        {
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            if (query[key] != null)
            {
                var value = query[key];
                if (value is T typedValue)
                {
                    return typedValue;
                }
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return default;
                }
            }
        }

        return default;
    }
}