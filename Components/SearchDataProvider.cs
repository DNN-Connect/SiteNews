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
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Search.Controllers;
using DotNetNuke.Services.Search.Entities;
using DotNetNuke.Services.Search.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Connect.Modules.SiteNews
{
   public class SearchDataProvider
   {
      private ModuleInfo _module;
      private UserInfo _user;
      private PortalSettings _portalSettings;
      protected int NumberOfDays
      {
         get
         {
            object o = _module.ModuleSettings["NumberOfDays"];
            int numberOfDays;
            try
            {
               if (!(Int32.TryParse(o.ToString(), out numberOfDays)))
                  numberOfDays = 5;
            }
            catch (Exception)
            {
               numberOfDays = 5;
            }
            return numberOfDays;
         }
      }

      protected List<int> SearchTypes
      {
         get
         {
            object o = _module.ModuleSettings["SearchTypes"];
            List<int> searchTypes = new List<int>();
            if (o == null)
            {
               searchTypes = SiteNewsHelpers.AllSearchTypes.Select(x => x.SearchTypeId).ToList();
            }
            else
            {
               searchTypes = o.ToString().Split(new char[] { ',' }).Select(int.Parse).ToList();
            }
            return searchTypes;
         }
      }

      protected int NumberOfResults
      {
         get
         {
            object o = _module.ModuleSettings["NumberOfResults"];
            int numberOfResults;
            try
            {
               if (!(Int32.TryParse(o.ToString(), out numberOfResults)))
                  numberOfResults = 25;
            }
            catch (Exception)
            {
               numberOfResults = 25;
            }
            return numberOfResults;
         }
      }

      protected bool IncludeDescription
      {
         get { return Convert.ToBoolean(_module.ModuleSettings["IncludeDescription"]); }
      }

      protected bool IncludeBody
      {
         get { return Convert.ToBoolean(_module.ModuleSettings["IncludeBody"]); }
      }

      protected bool IncludeAuthor
      {
         get { return Convert.ToBoolean(_module.ModuleSettings["IncludeAuthor"]); }
      }

      protected bool IncludeSnippet
      {
         get { return Convert.ToBoolean(_module.ModuleSettings["IncludeSnippet"]); }
      }

      public SearchDataProvider(ModuleInfo Module, UserInfo User, PortalSettings PortalSettings)
      {
         _module = Module;
         _user = User;
         _portalSettings = PortalSettings;
      }

      public IList<SearchResult> GetNewItems()
      {
         IList<SearchResult> searchResults = null;

         SearchQuery query = new SearchQuery();
         query.AllowLeadingWildcard = true;
         query.KeyWords = "*";
         query.PortalIds = new List<int>() { _module.PortalID };
         query.Locale = CultureInfo.CurrentCulture.Name;
         query.BeginModifiedTimeUtc = DateTime.UtcNow.AddDays(NumberOfDays * -1);
         query.EndModifiedTimeUtc = DateTime.UtcNow;
         query.SearchTypeIds = SearchTypes;
         query.SortField = SortFields.LastModified;
         query.SortDirection = SortDirections.Descending;
         query.WildCardSearch = true;

         int numberOfResults = NumberOfResults;

         query.PageSize = numberOfResults;

         searchResults = SearchController.Instance.SiteSearch(query).Results
            .Where(
               r => (
                  ((r.SearchTypeId == SearchHelper.Instance.GetSearchTypeByName("module").SearchTypeId) && (r.ModuleId != _module.ModuleID)) ||
                  ((r.SearchTypeId == SearchHelper.Instance.GetSearchTypeByName("tab").SearchTypeId) && (r.TabId != _module.TabID))
                  )
             )
            .GroupBy(r => new { r.SearchTypeId, r.ModuleId, r.TabId, r.Title, r.Description })
            .Select(g => g.LastOrDefault())
            .ToList();

         return searchResults;
      }
   }
}