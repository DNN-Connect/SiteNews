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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Connect.Modules.SiteNews
{
   public partial class Settings : ModuleSettingsBase
   {
      private ModuleController _controller = null;

      protected ModuleController Controller
      {
         get
         {
            if (_controller == null)
               _controller = new ModuleController();
            return _controller;
         }
      }

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
         set
         {
            UpdateIntegerSetting("NumberOfDays", (value == 5 ? (int?)null : value));
         }
      }

      protected string SearchTypes
      {
         get
         {
            object o = Settings["SearchTypes"];
            if (o == null)
               return SiteNewsHelpers.AllSearchTypes.Select(x => x.SearchTypeId.ToString()).Aggregate((current, next) => current + "," + next);
            else
               return o.ToString();
         }
         set
         {
            UpdateTextSetting("SearchTypes", value);
         }
      }

      protected int NumberOfResults
      {
         get
         {
            object o = Settings["NumberOfResults"];
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
         set
         {
            UpdateIntegerSetting("NumberOfResults", (value == 25 ? (int?)null : value));
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
         set
         {
            UpdateIntegerSetting("PageSize", (value == 5 ? (int?)null : value));
         }
      }

      protected bool IncludeSearchType
      {
         get { return Convert.ToBoolean(Settings["IncludeSearchType"]); }
         set { UpdateBooleanSetting("IncludeSearchType", value); }
      }

      protected bool IncludeInfo
      {
         get { return Convert.ToBoolean(Settings["IncludeInfo"]); }
         set { UpdateBooleanSetting("IncludeInfo", value); }
      }

      protected bool IncludeDescription
      {
         get { return Convert.ToBoolean(Settings["IncludeDescription"]); }
         set { UpdateBooleanSetting("IncludeDescription", value); }
      }

      protected bool IncludeBody
      {
         get { return Convert.ToBoolean(Settings["IncludeBody"]); }
         set { UpdateBooleanSetting("IncludeBody", value); }
      }

      protected bool IncludeAuthor
      {
         get { return Convert.ToBoolean(Settings["IncludeAuthor"]); }
         set { UpdateBooleanSetting("IncludeAuthor", value); }
      }

      protected bool IncludeSnippet
      {
         get { return Convert.ToBoolean(Settings["IncludeSnippet"]); }
         set { UpdateBooleanSetting("IncludeSnippet", value); }
      }

      public override void LoadSettings()
      {
         try
         {
            if (!(Page.IsPostBack))
            {
               NumberOfDaysRadioButtons.SelectedValue = NumberOfDays.ToString();
               NumberOfResultsRadioButtons.SelectedValue = NumberOfResults.ToString();
               PageSizeRadioButtons.SelectedValue = PageSize.ToString();
               IncludeSearchTypeCheckBox.Checked = IncludeSearchType;
               IncludeInfoCheckBox.Checked = IncludeInfo;
               IncludeDescriptionCheckBox.Checked = IncludeDescription;
               IncludeBodyCheckBox.Checked = IncludeBody;
               IncludeAuthorCheckBox.Checked = IncludeAuthor;
               IncludeSnippetCheckBox.Checked = IncludeSnippet;
            }
         }
         catch (Exception exc) //Module failed to load
         {
            Exceptions.ProcessModuleLoadException(this, exc);
         }
      }

      public override void UpdateSettings()
      {
         try
         {
            NumberOfDays = Convert.ToInt32(NumberOfDaysRadioButtons.SelectedValue);
            List<string> selectedItems = new List<string>();
            foreach (ListItem li in SearchTypeCheckBoxes.Items)
            {
               if (li.Selected) selectedItems.Add(li.Value);
            }
            SearchTypes = selectedItems.Aggregate((current, next) => current + "," + next);
            NumberOfResults = Convert.ToInt32(NumberOfResultsRadioButtons.SelectedValue);
            PageSize = Convert.ToInt32(PageSizeRadioButtons.SelectedValue);
            IncludeSearchType = IncludeSearchTypeCheckBox.Checked;
            IncludeInfo = IncludeInfoCheckBox.Checked;
            IncludeDescription = IncludeDescriptionCheckBox.Checked;
            IncludeBody = IncludeBodyCheckBox.Checked;
            IncludeAuthor = IncludeAuthorCheckBox.Checked;
            IncludeSnippet = IncludeSnippetCheckBox.Checked;
         }
         catch (Exception ex) //Module failed to load
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void SearchTypeCheckBoxes_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            CheckBoxList searchTypeCheckBoxes = (CheckBoxList)sender;
            searchTypeCheckBoxes.DataSource = SiteNewsHelpers.AllSearchTypes;
            searchTypeCheckBoxes.DataTextField = "SearchTypeName";
            searchTypeCheckBoxes.DataValueField = "SearchTypeId";
            searchTypeCheckBoxes.DataBind();

            foreach (ListItem li in searchTypeCheckBoxes.Items)
            {
               string caption = Localization.GetString(string.Format("Crawler_{0}.Text", li.Text), SiteNewsHelpers.SearchableModulesResourceFile);
               if (!(string.IsNullOrEmpty(caption)))
                  li.Text = caption;
            }

            foreach (string s in SearchTypes.Split(new char[] { ',' }))
               SearchTypeCheckBoxes.Items.FindByValue(s).Selected = true;
         }
      }

      private void UpdateBooleanSetting(string setting, bool settingValue)
      {
         if (Settings[setting] == null)
         {
            if (settingValue)
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
         }
         else
         {
            if (settingValue)
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            else
               Controller.DeleteModuleSetting(ModuleId, setting);
         }
      }

      private void UpdateTextSetting(string setting, string settingValue)
      {
         if (Settings[setting] == null)
         {
            if (!(string.IsNullOrEmpty(settingValue)))
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue);
         }
         else
         {
            if (!(string.IsNullOrEmpty(settingValue)))
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue);
            else
               Controller.DeleteModuleSetting(ModuleId, setting);
         }
      }

      private void UpdateIntegerSetting(string setting, int? settingValue)
      {
         if (Settings[setting] == null)
         {
            if (settingValue != null)
            {
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            }
         }
         else
         {
            if (settingValue != null)
               Controller.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            else
               Controller.DeleteModuleSetting(ModuleId, setting);
         }
      }

   }
}