using Nancy.ViewEngines.Razor;
using System.Collections.Generic;

namespace DMXForGamers.Web
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            yield return "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
            yield return "DMXForGamers.Models";
            yield return "DMXForGamers.Web";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "System";
            yield return "System.ComponentModel";
            yield return "DMXForGamers.Models";
            yield return "DMXForGamers.Web";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
