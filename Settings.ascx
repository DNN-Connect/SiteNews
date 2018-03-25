<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Connect.Modules.SiteNews.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<div class="dnnFormItem">
   <dnn:Label ID="NumberOfDaysLabel" runat="server"
      ControlName="NumberOfDaysRadioButtons"
      ResourceKey="NumberOfDays"
      Suffix=":" /> 
   <asp:RadioButtonList ID="NumberOfDaysRadioButtons" runat="server"
      RepeatColumns="5"
      RepeatDirection="Horizontal">
      <asp:ListItem Value="1" Text="1" />
      <asp:ListItem Value="5" Text="5" />
      <asp:ListItem Value="10" Text="10" />
      <asp:ListItem Value="25" Text="25" />
      <asp:ListItem Value="50" Text="50" />
   </asp:RadioButtonList>
</div>

<div class="dnnFormItem">
   <dnn:Label ID="SearchTypesLabel" runat="server"
      ControlName="SearchTypeCheckBoxes"
      ResourceKey="SearchTypes"
      Suffix=":" />
   <asp:CheckBoxList ID="SearchTypeCheckBoxes" runat="server"
      OnLoad="SearchTypeCheckBoxes_Load" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="NumberOfResultsLabel" runat="server"
      ControlName="NumberOfResultsRadioButtons"
      ResourceKey="NumberOfResults"
      Suffix=":" />
   <asp:RadioButtonList ID="NumberOfResultsRadioButtons" runat="server"
      RepeatColumns="4"
      RepeatDirection="Horizontal">
      <asp:ListItem Value="10" Text="10" />
      <asp:ListItem Value="25" Text="25" />
      <asp:ListItem Value="50" Text="50" />
      <asp:ListItem Value="100" Text="100" />
   </asp:RadioButtonList>
</div>

<div class="dnnFormItem">
   <dnn:Label ID="PageSizeLabel" runat="server"
      ControlName="PageSizeRadioButtons"
      ResourceKey="PageSize"
      Suffix=":" />
   <asp:RadioButtonList ID="PageSizeRadioButtons" runat="server"
      RepeatColumns="55"
      RepeatDirection="Horizontal">
      <asp:ListItem Value="1" Text="1" />
      <asp:ListItem Value="5" Text="5" />
      <asp:ListItem Value="10" Text="10" />
      <asp:ListItem Value="25" Text="25" />
      <asp:ListItem Value="50" Text="50" />
   </asp:RadioButtonList>
</div>

<h2><asp:Label ID="RenderingLabel" runat="server" ResourceKey="Rendering" /></h2>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeSearchTypeLabel" runat="server"
      ControlName="IncludeSearchTypeCheckBox"
      ResourceKey="IncludeSearchType"
      Suffix="?" />
   <asp:CheckBox ID="IncludeSearchTypeCheckBox" runat="server" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeInfoLabel" runat="server"
      ControlName="IncludeInfoCheckBox"
      ResourceKey="IncludeInfo"
      Suffix="?" />
   <asp:CheckBox ID="IncludeInfoCheckBox" runat="server" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeDescriptionLabel" runat="server"
      ControlName="IncludeDescriptionCheckBox"
      ResourceKey="IncludeDescription"
      Suffix="?" />
   <asp:CheckBox ID="IncludeDescriptionCheckBox" runat="server" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeBodyLabel" runat="server"
      ControlName="IncludeBodyCheckBox"
      ResourceKey="IncludeBody"
      Suffix="?" />
   <asp:CheckBox ID="IncludeBodyCheckBox" runat="server" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeAuthorLabel" runat="server"
      ControlName="IncludeAuthorCheckBox"
      ResourceKey="IncludeAuthor"
      Suffix="?" />
   <asp:CheckBox ID="IncludeAuthorCheckBox" runat="server" />
</div>

<div class="dnnFormItem">
   <dnn:Label ID="IncludeSnippetLabel" runat="server"
      ControlName="IncludeSnippetCheckBox"
      ResourceKey="IncludeSnippet"
      Suffix="?" />
   <asp:CheckBox ID="IncludeSnippetCheckBox" runat="server" />
</div>