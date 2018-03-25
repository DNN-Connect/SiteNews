<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Connect.Modules.SiteNews.View" %>

<asp:Panel ID="SiteSearchPanel" runat="server" CssClass="dnnForm">
   <asp:Panel ID="NoNewsFoundMessagePanel" runat="server"
      CssClasslass="dnnFormMessage dnnFormWarning"
      EnableViewState="false"
      Visible="false">
      <asp:Label ID="NoNewsFoundMessage" runat="server"
         ResourceKey="NoNewsFound" />
   </asp:Panel>
   <asp:ListView ID="ResultsList" runat="server"
      OnItemDataBound="ResultsList_ItemDataBound"
      ItemPlaceHolderID="ResultPlaceHolder">
      <LayoutTemplate>
         <asp:Panel ID="InfoPanel" runat="server"
            CssClass="dnnFormMessage dnnInfo">
            <asp:Label ID="FoundItems" runat="server" />
         </asp:Panel>
         <asp:PlaceHolder ID="ResultPlaceHolder" runat="server" />
      </LayoutTemplate>
      <ItemTemplate>
         <h2><asp:Label ID="HeaderLabel" runat="server" Text='<%# Eval("Title") %>' /></h2>
         <p>
            <asp:Label ID="TypeLabel" runat="server" Text='<%# string.Format("{0}: {1} (Tab: {2}, Module: {3})<br />", LocalizeString("TypeLabel"), GetSearchTypeName(Eval("SearchTypeId")), Eval("TabId"), Eval("ModuleId")) %>' />
            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# string.Format("{0}<br />", Eval("Description") )%>' />
            <asp:Label ID="BodyLabel" runat="server" Text='<%# string.Format("{0}<br />", Eval("Body")) %>' />
            <asp:Label ID="AuthorNameLabel" runat="server" Text='<%# string.Format("{0}<br />", Eval("AuthorName")) %>' />
            <asp:Label ID="DisplayModifiedTimeLabel" runat="server" Text='<%# Eval("DisplayModifiedTime") %>' /><br />
            <asp:Label ID="SnippetLabel" runat="server" Text='<%# string.Format("{0}<br />", Eval("Snippet")) %>' />
            <asp:HyperLink ID="ResultUrl" runat="server" Text='<%# string.Format("{0}<br />", Eval("Url")) %>' NavigateUrl='<%# Eval("Url") %>' />
         </p>
         <hr />
      </ItemTemplate>
   </asp:ListView>
   <asp:DataPager ID="ResultsPager" runat="server"
      OnLoad="ResultsPager_Load"
      PagedControlID="ResultsList">
      <Fields>
         <asp:NextPreviousPagerField FirstPageText="&lt;&lt;" ShowFirstPageButton="True" 
            ShowNextPageButton="False" ShowPreviousPageButton="False" />
         <asp:NumericPagerField />
         <asp:NextPreviousPagerField LastPageText="&gt;&gt;" ShowLastPageButton="True" 
            ShowNextPageButton="False" ShowPreviousPageButton="False" />
      </Fields>
   </asp:DataPager>
</asp:Panel>