<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Settings.ascx.cs" Inherits="WatchersNET.DNN.Modules.TagCloud.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<asp:panel id="pnlSettings" runat="server">
  <div class="dnnForm " id="SettingTabs">
    <ul class="dnnAdminTabNav">
        <li><a href="#BasicSettings"><asp:Label ID="lTab1" runat="server"></asp:Label></a></li>
        <li><a href="#TagSourceSettings"><asp:Label ID="lTab3" runat="server"></asp:Label></a></li>
        <li><a href="#FlashSettings"><asp:Label ID="lTab2" runat="server"></asp:Label></a></li>
        <li><a href="#ExcludeSettings"><asp:Label ID="lTab4" runat="server"></asp:Label></a></li>
    </ul>
  <div id="BasicSettings">
  <div class="dnnFormExpandContent"><a href="">Expand All</a></div>
  <h2 id="CommomOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="CommomOptionsPanelLink" resourcekey="lCommOpt"></asp:Label></a></h2>
  <fieldset class="dnnClear">
      <div class="dnnFormItem">
          <dnn:Label id="lblRenderMode" runat="server" ResourceKey="lblRenderMode" controlname="RenderModeType" suffix=":" CssClass="SubHead"></dnn:Label>
          <asp:DropDownList id="RenderModeType" Width="194px" runat="server" AutoPostBack="True"></asp:DropDownList>
      </div>
      <div class="dnnFormItem">
          <dnn:Label id="lblSkin" runat="server" ResourceKey="lblSkin" controlname="dDlSkins" suffix=":" CssClass="SubHead"></dnn:Label>
          <asp:DropDownList id="dDlSkins" Width="194px" runat="server"></asp:DropDownList>
      </div>
      <div class="dnnFormItem">
          <dnn:label id="lblTagsCloudWidth" runat="server" ResourceKey="lblTagsCloudWidth" controlname="tbTagsCloudWidth" suffix=":" CssClass="SubHead"></dnn:label>
          <dnn:label id="lblFlashWidth" runat="server" ResourceKey="lblFlashWidth" controlname="tbFlashWidth" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:TextBox id="tbFlashWidth" Width="50px" runat="server" TextMode="Number"></asp:TextBox>
          <asp:TextBox id="tbTagsCloudWidth" Width="50px" runat="server" TextMode="Number"></asp:TextBox>&nbsp;
          <asp:DropDownList id="dDlWidth" runat="server">
            <asp:ListItem Text="px" Value="pixel"></asp:ListItem>
            <asp:ListItem Text="%" Value="percent"></asp:ListItem>
          </asp:DropDownList>&nbsp;
          <asp:CustomValidator id="cVWidth" runat="server" OnServerValidate="CheckWidth" ControlToValidate="tbTagsCloudWidth" ErrorMessage="CustomValidator"></asp:CustomValidator>
          <asp:CustomValidator id="cVFlashWidth" runat="server" OnServerValidate="CheckFlashWidth" ControlToValidate="tbFlashWidth" ErrorMessage="CustomValidator"></asp:CustomValidator>
      </div>
      <div class="dnnFormItem">
          <dnn:label id="lblTagsCloudHeight" runat="server" ResourceKey="lblTagsCloudHeight" controlname="tbTagsCloudHeight" suffix=":" CssClass="SubHead"></dnn:label>

          <dnn:label id="lblFlashHeight" runat="server" ResourceKey="lblFlashHeight" controlname="tbFlashHeight" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:TextBox id="tbTagsCloudHeight" Width="50px" Text="auto" Enabled="false" runat="server"></asp:TextBox>

          <asp:TextBox id="tbFlashHeight" Width="50px" runat="server" TextMode="Number"></asp:TextBox>
          <asp:CustomValidator id="cVFlashHeight" runat="server" OnServerValidate="CheckFlashHeight" ControlToValidate="tbFlashHeight" ErrorMessage="CustomValidator"></asp:CustomValidator>
      </div>
      <div class="dnnFormItem">
          <dnn:Label id="lblTagSeparator" runat="server" ResourceKey="lblTagSeparator" controlname="txtTagSeparator" suffix=":" CssClass="SubHead"></dnn:Label>
          <asp:TextBox id="txtTagSeparator" runat="server" Width="194px" />
      </div>
      <div class="dnnFormItem">
          <dnn:label id="lblRenderUl" runat="server" ResourceKey="lblRenderUl" controlname="cbRenderUl" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:CheckBox id="cbRenderUl" Width="50px" runat="server"></asp:CheckBox>
      </div>
      <div class="dnnFormItem">
           <dnn:label id="lblCacheItems" runat="server" ResourceKey="lblCacheItems" controlname="cbCacheItems" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="cbCacheItems" Width="50px" runat="server"></asp:CheckBox>
      </div>
      <div class="dnnFormItem">
          <dnn:label id="CustomSearchPageLabel" runat="server" ResourceKey="CustomSearchPageLabel" controlname="CustomSearchPage" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:DropDownList runat="server" ID="CustomSearchPage"></asp:DropDownList>
      </div>
      <div class="dnnFormItem">
          <dnn:label id="SearchPageQueryStringLabel" runat="server" ResourceKey="SearchPageQueryStringLabel" controlname="SearchPageQueryString" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:TextBox runat="server" ID="SearchPageQueryString"></asp:TextBox>
      </div>
      <div class="dnnFormItem">
          <dnn:label id="SearchPageTaxQueryStringLabel" runat="server" ResourceKey="SearchPageTaxQueryStringLabel" controlname="SearchPageTaxQueryString" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:TextBox runat="server" ID="SearchPageTaxQueryString"></asp:TextBox>
      </div>
  </fieldset>
  <h2 id="FilterOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="FilterOptionsPanelLink" resourcekey="lFilterOpt"></asp:Label></a></h2>
  <fieldset class="dnnClear">
      <div class="dnnFormItem">
          <dnn:label id="lblOccurCount" runat="server" ResourceKey="lblOccurCount" controlname="tbOccurCount" suffix=":" CssClass="SubHead"></dnn:label>
          <asp:TextBox id="tbOccurCount" Width="50px" runat="server" TextMode="Number" />
      </div>
      <div class="dnnFormItem">
    <dnn:label id="lblTagsCount" runat="server" ResourceKey="lblTagsCount" controlname="tbTags" suffix=":" CssClass="SubHead"></dnn:label>
      <asp:TextBox id="tbTags" Width="50px" runat="server" TextMode="Number" />
     </div>
    <div class="dnnFormItem">
        <dnn:Label id="lblSortTags" runat="server" ResourceKey="lblSortTags" controlname="SortTags" suffix=":" CssClass="SubHead"></dnn:Label>
          <asp:DropDownList id="SortTags" Width="194px" runat="server"></asp:DropDownList>
    </div>
     <div class="dnnFormItem">
        <dnn:label id="lblRenderItemWeight" runat="server" ResourceKey="lblRenderItemWeight" controlname="RenderItemWeight" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="RenderItemWeight" Width="50px" runat="server"></asp:CheckBox>
     </div>
  </fieldset>
  <h2 id="LinkOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="LinkOptionsPanelLink" resourcekey="lLinkOpt"></asp:Label></a></h2>
  <fieldset class="dnnClear">
     <div class="dnnFormItem">
        <dnn:label id="lblTagsLink" runat="server" ResourceKey="lblTagsLink" controlname="cbTagsLink" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="cbTagsLink" Width="50px" runat="server"></asp:CheckBox>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTagsLinkChk" runat="server" ResourceKey="lblTagsLinkChk" controlname="cbTagsLinkChk" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="cbTagsLinkChk" Width="50px" runat="server"></asp:CheckBox>
    </div>
  </fieldset>
  </div>
  <div id="FlashSettings">
      <div class="dnnFormExpandContent"><a href="">Expand All</a></div>
  <h2 id="FlashOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="FlashOptionsPanelLink" resourcekey="lFlashOpt"></asp:Label></a></h2>
  <fieldset class="dnnClear">
      <div class="dnnFormItem">
        <dnn:label id="lblFontFamily" runat="server" ResourceKey="lblFontFamily" controlname="FontFamily" suffix=":" CssClass="SubHead"></dnn:label>
       <asp:TextBox id="FontFamily" runat="server" Width="300px" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTcolor" runat="server" ResourceKey="lblTcolor" controlname="tbTcolor" suffix=":" CssClass="SubHead"></dnn:label>
        #<asp:TextBox id="tbTcolor" runat="server" Width="100px" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTcolor2" runat="server" ResourceKey="lblTcolor2" controlname="tbTcolor2" suffix=":" CssClass="SubHead"></dnn:label>
        #<asp:TextBox id="tbTcolor2" runat="server" Width="100px" />
   </div>
   <div class="dnnFormItem">
        <dnn:label id="lblHicolor" runat="server" ResourceKey="lblHicolor" controlname="tbHicolor" suffix=":" CssClass="SubHead"></dnn:label>
        #<asp:TextBox id="tbHicolor" runat="server" Width="100px" />
   </div>
   <div class="dnnFormItem">
        <dnn:label id="lblBgcolor" runat="server" ResourceKey="lblBgcolor" controlname="tbBgcolor" suffix=":" CssClass="SubHead"></dnn:label>
        #<asp:TextBox id="tbBgcolor" runat="server" Width="100px" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="Label1" runat="server" ResourceKey="cbTransparent" controlname="cbTransparent" suffix="?" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="cbTransparent" runat="server" TextAlign="Right" Text="&nbsp;Transparent?" AutoPostBack="true" />
   </div>
   <div class="dnnFormItem">
        <dnn:label id="lblTspeed" runat="server" ResourceKey="lblTspeed" controlname="tbTspeed" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:TextBox id="tbTspeed" runat="server" Width="50px" TextMode="Number" />
        <asp:RangeValidator  id="rangevalidator1" runat="server" MinimumValue="25" MaximumValue="500" Type="Integer" ControlToValidate="tbTspeed" ErrorMessage="RangeValidator"></asp:RangeValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:Label id="lblWeightMode" runat="server" ResourceKey="lblWeightMode" controlname="CanvasWeightMode" suffix=":" CssClass="SubHead"></dnn:Label>
        <asp:DropDownList id="CanvasWeightMode" Width="194px" runat="server"></asp:DropDownList>
     </div>
      <div class="dnnFormItem">
          <dnn:Label id="lblWeightSize" runat="server" ResourceKey="lblWeightSize" controlname="CanvasWeightSize" suffix=":" CssClass="SubHead"></dnn:Label>
          <asp:TextBox id="CanvasWeightSize" runat="server"></asp:TextBox>
      </div>
      </fieldset>
      <h2 id="WordCloudOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="WordCloudOptionsPanelLabel" resourcekey="WordCloudOptionsPanelLabel"></asp:Label></a></h2>
      <fieldset class="dnnClear">
          <div class="dnnFormItem">
                    <dnn:label id="ShapeLabel" runat="server" ResourceKey="ShapeLabel" controlname="Shape" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:DropDownList id="Shape" runat="server" Width="300px" />
          </div>
          <div class="dnnFormItem">
                    <dnn:label id="GridSizeLabel" runat="server" ResourceKey="GridSizeLabel" controlname="GridSize" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:TextBox id="GridSize" runat="server" Width="50px" TextMode="Number" />
          </div>
          <div class="dnnFormItem">
                    <dnn:label id="EllipticityLabel" runat="server" ResourceKey="EllipticityLabel" controlname="Ellipticity" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:TextBox id="Ellipticity" runat="server" Width="50px" TextMode="Number" />
           </div>
           <div class="dnnFormItem">
                    <dnn:label id="WeightFactorLabel" runat="server" ResourceKey="WeightFactorLabel" controlname="WeightFactor" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:TextBox id="WeightFactor" runat="server" Width="50px" TextMode="Number" />
            </div>
            <div class="dnnFormItem">
                    <dnn:label id="MinSizeLabel" runat="server" ResourceKey="MinSizeLabel" controlname="MinSize" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:TextBox id="MinSize" runat="server" Width="50px" TextMode="Number" />
            </div>
            <div class="dnnFormItem">
                    <dnn:label id="FillBoxLabel" runat="server" ResourceKey="FillBoxLabel" controlname="FillBox" suffix=":" CssClass="SubHead"></dnn:label>
                    <asp:CheckBox id="FillBox" runat="server" Width="50px" />
            </div>
      </fieldset>
    </div>
    <div id="TagSourceSettings">
    <div class="dnnFormExpandContent"><a href="">Expand All</a></div>
      <h2 id="TagSrcOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="TagSrcOptionsPanelLink" resourcekey="lTagSrcOpt"></asp:Label></a></h2>
      <fieldset class="dnnClear">
         <div class="dnnFormItem">
            <dnn:label id="lblTagMode" runat="server" ResourceKey="lblTagMode" controlname="rBlTagMode" suffix=":" CssClass="SubHead"></dnn:label>
            <asp:CheckBoxList id="TagModes" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
              <asp:ListItem Text="Custom Tags" Value="ModeCustom"></asp:ListItem>
              <asp:ListItem Text="Taxonomy" Value="ModeTax"></asp:ListItem>
            </asp:CheckBoxList>
         </div>
        </fieldset>
        <asp:PlaceHolder id="phCustom" runat="server" Visible="False">
         <h2 id="TagSrcCustomPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="TagSrcCustomPanelLink" resourcekey="lTagSrcCustom"></asp:Label></a></h2>
         <fieldset class="dnnClear">
             <div class="dnnFormItem">
                 <dnn:label id="lblCustomTags" runat="server" ResourceKey="lblCustomTags" controlname="tbCustomTag" suffix=":" CssClass="SubHead"></dnn:label>
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
      <td style="vertical-align: top">
        <dnn:label id="lblCustomTag" runat="server" ResourceKey="lblCustomTag" controlname="tbCustomTag" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:Label id="lTagId" runat="server" Visible="false"></asp:Label>
         <asp:Label id="lTagLocale" runat="server" Visible="false"></asp:Label>
      </td>
      <td>
        <table id="tCustomTag" style="width:100%;" runat="server">
          <tr>
            <td style="vertical-align: top"><asp:Label ID="lCustomTag" runat="server" Width="100px" Text="Tag Name:"></asp:Label></td>
            <td style="vertical-align: top"><asp:TextBox id="tbCustomTag" runat="server" Width="194px" /></td>
          </tr>
          <tr>
            <td style="vertical-align: top"><asp:Label ID="lTagWeight" runat="server" Text="Tag Weight:"></asp:Label></td>
            <td style="vertical-align: top"><asp:DropDownList id="dDlTagWeight" runat="server"></asp:DropDownList></td>
          </tr>
          <tr>
            <td style="vertical-align: top"><asp:Label ID="lTagUrl" runat="server" Text="Tag URL:"></asp:Label></td>
            <td style="vertical-align: top">
              <dnn:url id="ctlTagUrl" runat="server" width="300" showtabs="True" showfiles="False" showUrls="True"
					urltype="F" showlog="False" shownewwindow="False" showtrack="False"></dnn:url>
		    </td>
          </tr>
          <tr>
            <td></td>
            <td style="vertical-align: top">
              <asp:LinkButton id="iBTagAdd" runat="server" CssClass="dnnPrimaryAction" />
              &nbsp;<asp:LinkButton id="iBTagSave" runat="server" Visible="false" CssClass="dnnPrimaryAction" />
              &nbsp;<asp:LinkButton id="iBTagLocalize" runat="server" Visible="false" CssClass="dnnPrimaryAction" />
              &nbsp;<asp:LinkButton id="iBTagEditCancel" runat="server" Visible="false" CssClass="dnnPrimaryAction" />
            </td>
          </tr>
        </table>
        </fieldset>
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
                 </div>
           <div class="dnnFormItem">
       <a onclick="showDialog('ImportDialog');" id="lnkImport" href="#" class="dnnPrimaryAction"><asp:Label id="lblImport" runat="server" Text="Import Tags"></asp:Label></a>
        &nbsp;
        <a onclick="showDialog('ExportDialog');" id="lnkExport" href="#" class="dnnPrimaryAction"><asp:Label id="lblExport" runat="server" Text="Export Tags"></asp:Label></a>
        <div id="ImportDialog" title="Import Custom Tags" style="display:none" class="dnnClear">
         <asp:UpdatePanel ID="upImport" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
               <dnn:url id="ctlImportFile" runat="server" width="300" showtabs="False" Required="False" filefilter="xml" showfiles="True" showUrls="False"
					urltype="F" showlog="False" shownewwindow="False" showtrack="False"></dnn:url>
                      <asp:LinkButton id="cmdImport" runat="server" Visible="false"></asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <div id="ExportDialog" title="Export Custom Tags" style="display:none" class="dnnClear">
         <asp:UpdatePanel ID="upExport" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
              <p><asp:DropDownList id="cboFolders" Runat="server" CssClass="NormalTextBox" Width="300"></asp:DropDownList></p>
              <p><asp:TextBox id="txtExportName" runat="server" Text="TagCloudCustomItems.xml" Width="300"></asp:TextBox></p>
              <asp:LinkButton id="cmdExport" runat="server" Visible="false"></asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
     </div>
    </fieldset>
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phTax" runat="server" Visible="False">
        <h2 id="TaxOptionPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="TaxOptionPanelLink" resourcekey="lTagSrcTax"></asp:Label></a></h2>
        <fieldset class="dnnClear">
            <div class="dnnFormItem">
             <dnn:label id="lblTaxMode" runat="server" ResourceKey="lblTaxMode" controlname="dDlTaxMode" suffix=":" CssClass="SubHead"></dnn:label>
             <asp:DropDownList id="dDlTaxMode" runat="server" AutoPostBack="true">
             </asp:DropDownList>
         </div>
         <div class="dnnFormItem">
              <dnn:label id="lblChooseVoc" runat="server" ResourceKey="lblChooseVoc" controlname="cBlVocabularies" suffix=":" CssClass="SubHead" Visible="false"></dnn:label>
             <asp:CheckBoxList ID="cBlVocabularies" runat="server" Visible="false"></asp:CheckBoxList>
         </div>
         </fieldset>
         </asp:PlaceHolder>
         <asp:PlaceHolder id="phVentrianNews" runat="server" Visible="False">
         <h2 id="VentrianNewsOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="VentrianNewsOptionsPanelLink" resourcekey="lTagSrcVentrianNews"></asp:Label></a></h2>
         <fieldset class="dnnClear">
            <div class="dnnFormItem">
             <dnn:label id="lblModuleVentrianNews" ResourceKey="lblModuleVentrianNews" runat="server" controlname="ddLTabsVentrianNews" suffix=":" CssClass="SubHead"></dnn:label>
             <asp:DropDownList id="ddLTabsVentrianNews" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
            </div>
         </fieldset>
         </asp:PlaceHolder>
         <asp:PlaceHolder id="phVentrianSimple" runat="server" Visible="False">
         <h2 id="VentrianSimpleOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="VentrianSimpleOptionsPanelLink" resourcekey="lTagSrcVentrianSimple"></asp:Label></a></h2>
         <fieldset class="dnnClear">
          <div class="dnnFormItem">
             <dnn:label id="lblModuleVentrianSimple" ResourceKey="lblModuleVentrianSimple" runat="server" controlname="ddLTabsVentrianSimple" suffix=":" CssClass="SubHead"></dnn:label>
             <asp:DropDownList id="ddLTabsVentrianSimple" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
           </div>
         </fieldset>
         </asp:PlaceHolder>
        <asp:PlaceHolder id="phActiveforums" runat="server" Visible="False">
        <h2 id="ActiveForumsOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="ActiveForumsOptionsPanelLink" resourcekey="lTagSrcActiveforums"></asp:Label></a></h2>
        <fieldset class="dnnClear">
            <div class="dnnFormItem">
            <dnn:label id="lblActiveforums" ResourceKey="lblModuleActiveforums" runat="server" controlname="ddLTabsActiveforums" suffix=":" CssClass="SubHead"></dnn:label>
             <asp:DropDownList id="ddLTabsActiveforums" Width="325" runat="server"
                datavaluefield="ModuleID" datatextfield="ModuleTitle">
             </asp:DropDownList>
            </div>
        </fieldset>
        </asp:PlaceHolder>

    </div>
    <div id="ExcludeSettings">
        <div class="dnnFormExpandContent"><a href="">Expand All</a></div>
        <h2 id="ExcludeOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="ExcludeOptionsPanelLink" resourcekey="lExcludeOpt"></asp:Label></a></h2>
  <fieldset class="dnnClear">
     <div class="dnnFormItem">
        <dnn:label id="lblExcludeCommon" runat="server" ResourceKey="lblExcludeCommon" controlname="ExcludeCommon" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:CheckBox id="ExcludeCommon" runat="server"></asp:CheckBox>
     </div>
     <div class="dnnFormItem">
        <dnn:label id="lblExlusionList" runat="server" ResourceKey="lblExlusionList" controlname="tbExlusLst" suffix=":" CssClass="SubHead"></dnn:label>
        <asp:ListBox id="lBExList" runat="server" Width="60%"></asp:ListBox>
    </div>
    <div class="dnnFormItem">
        <asp:LinkButton id="iBEdit" runat="server" ImageUrl="~/images/edit.gif" CssClass="dnnPrimaryAction" />
        <asp:LinkButton id="iBDelete" runat="server" Text="Delete" CssClass="dnnPrimaryAction" />
    </div>
  </fieldset>
  <h2 id="ModifyExcludeWordOptionsPanel" class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" id="ModifyExcludeWordOptionsPanelLink" resourcekey="lModifyWord"></asp:Label></a></h2>
  <fieldset class="dnnClear">
      <div class="dnnFormItem">
         <dnn:label id="lblExcludeWord" runat="server" ResourceKey="lblExcludeWord" controlname="tbExlusLst" suffix=":" CssClass="SubHead"></dnn:label>
         <asp:TextBox id="tbExlusLst" runat="server" Width="194px"></asp:TextBox>
     </div>
     <div class="dnnFormItem">
         <dnn:label id="lblExlusionType" runat="server" ResourceKey="lblExlusionType" controlname="ExclusionType" suffix=":" CssClass="SubHead"></dnn:label>
         <asp:DropDownList id="ExclusionType" runat="server" Width="194px"></asp:DropDownList>
      </div>
      <div class="dnnFormItem">
          <asp:LinkButton id="iBAdd" runat="server" Text="Add" CommandArgument="add" CssClass="dnnPrimaryAction" />&nbsp;
          <asp:LinkButton id="iBCancel" runat="server" Text="Cancel" CssClass="dnnPrimaryAction" Visible="false" />
      </div>
     </fieldset>
  </div>
  </div>
</asp:panel>
  <ul class="dnnActions dnnClear">
    <li><asp:LinkButton id="Update" Text="Update" runat="server" ResourceKey="Update" CssClass="dnnPrimaryAction" /></li>
    <li><asp:LinkButton id="Cancel" Text="Cancel" runat="server" ResourceKey="Cancel" CssClass="dnnSecondaryAction" /></li>
  </ul>