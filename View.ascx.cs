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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Search.Entities;
using DotNetNuke.Services.Search.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Connect.Modules.SiteNews
{
   public partial class View : PortalModuleBase
   {
      private SearchDataProvider _provider = null;
      protected SearchDataProvider Provider
      {
         get
         {
            if (_provider == null)
               _provider = new SearchDataProvider(ModuleConfiguration, UserInfo, PortalSettings);
            return _provider;
         }
      }

      protected int FoundItems { get; set; }

#region Settings
      protected int NumberOfDays
      {
         get
         {
            object o = Settings["NumberOfDays"];
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

      protected int PageSize
      {
         get
         {
            int pageSize = 5;
            object o = Settings["PageSize"];
            if (o != null)
               pageSize = Convert.ToInt32(o);
            return pageSize;
         }
      }

      protected bool IncludeSearchType
      {
         get { return Convert.ToBoolean(Settings["IncludeSearchType"]); }
      }

      protected bool IncludeInfo
      {
         get { return Convert.ToBoolean(Settings["IncludeInfo"]); }
      }

      protected bool IncludeDescription
      {
         get { return Convert.ToBoolean(Settings["IncludeDescription"]); }
      }

      protected bool IncludeBody
      {
         get { return Convert.ToBoolean(Settings["IncludeBody"]); }
      }

      protected bool IncludeAuthor
      {
         get { return Convert.ToBoolean(Settings["IncludeAuthor"]); }
      }

      protected bool IncludeSnippet
      {
         get { return Convert.ToBoolean(Settings["IncludeSnippet"]); }
      }
#endregion

      protected void Page_PreRender(object sender, EventArgs e)
      {
         BindData();
      }

      protected void ResultsList_ItemDataBound(object sender, ListViewItemEventArgs e)
      {
         switch (e.Item.ItemType)
         {
            case ListViewItemType.DataItem:
               SearchResult result = (SearchResult)e.Item.DataItem;

               // The Type Label is not really interesting... only for Admins/Debugging purposes
               Label typeLabel = (Label)e.Item.FindControl("TypeLabel");
               typeLabel.Visible = ((IncludeSearchType) && (!(string.IsNullOrEmpty(GetSearchTypeName(result.SearchTypeId)))));

               Label descriptionLabel = (Label)e.Item.FindControl("DescriptionLabel");
               descriptionLabel.Visible = ((IncludeDescription) && (!(string.IsNullOrEmpty(result.Description))));

               Label bodyLabel = (Label)e.Item.FindControl("BodyLabel");
               bodyLabel.Visible = ((IncludeBody) && (!(string.IsNullOrEmpty(result.Body))));

               Label authorNameLabel = (Label)e.Item.FindControl("AuthorNameLabel");
               authorNameLabel.Visible = ((IncludeAuthor && (!string.IsNullOrEmpty(result.AuthorName))));

               Label displayModifiedTimeLabel = (Label)e.Item.FindControl("DisplayModifiedTimeLabel");
               displayModifiedTimeLabel.Visible = (!(string.IsNullOrEmpty(result.DisplayModifiedTime)));

               Label snippetLabel = (Label)e.Item.FindControl("SnippetLabel");
               snippetLabel.Visible = ((IncludeSnippet) && (!(string.IsNullOrEmpty(result.Snippet))));

               break;
            default:
               break;
         }
      }

      protected string GetSearchTypeName(object searchType)
      {
         List<SearchType> searchTypes = SearchHelper.Instance.GetSearchTypes().ToList();
         string searchTypeName = searchTypes.Find(s => s.SearchTypeId == (int)searchType).SearchTypeName;
         string result = Localization.GetString(string.Format("Crawler_{0}.Text", searchTypeName.ToString()), SiteNewsHelpers.SearchableModulesResourceFile);
         if (string.IsNullOrEmpty(result))
            result = searchTypeName;
         return result;
      }

      private void BindData()
      {
         try
         {
            IList<SearchResult> searchResult = Provider.GetNewItems();
            ResultsList.DataSource = searchResult;
            FoundItems = searchResult.Count;
            ResultsList.DataBind();
            if (IncludeInfo)
            {
               ((Panel)ResultsList.FindControl("InfoPanel")).Visible = true;
               ((Label)ResultsList.FindControl("FoundItems")).Text = string.Format(Localization.GetString("FoundItems.Text", LocalResourceFile), FoundItems, NumberOfDays);
            }
            else
            {
               ((Panel)ResultsList.FindControl("InfoPanel")).Visible = false;
            }
            ResultsPager.Visible = (FoundItems > PageSize);
         }
         catch (Exception exc)
         {
            NoNewsFoundMessagePanel.Visible = true;
            Exceptions.LogException(exc);
         }
      }

      protected void ResultsPager_Load(object sender, EventArgs e)
      {
         DataPager resultsPager = (DataPager)sender;
         resultsPager.PageSize = PageSize;
      }
   }
}