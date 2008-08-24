// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace WudiStudio.Chestnut.Utilities
{
    public static class Global
    {
        public static ResourceManager Resources;

        public static int CountNodes = 0;

        public static void SetCulture(string name)
        {
            Global.Resources = new ResourceManager("WudiStudio.Chestnut.Utilities.Lang_" + name, typeof(Global).Assembly);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(name);
        }

        public static string GetText(string name)
        {
            return Global.Resources.GetString(name);
        }
    }
}
