#if !MergeSite
using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace LPush.Web.Admin
{
    public class RazorConfig : IRazorConfiguration
    {
        public bool AutoIncludeModelNamespace
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "System.Web.Razor";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "System.Web.Razor";
        }
    }
}

#endif