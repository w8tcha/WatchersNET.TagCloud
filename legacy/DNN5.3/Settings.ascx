<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Settings.ascx.cs" Inherits="WatchersNET.DNN.Modules.TagCloud.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<asp:panel id="pnlSettings" runat="server">
  <img class="TagCloudLogo" style="margin: 0 auto;" src="<%= ResolveUrl("TagCloudLogo.png")%>" alt="WatchersNET.TagCloud Logo" title="WatchersNET.TagCloud Logo" />
  <div id="SettingTabs" style="width:800px;">
    <ul>
        <li><a href="#BasicSettings"><asp:Label ID="lTab1" runat="server"></asp:Label></a></li>
        <li><a href="#TagSourceSettings"><asp:Label ID="lTab3" runat="server"></asp:Label></a></li>
        <li><a href="#FlashSettings"><asp:Label ID="lTab2" runat="server"></asp:Label></a></li>
        <li><a href="#ExcludeSettings"><asp:Label ID="lTab4" runat="server"></asp:Label></a></li>
    </ul>
  <div id="BasicSettings">
  <dnn:sectionhead id="dshCommOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lCommOpt" section="tblCommOpt" />
  <table id="tblCommOpt" runat="server" style="margin-left:20px; width:100%;">
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:Label id="lblRenderMode" runat="server" ResourceKey="lblRenderMode" controlname="RenderModeType" suffix=":" CssClass="SubHead"></dnn:Label>
      </td>
      <td>
        <asp:DropDownList id="RenderModeType" Width="194px" runat="server" AutoPostBack="True"></asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:Label id="lblSkin" runat="server" ResourceKey="lblSkin" controlname="dDlSkins" suffix=":" CssClass="SubHead"></dnn:Label>
      </td>
      <td>
        <asp:DropDownList id="dDlSkins" Width="194px" runat="server"></asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td valign="top">
          <dnn:label id="lblTagsCloudWidth" runat="server" ResourceKey="lblTagsCloudWidth" controlname="tbTagsCloudWidth" suffix=":" CssClass="SubHead"></dnn:label>
          <dnn:label id="lblFlashWidth" runat="server" ResourceKey="lblFlashWidth" controlname="tbFlashWidth" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
          <asp:TextBox id="tbFlashWidth" Width="50px" runat="server" CssClass="NumericTextBox"></asp:TextBox>
          <asp:TextBox id="tbTagsCloudWidth" Width="50px" runat="server" CssClass="NumericTextBox"></asp:TextBox>&nbsp;
          <asp:DropDownList id="dDlWidth" runat="server">
            <asp:ListItem Text="px" Value="pixel"></asp:ListItem>
            <asp:ListItem Text="%" Value="percent"></asp:ListItem>
          </asp:DropDownList>&nbsp;
          <asp:CustomValidator id="cVWidth" runat="server" OnServerValidate="CheckWidth" ControlToValidate="tbTagsCloudWidth" ErrorMessage="CustomValidator"></asp:CustomValidator>
          <asp:CustomValidator id="cVFlashWidth" runat="server" OnServerValidate="CheckFlashWidth" ControlToValidate="tbFlashWidth" ErrorMessage="CustomValidator"></asp:CustomValidator>
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblTagsCloudHeight" runat="server" ResourceKey="lblTagsCloudHeight" controlname="tbTagsCloudHeight" suffix=":" CssClass="SubHead"></dnn:label>
          
        <dnn:label id="lblFlashHeight" runat="server" ResourceKey="lblFlashHeight" controlname="tbFlashHeight" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:TextBox id="tbTagsCloudHeight" Width="50px" Text="auto" Enabled="false" runat="server"></asp:TextBox>
          
        <asp:TextBox id="tbFlashHeight" Width="50px" runat="server" CssClass="NumericTextBox"></asp:TextBox>
        <asp:CustomValidator id="cVFlashHeight" runat="server" OnServerValidate="CheckFlashHeight" ControlToValidate="tbFlashHeight" ErrorMessage="CustomValidator"></asp:CustomValidator>
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:Label id="lblTagSeparator" runat="server" ResourceKey="lblTagSeparator" controlname="txtTagSeparator" suffix=":" CssClass="SubHead"></dnn:Label>
      </td>
      <td>
          <asp:TextBox id="txtTagSeparator" runat="server" Width="194px" />
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblRenderUl" runat="server" ResourceKey="lblRenderUl" controlname="cbRenderUl" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:CheckBox id="cbRenderUl" Width="50px" runat="server"></asp:CheckBox>
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblCacheItems" runat="server" ResourceKey="lblCacheItems" controlname="cbCacheItems" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:CheckBox id="cbCacheItems" Width="50px" runat="server"></asp:CheckBox>
      </td>
    </tr>
    <tr>
        <td style="vertical-align: top">
            <dnn:label id="CustomSearchPageLabel" runat="server" ResourceKey="CustomSearchPageLabel" controlname="CustomSearchPage" suffix=":" CssClass="SubHead"></dnn:label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="CustomSearchPage"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top">
            <dnn:label id="SearchPageQueryStringLabel" runat="server" ResourceKey="SearchPageQueryStringLabel" controlname="SearchPageQueryString" suffix=":" CssClass="SubHead"></dnn:label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="SearchPageQueryString"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top">
            <dnn:label id="SearchPageTaxQueryStringLabel" runat="server" ResourceKey="SearchPageTaxQueryStringLabel" controlname="SearchPageTaxQueryString" suffix=":" CssClass="SubHead"></dnn:label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="SearchPageTaxQueryString"></asp:TextBox>
        </td>
    </tr>
  </table>
  <dnn:sectionhead id="dshFilterOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lFilterOpt" section="tblFilterOpt" />
  <table id="tblFilterOpt" runat="server" style="margin-left:20px; width:100%;">
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblOccurCount" runat="server" ResourceKey="lblOccurCount" controlname="tbOccurCount" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:TextBox id="tbOccurCount" Width="50px" runat="server" CssClass="NumericTextBox" />
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblTagsCount" runat="server" ResourceKey="lblTagsCount" controlname="tbTags" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:TextBox id="tbTags" Width="50px" runat="server" CssClass="NumericTextBox" />
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:Label id="lblSortTags" runat="server" ResourceKey="lblSortTags" controlname="SortTags" suffix=":" CssClass="SubHead"></dnn:Label>
      </td>
      <td>
          <asp:DropDownList id="SortTags" Width="194px" runat="server"></asp:DropDownList>
      </td>
    </tr>
     <tr>
      <td valign="top">
        <dnn:label id="lblRenderItemWeight" runat="server" ResourceKey="lblRenderItemWeight" controlname="RenderItemWeight" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:CheckBox id="RenderItemWeight" Width="50px" runat="server"></asp:CheckBox>
      </td>
    </tr>
  </table>
  <dnn:sectionhead id="dshLinkOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lLinkOpt" section="tblLinkOpt" />
  <table id="tblLinkOpt" runat="server" style="margin-left:20px; width:100%;">
     <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblTagsLink" runat="server" ResourceKey="lblTagsLink" controlname="cbTagsLink" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:CheckBox id="cbTagsLink" Width="50px" runat="server"></asp:CheckBox>
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblTagsLinkChk" runat="server" ResourceKey="lblTagsLinkChk" controlname="cbTagsLinkChk" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:CheckBox id="cbTagsLinkChk" Width="50px" runat="server"></asp:CheckBox>
      </td>
    </tr>
  </table>
  </div>
  <div id="FlashSettings">
  <dnn:sectionhead id="dshFlashOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lFlashOpt" section="tblFlashOpt" />
  <table id="tblFlashOpt" runat="server" style="margin-left:20px; width:100%;">
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblFontFamily" runat="server" ResourceKey="lblFontFamily" controlname="FontFamily" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
       <asp:TextBox id="FontFamily" runat="server" Width="300px" />
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblTcolor" runat="server" ResourceKey="lblTcolor" controlname="tbTcolor" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        #<asp:TextBox id="tbTcolor" runat="server" Width="100px" />
        
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblTcolor2" runat="server" ResourceKey="lblTcolor2" controlname="tbTcolor2" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        #<asp:TextBox id="tbTcolor2" runat="server" Width="100px" />
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblHicolor" runat="server" ResourceKey="lblHicolor" controlname="tbHicolor" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        #<asp:TextBox id="tbHicolor" runat="server" Width="100px" />
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblBgcolor" runat="server" ResourceKey="lblBgcolor" controlname="tbBgcolor" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        #<asp:TextBox id="tbBgcolor" runat="server" Width="100px" />
      </td>
    </tr>
    <tr>
      <td valign="top">
      </td>
      <td>
        <asp:CheckBox id="cbTransparent" runat="server" TextAlign="Right" Text="&nbsp;Transparent?" AutoPostBack="true" />
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblTspeed" runat="server" ResourceKey="lblTspeed" controlname="tbTspeed" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td>
        <asp:TextBox id="tbTspeed" runat="server" Width="50px" CssClass="NumericTextBox" />
        <asp:RangeValidator  id="rangevalidator1" runat="server" MinimumValue="25" MaximumValue="500" Type="Integer" ControlToValidate="tbTspeed" ErrorMessage="RangeValidator"></asp:RangeValidator>
      </td>
    </tr>
    <tr>
      <td style="width:300px;" valign="top">
        <dnn:Label id="lblWeightMode" runat="server" ResourceKey="lblWeightMode" controlname="CanvasWeightMode" suffix=":" CssClass="SubHead"></dnn:Label>
      </td>
      <td>
        <asp:DropDownList id="CanvasWeightMode" Width="194px" runat="server"></asp:DropDownList>
      </td>
    </tr>
      </table>
      <dnn:sectionhead id="WordCloudOptionsPanel" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="WordCloudOptionsPanelLabel" section="WordlCloudOptionsTable" />
      <table id="WordlCloudOptionsTable" runat="server" style="margin-left:20px; width:100%;">
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="ShapeLabel" runat="server" ResourceKey="ShapeLabel" controlname="Shape" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:DropDownList id="Shape" runat="server" Width="300px" />
                </td>
            </tr>
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="GridSizeLabel" runat="server" ResourceKey="GridSizeLabel" controlname="GridSize" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:TextBox id="GridSize" runat="server" Width="50px" CssClass="NumericTextBox" />
                </td>
            </tr>
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="EllipticityLabel" runat="server" ResourceKey="EllipticityLabel" controlname="Ellipticity" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:TextBox id="Ellipticity" runat="server" Width="50px" CssClass="NumericTextBox" />
                </td>
            </tr>
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="WeightFactorLabel" runat="server" ResourceKey="WeightFactorLabel" controlname="WeightFactor" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:TextBox id="WeightFactor" runat="server" Width="50px" CssClass="NumericTextBox" />
                </td>
            </tr>
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="MinSizeLabel" runat="server" ResourceKey="MinSizeLabel" controlname="MinSize" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:TextBox id="MinSize" runat="server" Width="50px" CssClass="NumericTextBox" />
                </td>
            </tr>
            <tr>
                <td style="width:300px;vertical-align: top">
                    <dnn:label id="FillBoxLabel" runat="server" ResourceKey="FillBoxLabel" controlname="FillBox" suffix=":" CssClass="SubHead"></dnn:label>
                </td>
                <td>
                    <asp:CheckBox id="FillBox" runat="server" Width="50px" />
                </td>
            </tr>
        </table>
    </div>
    <div id="TagSourceSettings">
      <dnn:sectionhead id="dshTagSrcOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcOpt" section="tblTagSrcOpt" />
      <table id="tblTagSrcOpt" runat="server" style="margin-left:20px; width:100%;">
        <tr>
          <td style="width:150px;" valign="top">
            <dnn:label id="lblTagMode" runat="server" ResourceKey="lblTagMode" controlname="rBlTagMode" suffix=":" CssClass="SubHead"></dnn:label>
          </td>
          <td>
            <asp:CheckBoxList id="TagModes" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
              <asp:ListItem Text="Search Referrals" Value="ModeReferrals"></asp:ListItem>
              <asp:ListItem Text="Common Words" Value="ModeSearch"></asp:ListItem>
              <asp:ListItem Text="Custom Tags" Value="ModeCustom"></asp:ListItem>
              <asp:ListItem Text="Taxonomy" Value="ModeTax"></asp:ListItem>
            </asp:CheckBoxList>
          </td>
        </tr>
        </table>
        <asp:PlaceHolder id="phCustom" runat="server" Visible="False">
         <dnn:sectionhead id="secCustom" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcCustom" section="tabCustom" />
      <table id="tabCustom" runat="server" style="margin-left:20px; width:100%;">
        <tr>
           <td style="vertical-align:top">
             <dnn:label id="lblCustomTags" runat="server" ResourceKey="lblCustomTags" controlname="tbCustomTag" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:UpdatePanel ID="upGrid" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
              <table style="width:100%;">
                <tr>
                  <td colspan="2">
       <asp:DataGrid id="grdTagList" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Gray" BorderWidth="1px">
		<Columns>
			<asp:BoundColumn Visible="False" DataField="TagID" SortExpression="TagID" ReadOnly="True" HeaderText="TagID"></asp:BoundColumn>
			<asp:BoundColumn DataField="TagID" SortExpression="TagID" ReadOnly="True" HeaderText="TagID">
				<HeaderStyle HorizontalAlign="left" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:TemplateColumn>
				<HeaderStyle HorizontalAlign="Center" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle HorizontalAlign="left"></ItemStyle>
				<ItemTemplate>
					<asp:ImageButton id="btnEdit" AlternateText="Item bearbeiten" ImageUrl="~/images/edit.gif" CssClass="CommandButton"
						runat="server"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<HeaderStyle HorizontalAlign="Center" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle HorizontalAlign="left"></ItemStyle>
				<ItemTemplate>
					<asp:ImageButton id="btnDelete" AlternateText="Item löschen" ImageUrl="~/images/delete.gif" CssClass="CommandButton"
						runat="server"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="Tag" SortExpression="Tag" ReadOnly="True" HeaderText="Tag">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="TagUrl" SortExpression="TagUrl" ReadOnly="True" HeaderText="Tag URL">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Weight" SortExpression="Weight" ReadOnly="True" HeaderText="Weight">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
      </td>
    </tr>
    <tr>
      <td valign="top">
        <dnn:label id="lblCustomTag" runat="server" ResourceKey="lblCustomTag" controlname="tbCustomTag" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:Label id="lTagId" runat="server" Visible="false"></asp:Label>
         <asp:Label id="lTagLocale" runat="server" Visible="false"></asp:Label>
      </td>
      <td>
        <table id="tCustomTag" style="width:100%;" runat="server">
          <tr>
            <td valign="top"><asp:Label ID="lCustomTag" runat="server" Width="100px" Text="Tag Name:"></asp:Label></td>
            <td valign="top"><asp:TextBox id="tbCustomTag" runat="server" Width="194px" /></td>
          </tr>
          <tr>
            <td valign="top"><asp:Label ID="lTagWeight" runat="server" Text="Tag Weight:"></asp:Label></td>
            <td valign="top"><asp:DropDownList id="dDlTagWeight" runat="server"></asp:DropDownList></td>
          </tr>
          <tr>
            <td valign="top"><asp:Label ID="lTagUrl" runat="server" Text="Tag URL:"></asp:Label></td>
            <td valign="top">
              <dnn:url id="ctlTagUrl" runat="server" width="300" showtabs="True" showfiles="False" showUrls="True"
					urltype="F" showlog="False" shownewwindow="False" showtrack="False"></dnn:url>
		    </td>
          </tr>
          <tr>
            <td></td>
            <td valign="top">
              <asp:LinkButton id="iBTagAdd" runat="server" CssClass="CustomButton" />
              &nbsp;<asp:LinkButton id="iBTagSave" runat="server" Visible="false" CssClass="CustomButton" />
              &nbsp;<asp:LinkButton id="iBTagLocalize" runat="server" Visible="false" CssClass="CustomButton" />
              &nbsp;<asp:LinkButton id="iBTagEditCancel" runat="server" Visible="false" CssClass="CustomButton" />
            </td>
          </tr>
        </table>
        <asp:DataGrid id="grdTagLocales" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Gray" BorderWidth="1px" style="margin-top:10px;">
		<Columns>
			<asp:BoundColumn Visible="False" DataField="TagID" SortExpression="TagID" ReadOnly="True" HeaderText="TagID"></asp:BoundColumn>
			<asp:BoundColumn DataField="TagID" SortExpression="TagID" ReadOnly="True" HeaderText="TagID">
				<HeaderStyle HorizontalAlign="left" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:TemplateColumn>
				<HeaderStyle HorizontalAlign="Center" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle HorizontalAlign="left"></ItemStyle>
				<ItemTemplate>
					<asp:ImageButton id="btnEdit" AlternateText="Item bearbeiten" ImageUrl="~/images/edit.gif" CssClass="CommandButton"
						runat="server"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<HeaderStyle HorizontalAlign="Center" Width="1%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle HorizontalAlign="left"></ItemStyle>
				<ItemTemplate>
					<asp:ImageButton id="btnDelete" AlternateText="Item löschen" ImageUrl="~/images/delete.gif" CssClass="CommandButton"
						runat="server"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:BoundColumn DataField="Locale" SortExpression="Locale" ReadOnly="True" HeaderText="Locale">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Tag" SortExpression="Tag" ReadOnly="True" HeaderText="Tag">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="TagUrl" SortExpression="TagUrl" ReadOnly="True" HeaderText="Tag URL">
				<HeaderStyle HorizontalAlign="left" Width="10%" CssClass="SubHead"></HeaderStyle>
				<ItemStyle CssClass="Normal"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
      </td>
    </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel> 
           </td>
         </tr>
         <tr>
           <td>
           </td>
           <td style="height:40px">
       <a onclick="showDialog('ImportDialog');" id="lnkImport" href="#"><asp:Label id="lblImport" runat="server" Text="Import Tags" CssClass="CustomButton"></asp:Label></a>
        &nbsp;
        <a onclick="showDialog('ExportDialog');" id="lnkExport" href="#"><asp:Label id="lblExport" runat="server" Text="Export Tags" CssClass="CustomButton"></asp:Label></a>
        <div id="ImportDialog" title="Import Custom Tags" style="display:none">
         <asp:UpdatePanel ID="upImport" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
               <dnn:url id="ctlImportFile" runat="server" width="300" showtabs="False" Required="False" filefilter="xml" showfiles="True" showUrls="False"
					urltype="F" showlog="False" shownewwindow="False" showtrack="False"></dnn:url>
              <asp:LinkButton id="cmdImport" runat="server" Visible="false" CssClass="CustomButton"></asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <div id="ExportDialog" title="Export Custom Tags" style="display:none">
         <asp:UpdatePanel ID="upExport" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
              <p><asp:DropDownList id="cboFolders" Runat="server" CssClass="NormalTextBox" Width="300"></asp:DropDownList></p>
              <p><asp:TextBox id="txtExportName" runat="server" Text="TagCloudCustomItems.xml" Width="300"></asp:TextBox></p>
              <asp:LinkButton id="cmdExport" runat="server" Visible="false"></asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </td>
         </tr>
    </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phTax" runat="server" Visible="False">
        <dnn:sectionhead id="secTax" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcTax" section="tabTax" />
      <table id="tabTax" runat="server" style="margin-left:20px; width:100%;">
         <tr>
           <td>
             <dnn:label id="lblTaxMode" runat="server" ResourceKey="lblTaxMode" controlname="dDlTaxMode" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:DropDownList id="dDlTaxMode" runat="server" AutoPostBack="true">
             </asp:DropDownList>
           </td>
         </tr>
         <tr>
           <td>
              <dnn:label id="lblChooseVoc" runat="server" ResourceKey="lblChooseVoc" controlname="cBlVocabularies" suffix=":" CssClass="SubHead" Visible="false"></dnn:label>
           </td>
           <td>
             <asp:CheckBoxList ID="cBlVocabularies" runat="server" Visible="false"></asp:CheckBoxList>
           </td>
         </tr>
         </table>
         </asp:PlaceHolder>
         <asp:PlaceHolder id="phReferrals" runat="server" Visible="False">
        <dnn:sectionhead id="secReferrals" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcReferrals" section="tabReferrals" />
      <table id="tabReferrals" runat="server" style="margin-left:20px; width:100%;">
         <tr>
           <td>
           </td>
           <td>
             <asp:CheckBoxList ID="cBlSearches" runat="server"></asp:CheckBoxList>
           </td>
         </tr>
         <tr>
           <td>
             <dnn:label id="lblStartDate" runat="server" ResourceKey="lblStartDate" controlname="txtStartDate" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
               <asp:TextBox id="txtStartDate" runat="server" />
           </td>
          </tr>
          <tr>
             <td>
               <dnn:label id="lblEndDate" runat="server" ResourceKey="lblEndDate" controlname="txtEndDate" suffix=":" CssClass="SubHead"></dnn:label>
            </td>
            <td>
               <asp:TextBox id="txtEndDate" Enabled="false" runat="server" />
             </td>
         </tr>
         </table>
         </asp:PlaceHolder>
         <asp:PlaceHolder id="phDnnBlog" runat="server" Visible="False">
         <dnn:sectionhead id="secDnnBlog" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcDnnBlog" section="tabDnnBlog" />
      <table id="tabDnnBlog" runat="server" style="margin-left:20px; width:100%;">
         <tr>
           <td>
             <dnn:label id="lblModuleDnnBlog" ResourceKey="lblModuleDnnBlog" runat="server" controlname="ddLModulesVentrian" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:DropDownList id="ddLTabsDnnBlog" Width="325" runat="server"
                DataValueField="ModuleID" DataTextField="ModuleTitle">
             </asp:DropDownList>
           </td>
         </tr>
         <tr>
           <td>
             <dnn:label id="lblSelectBlogs" ResourceKey="lblSelectBlogs" runat="server" controlname="checkAllBlogs" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:CheckBox id="checkAllBlogs" ResourceKey="checkAllBlogs" runat="server" Checked="true" />
             <asp:CheckBoxList ID="checListBlogs" runat="server"
              DataValueField="BlogID" DataTextField="Title">
             </asp:CheckBoxList>
           </td>
         </tr>
         </table>
          </asp:PlaceHolder>
         <asp:PlaceHolder id="phVentrianNews" runat="server" Visible="False">
         <dnn:sectionhead id="secVentrianNews" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcVentrianNews" section="tabVentrianNews" />
      <table id="tabVentrianNews" runat="server" style="margin-left:20px; width:100%;">
         <tr>
           <td>
             <dnn:label id="lblModuleVentrianNews" ResourceKey="lblModuleVentrianNews" runat="server" controlname="ddLTabsVentrianNews" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:DropDownList id="ddLTabsVentrianNews" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
           </td>
         </tr>
         </table>
         </asp:PlaceHolder>
         <asp:PlaceHolder id="phVentrianSimple" runat="server" Visible="False">
         <dnn:sectionhead id="secVentrianSimple" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcVentrianSimple" section="tabVentrianSimple" />
      <table id="tabVentrianSimple" runat="server" style="margin-left:20px; width:100%;">
         <tr>
           <td>
             <dnn:label id="lblModuleVentrianSimple" ResourceKey="lblModuleVentrianSimple" runat="server" controlname="ddLTabsVentrianSimple" suffix=":" CssClass="SubHead"></dnn:label>
           </td>
           <td>
             <asp:DropDownList id="ddLTabsVentrianSimple" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
           </td>
         </tr>
         </table>
         </asp:PlaceHolder>
        <asp:PlaceHolder id="phActiveforums" runat="server" Visible="False">
        <dnn:sectionhead id="secActiveforums" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lTagSrcActiveforums" section="tabActiveforums" />
      <table id="tabActiveforums" runat="server" style="margin-left:20px; width:100%;">
        <tr>
          <td>
            <dnn:label id="lblActiveforums" ResourceKey="lblModuleActiveforums" runat="server" controlname="ddLTabsActiveforums" suffix=":" CssClass="SubHead"></dnn:label>
          </td>
          <td>
             <asp:DropDownList id="ddLTabsActiveforums" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
          </td>
        </tr>
        </table>
        </asp:PlaceHolder>

    </div>
    <div id="ExcludeSettings">
  <dnn:sectionhead id="dshExcludeOpt" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lExcludeOpt" section="tblExcludeOpt" />
  <table id="tblExcludeOpt" runat="server" style="margin-left:20px; width:100%;">
       <tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblExcludeCommon" runat="server" ResourceKey="lblExcludeCommon" controlname="ExcludeCommon" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td valign="top">
        <asp:CheckBox id="ExcludeCommon" runat="server"></asp:CheckBox>
      </td>
    </tr><tr>
      <td style="width:300px;" valign="top">
        <dnn:label id="lblExlusionList" runat="server" ResourceKey="lblExlusionList" controlname="tbExlusLst" suffix=":" CssClass="SubHead"></dnn:label>
      </td>
      <td valign="top">
        <asp:ListBox id="lBExList" runat="server" Width="60%"></asp:ListBox>
      </td>
    </tr>
    <tr>
      <td></td>
      <td>
        <asp:LinkButton id="iBEdit" runat="server" ImageUrl="~/images/edit.gif" CssClass="CustomButton" />
        <asp:LinkButton id="iBDelete" runat="server" Text="Delete" CssClass="CustomButton" />
      </td>
    </tr>
  </table>
  <dnn:sectionhead id="dshModifyExcludeWord" runat="server" cssclass="Head" includerule="True" isExpanded="True" resourcekey="lModifyWord" section="tblModifyExcludeWord" />
    <table id="tblModifyExcludeWord" runat="server" style="margin-left:20px; width:100%;">
      <tr>
        <td style="width:300px;" valign="top">
         <dnn:label id="lblExcludeWord" runat="server" ResourceKey="lblExcludeWord" controlname="tbExlusLst" suffix=":" CssClass="SubHead"></dnn:label>
        </td>
        <td>
         <asp:TextBox id="tbExlusLst" runat="server" Width="194px"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td>
         <dnn:label id="lblExlusionType" runat="server" ResourceKey="lblExlusionType" controlname="ExclusionType" suffix=":" CssClass="SubHead"></dnn:label>
        </td>
        <td>
         <asp:DropDownList id="ExclusionType" runat="server" Width="194px"></asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td></td>
        <td>
          <asp:LinkButton id="iBAdd" runat="server" Text="Add" CommandArgument="add" CssClass="CustomButton" />&nbsp;
          <asp:LinkButton id="iBCancel" runat="server" Text="Cancel" CssClass="CustomButton" Visible="false" />
        </td>
      </tr>
     </table>
  </div>
  </div>
</asp:panel>
