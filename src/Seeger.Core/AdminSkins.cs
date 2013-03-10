using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Seeger.Templates;

namespace Seeger
{
    public class AdminSkins
    {
        static AdminSkins()
        {
            Skins = new SkinCollection("/App_Themes");
        }

        public static SkinCollection Skins { get; private set; }

        public static Skin Find(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            return Skins.FindTheme(name);
        }
    }
}
