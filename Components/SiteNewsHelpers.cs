/*
DNN-Connect - http://dnn-connect.org
Copyright (c) 2017
by DNN-Connect
Written by Michael Tobisch

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Search.Entities;
using DotNetNuke.Services.Search.Internals;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Connect.Modules.SiteNews
{
   public static class SiteNewsHelpers
   {
      public const string SearchableModulesResourceFile = "/DesktopModules/Admin/SearchResults/App_LocalResources/SearchableModules.resx";
      public static List<SearchType> AllSearchTypes
      {
         get
         {
            return (List<SearchType>)SearchHelper.Instance.GetSearchTypes();
         }
      }

      public static int GetTabIDFromUrl(string url, int moduleID, int portalID)
      {
         int getTabIDFromUrl = 0;

         if (url.ToLower().IndexOf("tabid") > 0)
         {
            Int32.TryParse(Regex.Match(url, "tabid[=/](\\d+)", RegexOptions.IgnoreCase).Groups[1].Value, out getTabIDFromUrl);
         }

         if (getTabIDFromUrl == 0)
         {
            IList<ModuleInfo> modules = ModuleController.Instance.GetTabModulesByModule(moduleID);

            foreach (ModuleInfo module in modules)
            {
               TabInfo tab = TabController.Instance.GetTab(module.TabID, portalID);
               if (url.StartsWith(tab.FullUrl))
               {
                  getTabIDFromUrl = tab.TabID;
                  break;
               }
            }
         }
         return getTabIDFromUrl;
      }

      public static bool IsProfilePage(int tabID, int userTabID, int portalID)
      {
         TabInfo tab = TabController.Instance.GetTab(tabID, portalID);
         bool isProfilePage = (tab.TabID == userTabID);
         
         while (!(isProfilePage))
         {
            if (tab.ParentId <= 0)
               break;
            tab = TabController.Instance.GetTab(tab.ParentId, tab.PortalID);
            isProfilePage = (tab.TabID == userTabID);
         }
         return isProfilePage;
      }

      public static bool IsAdminPage(int tabID, int adminTabID, int portalID)
      {
         TabInfo tab = TabController.Instance.GetTab(tabID, portalID);
         bool isAdminPage = (tab.TabID == adminTabID);

         while (!(isAdminPage))
         {
            if (tab.ParentId <= 0)
               break;
            tab = TabController.Instance.GetTab(tab.ParentId, tab.PortalID);
            isAdminPage = (tab.TabID == adminTabID);
         }
         return isAdminPage;

      }
   }
}