/*  *********************************************************************************************
*
*   WatchersNET.TagCloud - This Module displays the most frequently used words (Tags) of your Portal as a
*   standard Web 2.0 Tag Cloud, or You can define your own Tags list.  The Tags are links which linked to the Portal Search to
*   show all Pages with that Tag.
*
*   The Tag Cloud will be rendered as 3D Flash Cloud, and as alternative for Non Flash
*   Users as a list of hyperlinks in varying styles depending on a weight.
*   This is similar to tag clouds in del.icio.us or Flickr.
*
*   Copyright(c) Ingo Herbote (thewatcher@watchersnet.de)
*   All rights reserved.
*   Internet: http://www.watchersnet.de/TagCloud
*
*   WatchersNET.TagCloud is released under the New BSD License, see below
************************************************************************************************
*
*   Redistribution and use in source and binary forms, with or without modification,
*   are permitted provided that the following conditions are met:
*
*   * Redistributions of source code must retain the above copyright notice,
*   this list of conditions and the following disclaimer.
*
*   * Redistributions in binary form must reproduce the above copyright notice,
*   this list of conditions and the following disclaimer in the documentation and/
*   or other materials provided with the distribution.
*
*   * Neither the name of WatchersNET nor the names of its contributors
*   may be used to endorse or promote products derived from this software without
*   specific prior written permission.
*
*   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
*   ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
*   OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
*   IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
*   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
*   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
*   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
*   LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
*   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
************************************************************************************************
*/

namespace WatchersNET.DNN.Modules.TagCloud
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml.Serialization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Content.Common;
    using DotNetNuke.Entities.Content.Taxonomy;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.FileSystem;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;
    using DotNetNuke.Web.Client.ClientResourceManagement;

    using WatchersNET.DNN.Modules.TagCloud.Constants;
    using WatchersNET.DNN.Modules.TagCloud.Objects;

    using DataCache = DotNetNuke.Common.Utilities.DataCache;
    using FileInfo = System.IO.FileInfo;
    using Globals = DotNetNuke.Common.Globals;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    public partial class Settings : PortalModuleBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The c validator por id.
        /// </summary>
        protected CustomValidator cValidatorPorId;

        /// <summary>
        ///   The ctl import file.
        /// </summary>
        protected UrlControl ctlImportFile;

        /// <summary>
        ///   The ctl tag url.
        /// </summary>
        protected UrlControl ctlTagUrl;

        /*
        /// <summary>
        ///   The dsh comm opt.
        /// </summary>
        protected SectionHeadControl dshCommOpt;

        /// <summary>
        ///   The dsh flash opt.
        /// </summary>
        protected SectionHeadControl dshFlashOpt;

        /// <summary>
        /// The dsh Exlcude Opt.
        /// </summary>
        protected SectionHeadControl dshExcludeOpt;
         * */

        /// <summary>
        ///   The lbl choose voc.
        /// </summary>
        protected LabelControl lblChooseVoc;

        /// <summary>
        ///   The pnl setting.
        /// </summary>
        protected Panel pnlSetting;

        /// <summary>
        ///   The up export.
        /// </summary>
        protected UpdatePanel upExport;

        /// <summary>
        ///   The up grid.
        /// </summary>
        protected UpdatePanel upGrid;

        /// <summary>
        ///   The up import.
        /// </summary>
        protected UpdatePanel upImport;

        /// <summary>
        ///   The b mode activeforums.
        /// </summary>
        private bool bModeActiveforums;

        /// <summary>
        ///   The b mode custom.
        /// </summary>
        private bool bModeCustom;

        /// <summary>
        ///   The b mode newsarticles.
        /// </summary>
        private bool bModeNewsarticles;

        /// <summary>
        ///   The b mode simplegallery.
        /// </summary>
        private bool bModeSimplegallery;

        /// <summary>
        ///   The b mode tax.
        /// </summary>
        private bool bModeTax;

        /// <summary>
        ///   The exlusion words.
        /// </summary>
        private readonly List<ExcludeWord> exlusionWords = new List<ExcludeWord>();

        /// <summary>
        ///   The vocabularies.
        /// </summary>
        private string[] vocabularies;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets ImportFile.
        /// </summary>
        private UrlControl ImportFile
        {
            get
            {
                return this.ctlImportFile;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if Height is a Numeric value
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        protected void CheckFlashHeight(object source, ServerValidateEventArgs e)
        {
            if (!Utility.IsNumeric(e.Value))
            {
                this.cVFlashHeight.ErrorMessage = Localization.GetString("Error.Text", this.LocalResourceFile);
                e.IsValid = false;
            }
            else
            {
                if (int.Parse(e.Value) < 150)
                {
                    this.cVFlashHeight.ErrorMessage = Localization.GetString("Error_1.Text", this.LocalResourceFile);
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
            }
        }

        /// <summary>
        /// Check if Width is a Numeric value
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void CheckFlashWidth(object source, ServerValidateEventArgs e)
        {
            if (!Utility.IsNumeric(e.Value))
            {
                this.cVFlashWidth.ErrorMessage = Localization.GetString("Error.Text", this.LocalResourceFile);
                e.IsValid = false;
            }
            else
            {
                if (int.Parse(e.Value) < 150)
                {
                    this.cVFlashWidth.ErrorMessage = Localization.GetString("Error_1.Text", this.LocalResourceFile);
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
            }
        }

        /// <summary>
        /// Check if Width is a Numeric value
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void CheckWidth(object source, ServerValidateEventArgs e)
        {
            if (!Utility.IsNumeric(e.Value))
            {
                this.cVWidth.ErrorMessage = Localization.GetString("Error.Text", this.LocalResourceFile);
                e.IsValid = false;
            }
            else
            {
                if (int.Parse(e.Value) < 150 && this.dDlWidth.SelectedValue.Equals("pixel"))
                {
                    this.cVWidth.ErrorMessage = Localization.GetString("Error_1.Text", this.LocalResourceFile);
                    e.IsValid = false;
                }
                else if (int.Parse(e.Value) < 25 && this.dDlWidth.SelectedValue.Equals("percent"))
                {
                    this.cVWidth.ErrorMessage = Localization.GetString("Error_2.Text", this.LocalResourceFile);
                    e.IsValid = false;
                }
                else if (int.Parse(e.Value) > 100 && this.dDlWidth.SelectedValue.Equals("percent"))
                {
                    this.cVWidth.ErrorMessage = Localization.GetString("Error_3.Text", this.LocalResourceFile);
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Settings_Load;
            this.Update.Click += this.UpdateClick;
            this.Cancel.Click += this.CancelClick;

            // CODEGEN: This call is required by the ASP.NET Web Form Designer.);
            this.InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.Page.ClientScript.RegisterForEventValidation(this.cmdExport.UniqueID);
            this.Page.ClientScript.RegisterForEventValidation(this.cmdImport.UniqueID);

            base.Render(writer);
        }

        /// <summary>
        /// Load Settings
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The Event Arguments
        /// </param>
        private void Settings_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsPostBack)
                {
                    return;
                }

                this.LoadModuleSettings();
                this.AddJavaScript(0);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Save the module settings
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The Event Arguments
        /// </param>
        private void UpdateClick(object sender, EventArgs e)
        {
            try
            {
                this.SaveChanges();

                this.Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Cancel Edit Settings
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The Event Arguments
        /// </param>
        private void CancelClick(object sender, EventArgs e)
        {
            this.Response.Redirect(Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Add Third Party Tag Modes if available
        /// </summary>
        private void AddExtraModes()
        {
            if (Utility.IsNewsArticlesInstalled())
            {
                this.TagModes.Items.Add(new ListItem("NewsArticles Tags", "ModeNewsarticles"));
            }

            if (Utility.IsSimplyGalleryInstalled())
            {
                this.TagModes.Items.Add(new ListItem("SimpleGallery Tags", "ModeSimplegallery"));
            }

            if (Utility.IsActiveForumsInstalled())
            {
                this.TagModes.Items.Add(new ListItem("ActiveForums Tags", "ModeActiveforums"));
            }
        }

        /// <summary>
        /// Add the needed Javascript for the Color Picker
        /// </summary>
        /// <param name="restoreTab">The restore Tab.</param>
        /// <param name="panelOpenSelector">The panel open selector.</param>
        private void AddJavaScript(int restoreTab, string panelOpenSelector = null)
        {
            this.AddTabScript(restoreTab, panelOpenSelector);

            Type csType = typeof(Page);

            StringBuilder sbColorPScript = new StringBuilder();

            sbColorPScript.Append("$(document).ready(function() {");

            sbColorPScript.Append("jQuery.fn.jPicker.defaults.window.position.y=($(\".TagCloudLogo\").position().top) + 200 + 'px';");
            sbColorPScript.AppendFormat("jQuery.fn.jPicker.defaults.images.clientPath='{0}';", this.ResolveUrl("images/"));

            // Tcolor
            sbColorPScript.AppendFormat("$(\"#{0}\").jPicker({{window:{{title:'{1}' }} }});", this.tbTcolor.ClientID, Localization.GetString("lblTcolor.Help", this.LocalResourceFile));

            // Tcolor2
            sbColorPScript.AppendFormat("$(\"#{0}\").jPicker({{window:{{title:'{1}' }} }});", this.tbTcolor2.ClientID, Localization.GetString("lblTcolor2.Help", this.LocalResourceFile));

            // Hicolor
            sbColorPScript.AppendFormat("$(\"#{0}\").jPicker({{window:{{title:'{1}' }} }});", this.tbHicolor.ClientID, Localization.GetString("lblHicolor.Help", this.LocalResourceFile));

            // BgColor
            if (this.tbBgcolor.Enabled)
            {
                sbColorPScript.AppendFormat("$(\"#{0}\").jPicker({{window:{{title:'{1}' }} }});", this.tbBgcolor.ClientID, Localization.GetString("lblBgcolor.Help", this.LocalResourceFile));
                sbColorPScript.Append("});");
            }
            else
            {
                sbColorPScript.Append("});");
            }

            ScriptManager.RegisterStartupScript(this, csType, this.ID, sbColorPScript.ToString(), true);
        }

        /// <summary>
        /// The add tab script.
        /// </summary>
        /// <param name="restoreTab">The restore Tab.</param>
        /// <param name="panelOpenSelector">The panel open selector.</param>
        private void AddTabScript(int restoreTab, string panelOpenSelector = null)
        {
            ClientResourceManager.RegisterStyleSheet(this.Page, this.ResolveUrl("module.css"));

            /*ClientResourceManager.RegisterStyleSheet(
                this.Page, "//ajax.googleapis.com/ajax/libs/jqueryui/1/themes/blitzer/jquery-ui.css");
            */
            Type csType = typeof(Page);

            // Register jQuery
            jQuery.RequestRegistration();

            jQuery.RequestUIRegistration();

            jQuery.RequestDnnPluginsRegistration();

            ScriptManager.RegisterClientScriptInclude(
                this, csType, "jqueryNumeric", this.ResolveUrl("js/jquery.numeric.js"));

            ScriptManager.RegisterClientScriptInclude(
                this, csType, "jqueryColorPicker", this.ResolveUrl("js/jpicker-1.1.6.min.js"));

            StringBuilder sbDialogJs = new StringBuilder();

            sbDialogJs.Append("jQuery(function ($) {");
            sbDialogJs.Append("var setupTagCloudSettingsTabs = function () {");

            sbDialogJs.AppendFormat("$(\"#SettingTabs\").dnnTabs({{ selected: {0} }}).dnnPanels();", restoreTab);

            var expandText = Localization.GetString("ExpandAll", Localization.SharedResourceFile);
            var collapseText = Localization.GetString("CollapseAll", Localization.SharedResourceFile);

            sbDialogJs.AppendFormat(
                "$('#BasicSettings .dnnFormExpandContent a').dnnExpandAll({{ expandText: '{0}', collapseText: '{1}', targetArea: '#BasicSettings' }});",
                expandText,
                collapseText);
            sbDialogJs.AppendFormat(
                "$('#TagSourceSettings .dnnFormExpandContent a').dnnExpandAll({{ expandText: '{0}', collapseText: '{1}', targetArea: '#TagSourceSettings' }});",
                expandText,
                collapseText);
            sbDialogJs.AppendFormat(
                "$('#FlashSettings .dnnFormExpandContent a').dnnExpandAll({{ expandText: '{0}', collapseText: '{1}', targetArea: '#FlashSettings' }});",
                expandText,
                collapseText);
            sbDialogJs.AppendFormat(
                "$('#ExcludeSettings .dnnFormExpandContent a').dnnExpandAll({{ expandText: '{0}', collapseText: '{1}', targetArea: '#ExcludeSettings' }});",
                expandText,
                collapseText);

            sbDialogJs.Append("};");

            sbDialogJs.Append("setupTagCloudSettingsTabs();");

            sbDialogJs.Append("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {");
            sbDialogJs.Append("setupTagCloudSettingsTabs();");

            sbDialogJs.Append("}); });");

            if (!string.IsNullOrEmpty(panelOpenSelector))
            {
                sbDialogJs.Append("$(document).ready(function() {");

                sbDialogJs.AppendFormat("$('#{0} a').click();", panelOpenSelector);
                sbDialogJs.Append("});");
            }

            /*if (this.bModeCustom)
            {*/
                sbDialogJs.Append("$(document).ready(function() {");

                sbDialogJs.Append("jQuery(\".NumericTextBox\").numeric();");

                // Import Dialog
                sbDialogJs.Append("jQuery('#ImportDialog').dialog({");
                sbDialogJs.Append("autoOpen: false,");
                sbDialogJs.Append("width: 350,");
                sbDialogJs.AppendFormat(
                    "buttons: {{ \"Cancel\": function () {{ jQuery(this).dialog(\"close\"); }}, \"{1}\": function () {{ __doPostBack('{0}', '');jQuery(this).dialog(\"close\"); }} }},",
                    this.cmdImport.ClientID.Replace("_", "$"),
                Localization.GetString("cmdImport.Text", this.LocalResourceFile));
                sbDialogJs.Append("open: function (type, data) {");
                sbDialogJs.Append("jQuery(this).parent().appendTo(\"form\");");
                sbDialogJs.Append("}");
                sbDialogJs.Append("});");

                // Export Dialog
                sbDialogJs.Append("jQuery('#ExportDialog').dialog({");
                sbDialogJs.Append("autoOpen: false,");
                sbDialogJs.Append("width: 350,");
                sbDialogJs.AppendFormat(
                    "buttons: {{ \"Cancel\": function () {{ jQuery(this).dialog(\"close\"); }}, \"{2}\": function () {{ __doPostBack('{0}', '');alert('{1}'); jQuery(this).dialog(\"close\"); }} }},",
                    this.cmdExport.ClientID.Replace("_", "$"),
                    Localization.GetString("TagsExported.Text", this.LocalResourceFile),
                    Localization.GetString("cmdExport.Text", this.LocalResourceFile));
                sbDialogJs.Append("open: function (type, data) {");
                sbDialogJs.Append("jQuery(this).parent().appendTo(\"form\");");
                sbDialogJs.Append("}");
                sbDialogJs.Append("});");

                sbDialogJs.Append("});");

                // Dialog Open Script
                sbDialogJs.Append("function showDialog(id) {");
                sbDialogJs.Append("$('#' + id).dialog(\"open\");");
                sbDialogJs.Append("}");
            //}

            ScriptManager.RegisterStartupScript(this, csType, "jqueryTabnDialogScript", sbDialogJs.ToString(), true);
        }

        /// <summary>
        /// Set Transparent on/off and Background Textbox enabled/disabled
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void CBTransparentCheckedChanged(object sender, EventArgs e)
        {
            this.tbBgcolor.Enabled = !this.cbTransparent.Checked;

            this.AddJavaScript(2);
        }

        /// <summary>
        /// Show Vocabulary Selector
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void DDlTaxModeSelectedIndexChanged(object sender, EventArgs e)
        {
            this.AddJavaScript(1);

            if (this.dDlTaxMode.SelectedValue.Equals("custom"))
            {
                this.cBlVocabularies.Visible = true;
                this.lblChooseVoc.Visible = true;

                if (this.vocabularies == null)
                {
                    return;
                }

                foreach (string sVocabulary in this.vocabularies)
                {
                    this.cBlVocabularies.Items.FindByValue(sVocabulary).Selected = true;
                }
            }
            else
            {
                this.cBlVocabularies.Visible = false;
                this.lblChooseVoc.Visible = false;
            }
        }

        /// <summary>
        /// Delete all Locales of the Custom Tag
        /// </summary>
        /// <param name="iTagId">
        /// The i Tag Id.
        /// </param>
        private void DeleteAllLocales(int iTagId)
        {
            foreach (Locale language in new LocaleController().GetLocales(this.PortalId).Values)
            {
                DataControl.TagCloudItemsDeleteMl(iTagId, this.ModuleId, language.Code);
            }
        }

        /// <summary>
        /// Export all Custom Tags as serialised Xml File
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void ExportClick(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomTags>));

            string sXmlFile = this.txtExportName.Text;

            if (string.IsNullOrEmpty(sXmlFile))
            {
                // If Input File Name is Empty
                sXmlFile = "TagCloudCustomItems.xml";
            }
            else if (!sXmlFile.EndsWith(".xml"))
            {
                // Make Sure File Extension is xml
                sXmlFile += ".xml";
            }

            var exportFileStream = new FileStream(Path.GetTempFileName(), FileMode.Create);

            TextWriter tr = new StreamWriter(exportFileStream, Encoding.UTF8);
            serializer.Serialize(tr, DataControl.TagCloudItemsGetByModule(this.ModuleId));

            var folderInfo = FolderManager.Instance.GetFolder(Convert.ToInt32(this.cboFolders.SelectedValue));

            FileManager.Instance.AddFile(folderInfo, sXmlFile, exportFileStream);

            tr.Close();

            this.upGrid.Update();

            // RESET Dialog
            this.cboFolders.SelectedIndex = 0;
            this.txtExportName.Text = "TagCloudCustomItems.xml";

            this.AddJavaScript(1);
        }

        /// <summary>
        /// Load the Custom Tag List from Sql
        /// </summary>
        private void FillCustomTags()
        {
            DataTable dtTags = new DataTable();

            dtTags.Columns.Add("TagID");
            dtTags.Columns.Add("Tag");
            dtTags.Columns.Add("TagUrl");
            dtTags.Columns.Add("Weight");

            foreach (CustomTags tag in DataControl.TagCloudItemsGetByModule(this.ModuleId))
            {
                DataRow drNewRow = dtTags.NewRow();

                drNewRow["TagID"] = tag.iTagId.ToString();
                drNewRow["Tag"] = tag.sTag;
                drNewRow["TagUrl"] = tag.sUrl;
                drNewRow["Weight"] = tag.iWeight.ToString();

                dtTags.Rows.Add(drNewRow);
            }

            this.grdTagList.DataSource = dtTags;
            this.grdTagList.DataBind();
        }

        /// <summary>
        /// Fill Folder DropDown for the Export Custom Tags Dialog
        /// </summary>
        private void FillExportFolders()
        {
            // ArrayList folders = FileSystemUtils.GetFoldersByUser(this.PortalId, false, false, "READ, WRITE");
            var folders = FolderManager.Instance.GetFolders(UserController.GetCurrentUserInfo(), "READ, WRITE");

            foreach (ListItem folderItem in from FolderInfo folder in folders
                                            select
                                                new ListItem
                                                    {
                                                        Text =
                                                            folder.FolderPath == Null.NullString
                                                                ? "Root"
                                                                : folder.FolderPath,
                                                        Value = folder.FolderID.ToString()
                                                    })
            {
                this.cboFolders.Items.Add(folderItem);
            }
        }

        /// <summary>
        /// Load Localized Tag
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="tagUrl">The tag URL.</param>
        private void FillLocalizedTag(string tag, string tagUrl)
        {
            this.grdTagLocales.Visible = true;

            DataTable dtLocales = new DataTable();

            dtLocales.Columns.Add("TagID");
            dtLocales.Columns.Add("Locale");
            dtLocales.Columns.Add("Tag");
            dtLocales.Columns.Add("TagUrl");

            List<Locales> localesList = DataControl.TagCloudItemsGetByLocale(this.ModuleId, int.Parse(this.lTagId.Text));

            foreach (Locale language in new LocaleController().GetLocales(this.PortalId).Values)
            {
                DataRow drNewRow = dtLocales.NewRow();

                drNewRow["TagID"] = this.lTagId.Text;
                drNewRow["Locale"] = language.Code;

                drNewRow["Tag"] = tag;
                drNewRow["TagUrl"] = tagUrl;

                // Fill With SQL Values
                Locale language1 = language;

                Locales locale = localesList.Find(check => check.Locale.Equals(language1.Code));

                if (locale != null)
                {
                    drNewRow["Tag"] = locale.TagMl;
                    drNewRow["TagUrl"] = locale.UrlMl;
                }

                dtLocales.Rows.Add(drNewRow);
            }

            this.grdTagLocales.DataSource = dtLocales;
            this.grdTagLocales.DataBind();
        }

        /// <summary>
        /// Get all Modules
        /// </summary>
        /// <param name="sModuleName">
        /// The s Module Name.
        /// </param>
        /// <param name="objTabs">
        /// The obj Tabs.
        /// </param>
        /// <param name="dropDownList">
        /// The drop Down List.
        /// </param>
        private void FillModuleList(string sModuleName, IEnumerable<TabInfo> objTabs, ListControl dropDownList)
        {
            dropDownList.Items.Clear();

            var objTabController = new TabController();

            //var objDesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName(sModuleName, this.PortalId);
            var objDesktopModuleController = new DesktopModuleController();
            var objDesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName(sModuleName);

            if (objDesktopModuleInfo == null)
            {
                objDesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByName(sModuleName);

                if (objDesktopModuleInfo == null)
                {
                    return;
                }
            }

            foreach (TabInfo objTab in objTabs.Where(tab => !tab.IsDeleted))
            {
                ModuleController objModules = new ModuleController();

                foreach (KeyValuePair<int, ModuleInfo> pair in objModules.GetTabModules(objTab.TabID))
                {
                    ModuleInfo objModule = pair.Value;

                    if (objModule.IsDeleted)
                    {
                        continue;
                    }

                    if (objModule.DesktopModuleID != objDesktopModuleInfo.DesktopModuleID)
                    {
                        continue;
                    }

                    string strPath = objTab.TabName;
                    TabInfo objTabSelected = objTab;

                    while (objTabSelected.ParentId != Null.NullInteger)
                    {
                        objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, false);
                        if (objTabSelected == null)
                        {
                            break;
                        }

                        strPath = string.Format("{0} -> {1}", objTabSelected.TabName, strPath);
                    }

                    ListItem objListItem;

                    objListItem = new ListItem
                    {
                        Value = string.Format("{0}-{1}", objModule.TabID, objModule.ModuleID),
                        Text = string.Format("{0} -> {1}", strPath, objModule.ModuleTitle)
                    };

                    dropDownList.Items.Add(objListItem);
                }
            }
        }

        /// <summary>
        /// Fill Search Items List
        /// </summary>
        private void FillSearchLst()
        {
            ListItem item0 = new ListItem();
            ListItem item1 = new ListItem();
            ListItem item2 = new ListItem();
            ListItem item3 = new ListItem();
            ListItem item4 = new ListItem();

            // Google, Bing, Yahoo, Ask.com and the Portal Search
            item0.Text = string.Format("<img src=\"{0}\" />&nbsp;Google", this.ResolveUrl("google.gif"));
            item0.Value = "google";
            item0.Enabled = false;
            item0.Selected = true;

            item1.Text = string.Format("<img src=\"{0}\" />&nbsp;Bing", this.ResolveUrl("Bing.gif"));
            item1.Value = "bing";
            item1.Enabled = false;
            item1.Selected = true;

            item2.Text = string.Format("<img src=\"{0}\" />&nbsp;Yahoo", this.ResolveUrl("yahoo.gif"));
            item2.Value = "yahoo";
            item2.Enabled = false;
            item2.Selected = true;

            item3.Text = string.Format("<img src=\"{0}\" />&nbsp;Ask.com", this.ResolveUrl("ask.gif"));
            item3.Value = "ask";
            item3.Enabled = false;
            item3.Selected = true;

            item4.Text = string.Format("<img src=\"{0}\" />&nbsp;Site Search", this.ResolveUrl("dotnetnuke.gif"));
            item4.Value = "site";
            item4.Enabled = false;
            item4.Selected = true;
        }

        /// <summary>
        /// Loads the List of available Skins.
        /// </summary>
        private void FillSkinList()
        {
            this.dDlSkins.Items.Clear();

            DirectoryInfo objDir = new DirectoryInfo(this.MapPath(this.ResolveUrl("Skins")));

            foreach (FileInfo objFile in objDir.GetFiles("*.css"))
            {
                string sName = objFile.Name;
                sName = sName.Remove(objFile.Name.LastIndexOf(".", StringComparison.Ordinal));

                ListItem skinItem = new ListItem { Text = objFile.Name, Value = sName };

                this.dDlSkins.Items.Add(skinItem);
            }

            // Add No SKIN Item
            this.dDlSkins.Items.Add(new ListItem { Text = "No Skin", Value = "None" });
        }

        /// <summary>
        /// Fill DropDownList with Ventrian Instances
        /// </summary>
        /// <param name="tagMode">
        /// The tag Mode.
        /// </param>
        /// <param name="tabs">
        /// The <paramref name="tabs"/>.
        /// </param>
        private void FillTabList(string tagMode, IEnumerable<TabInfo> tabs)
        {
            this.GetModuleInstaces(tabs, tagMode);
        }

        /// <summary>
        /// Fill Options for Tax Mode
        /// </summary>
        private void FillTaxOptions()
        {
            ListItem itemTab = new ListItem
                {
                   Text = Localization.GetString("TabTerms.Text", this.LocalResourceFile), Value = "tab"
                };

            this.dDlTaxMode.Items.Add(itemTab);

            ListItem itemAll = new ListItem
                {
                   Text = Localization.GetString("AllTerms.Text", this.LocalResourceFile), Value = "all"
                };

            this.dDlTaxMode.Items.Add(itemAll);

            ListItem itemCustom = new ListItem
                {
                   Text = Localization.GetString("CustomVocabulary.Text", this.LocalResourceFile), Value = "custom"
                };

            this.dDlTaxMode.Items.Add(itemCustom);
        }

        /// <summary>
        /// Fill Terms Vocabularie Selector
        /// </summary>
        private void FillVocabularies()
        {
            try
            {
                IVocabularyController vocabRep = Util.GetVocabularyController();

                IQueryable<Vocabulary> vs = from v in vocabRep.GetVocabularies()
                                            where
                                                v.ScopeType.ScopeType == "Application" ||
                                                (v.ScopeType.ScopeType == "Portal" && v.ScopeId == this.PortalId)
                                            select v;

                foreach (Vocabulary v in vs)
                {
                    this.cBlVocabularies.Items.Add(new ListItem(v.Name, v.VocabularyId.ToString()));
                }
            }
            catch (Exception)
            {
                this.dDlTaxMode.Items.RemoveAt(2);
            }
        }

        /// <summary>
        /// Fill the Weight Items
        /// </summary>
        private void FillWeightList()
        {
            for (int i = 00; i < 201; i++)
            {
                ListItem item = new ListItem { Text = i.ToString(), Value = i.ToString() };

                this.dDlTagWeight.Items.Add(item);
            }
        }

        /// <summary>
        /// Get All Module Instances
        /// </summary>
        /// <param name="objTabs">
        /// The obj Tabs.
        /// </param>
        /// <param name="sTagMode">
        /// The s Tag Mode.
        /// </param>
        private void GetModuleInstaces(IEnumerable<TabInfo> objTabs, string sTagMode)
        {
            switch (sTagMode)
            {
                case "newsarticles":
                    {
                        this.FillModuleList("DnnForge - NewsArticles", objTabs, this.ddLTabsVentrianNews);

                        if (this.ddLTabsVentrianNews.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty((string)this.Settings["NewsArticlesTab"]) &&
                                !string.IsNullOrEmpty((string)this.Settings["NewsArticlesModule"]))
                            {
                                if (this.ddLTabsVentrianNews.Items.FindByValue(string.Format(
                                    "{0}-{1}",
                                    this.Settings["NewsArticlesTab"],
                                    this.Settings["NewsArticlesModule"])) == null)
                                {
                                    return;
                                }

                                this.ddLTabsVentrianNews.SelectedValue = string.Format(
                                    "{0}-{1}",
                                    this.Settings["NewsArticlesTab"],
                                    this.Settings["NewsArticlesModule"]);
                            }
                        }
                    }

                    break;
                case "simplegallery":
                    {
                        this.FillModuleList("SimpleGallery", objTabs, this.ddLTabsVentrianSimple);

                        if (this.ddLTabsVentrianSimple.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty((string)this.Settings["SimpleGalleryTab"]) &&
                                !string.IsNullOrEmpty((string)this.Settings["SimpleGalleryModule"]))
                            {
                                if (this.ddLTabsVentrianSimple.Items.FindByValue(string.Format(
                                    "{0}-{1}",
                                    this.Settings["SimpleGalleryTab"],
                                    this.Settings["SimpleGalleryModule"])) == null)
                                {
                                    return;
                                }

                                this.ddLTabsVentrianSimple.SelectedValue = string.Format(
                                    "{0}-{1}",
                                    this.Settings["SimpleGalleryTab"],
                                    this.Settings["SimpleGalleryModule"]);
                            }
                        }
                    }

                    break;
                case "activeforums":
                    {
                        this.FillModuleList("Active Forums", objTabs, this.ddLTabsActiveforums);

                        if (this.ddLTabsActiveforums.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty((string)this.Settings["ActiveForumsTab"]) &&
                                !string.IsNullOrEmpty((string)this.Settings["ActiveForumsModule"]))
                            {
                                if (this.ddLTabsVentrianSimple.Items.FindByValue(string.Format(
                                    "{0}-{1}",
                                    this.Settings["ActiveForumsTab"],
                                    this.Settings["ActiveForumsModule"])) == null)
                                {
                                    return;
                                }

                                this.ddLTabsActiveforums.SelectedValue = string.Format(
                                    "{0}-{1}",
                                    this.Settings["ActiveForumsTab"],
                                    this.Settings["ActiveForumsModule"]);
                            }
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// The grd tag list item command.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void GrdTagListItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (((Control)e.CommandSource).ID)
            {
                case "btnEdit":
                    {
                        this.grdTagLocales.Visible = false;

                        this.iBTagSave.Visible = true;
                        this.iBTagSave.Text =
                            string.Format(
                                "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                                this.ResolveUrl("~/images/save.gif"),
                                Localization.GetString("SaveTag.Text", this.LocalResourceFile),
                                Localization.GetString("SaveTag.Text", this.LocalResourceFile));

                        this.iBTagLocalize.Visible = true;
                        this.iBTagAdd.Visible = false;
                        this.iBTagEditCancel.Visible = true;

                        this.tbCustomTag.Text = e.Item.Cells[4].Text;

                        if (!e.Item.Cells[5].Text.Equals("&nbsp;"))
                        {
                            this.ctlTagUrl.Url = e.Item.Cells[5].Text;
                        }

                        this.lTagId.Text = e.Item.Cells[0].Text;
                        this.dDlTagWeight.SelectedValue = e.Item.Cells[6].Text;
                    }

                    break;
                case "btnDelete":
                    {
                        this.iBTagSave.Visible = false;
                        this.iBTagLocalize.Visible = false;
                        this.iBTagAdd.Visible = true;

                        this.grdTagLocales.Visible = false;

                        DataControl.TagCloudItemsDelete(int.Parse(e.Item.Cells[0].Text), this.ModuleId);

                        // Delete also Locales
                        this.DeleteAllLocales(int.Parse(e.Item.Cells[0].Text));

                        this.tbCustomTag.Text = string.Empty;
                        this.lTagId.Text = string.Empty;
                        this.lTagLocale.Text = string.Empty;

                        this.FillCustomTags();
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The grd tag list item data bound.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void GrdTagListItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ImageButton btnTemp = (ImageButton)e.Item.FindControl("btnDelete");

            if (btnTemp != null)
            {
                btnTemp.Attributes.Add(
                    "OnClick",
                    string.Format("return confirm('{0}')", Localization.GetString("DeleteTag.Text", this.LocalResourceFile)));
            }
        }

        /// <summary>
        /// The grd tag locales item command.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void GrdTagLocalesItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (((Control)e.CommandSource).ID)
            {
                case "btnEdit":
                    {
                        this.iBTagSave.Visible = true;
                        this.iBTagSave.Text =
                            string.Format(
                                "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                                this.ResolveUrl("~/images/save.gif"),
                                Localization.GetString("SaveLocale.Text", this.LocalResourceFile),
                                Localization.GetString("SaveLocale.Text", this.LocalResourceFile));

                        this.iBTagAdd.Visible = false;
                        this.iBTagEditCancel.Visible = true;

                        if (!e.Item.Cells[5].Text.Equals("&nbsp;"))
                        {
                            this.tbCustomTag.Text = e.Item.Cells[5].Text;
                        }

                        if (!e.Item.Cells[6].Text.Equals("&nbsp;"))
                        {
                            this.ctlTagUrl.Url = e.Item.Cells[6].Text;
                        }

                        this.lTagId.Text = e.Item.Cells[0].Text;
                        this.lTagLocale.Text = e.Item.Cells[4].Text;
                    }

                    break;
                case "btnDelete":
                    {
                        DataControl.TagCloudItemsDeleteMl(
                            int.Parse(e.Item.Cells[0].Text), this.ModuleId, e.Item.Cells[4].Text);
                        this.FillLocalizedTag(string.Empty, string.Empty);

                        this.tbCustomTag.Text = string.Empty;
                        this.lTagId.Text = string.Empty;
                        this.lTagLocale.Text = string.Empty;
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The grd tag locales item data bound.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void GrdTagLocalesItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ImageButton btnTemp = (ImageButton)e.Item.FindControl("btnDelete");

            if (btnTemp != null)
            {
                btnTemp.Attributes.Add(
                    "OnClick",
                    string.Format("return confirm('{0}')", Localization.GetString("DeleteLocale.Text", this.LocalResourceFile)));
            }
        }

        /// <summary>
        /// Add new Exclude Word
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void IbAddClick(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbExlusLst.Text))
            {
                switch ((string)e.CommandArgument)
                {
                    case "save":
                        {
                            var updateItem = new ExcludeWord
                            {
                                Word = this.tbExlusLst.Text,
                                WordID = int.Parse(this.lBExList.SelectedValue),
                                ExcludeWordType = (ExcludeType)Enum.Parse(typeof(ExcludeType), this.ExclusionType.SelectedItem.Text),
                                ModuleID = this.TabModuleId
                            };

                            DataControl.TagCloudExcludeWordUpdate(updateItem);

                            this.exlusionWords.AddRange(DataControl.TagCloudExcludeWordsGetByModule(this.TabModuleId));

                            this.lBExList.Items.Clear();

                            foreach (
                                ListItem item in this.exlusionWords.Select(wordItem => new ListItem { Text = wordItem.Word, Value = wordItem.WordID.ToString() }))
                            {
                                this.lBExList.Items.Add(item);
                            }

                            this.iBAdd.CommandArgument = "add";

                            this.iBAdd.Text =
                                string.Format(
                                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                                    this.ResolveUrl("~/images/add.gif"),
                                    Localization.GetString("AddExWord.Text", this.LocalResourceFile));
                            this.iBAdd.ToolTip = Localization.GetString("AddExWord.Text", this.LocalResourceFile);

                            this.iBCancel.Visible = false;
                        }

                        break;
                    case "add":
                        {
                            var newItem = new ExcludeWord
                            {
                                Word = this.tbExlusLst.Text,
                                WordID = 0,
                                ExcludeWordType = (ExcludeType)Enum.Parse(typeof(ExcludeType), this.ExclusionType.SelectedItem.Text),
                                ModuleID = this.TabModuleId
                            };

                            int wordId = DataControl.TagCloudExcludeWordAdd(newItem);

                            ListItem item = new ListItem { Text = newItem.Word, Value = wordId.ToString() };

                            this.lBExList.Items.Add(item);
                        }

                        break;
                }
            }

            this.tbExlusLst.Text = string.Empty;

            this.AddJavaScript(3);
        }

        /// <summary>
        /// Delete Exclude Word
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void IbDeleteClick(object sender, EventArgs e)
        {
            if (this.lBExList.SelectedItem != null)
            {
                DataControl.TagCloudExcludeWordDelete(this.TabModuleId, int.Parse(this.lBExList.SelectedValue));

                this.lBExList.Items.RemoveAt(this.lBExList.SelectedIndex);
            }

            this.tbExlusLst.Text = string.Empty;

            this.AddJavaScript(3);
        }

        /// <summary>
        /// Cancel Edit Word
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void IbCancelClick(object sender, EventArgs e)
        {
            this.iBAdd.CommandArgument = "add";

            this.iBAdd.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                    this.ResolveUrl("~/images/add.gif"),
                    Localization.GetString("AddExWord.Text", this.LocalResourceFile));
            this.iBAdd.ToolTip = Localization.GetString("AddExWord.Text", this.LocalResourceFile);

            this.iBCancel.Visible = false;

            this.tbExlusLst.Text = string.Empty;

            this.AddJavaScript(3);
        }

        /// <summary>
        /// Edit Exclude Word
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void IbEditClick(object sender, EventArgs e)
        {
            if (this.lBExList.SelectedItem != null)
            {
                var editItem = DataControl.TagCloudExcludeWordsGetWord(this.TabModuleId, int.Parse(this.lBExList.SelectedValue));

                this.tbExlusLst.Text = editItem.Word;
                this.ExclusionType.SelectedValue = editItem.ExcludeWordType.ToString();
            }

            this.iBAdd.CommandArgument = "save";

            this.iBAdd.Text =
                                string.Format(
                                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                                    this.ResolveUrl("~/images/save.gif"),
                                    Localization.GetString("SaveExWord.Text", this.LocalResourceFile));
            this.iBAdd.ToolTip = Localization.GetString("SaveExWord.Text", this.LocalResourceFile);

            this.iBCancel.Visible = true;

            this.AddJavaScript(3);
        }

        /// <summary>
        /// Import a List with Custom Tags
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void ImportClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ImportFile.Url))
            {
                return;
            }

            string sXmlImport = this.ImportFile.Url;

            this.upGrid.Update();

            // RESET Dialog
            this.ImportFile.Url = null;

            try
            {
                int iFileId = int.Parse(sXmlImport.Substring(7));

                // var objFileController = new FileController();
                var objFileInfo = FileManager.Instance.GetFile(iFileId);

               /* DotNetNuke.Services.FileSystem.FileInfo objFileInfo = objFileController.GetFileById(
                    iFileId, this.PortalSettings.PortalId);*/
                sXmlImport = this.PortalSettings.HomeDirectoryMapPath + objFileInfo.Folder + objFileInfo.FileName;

                XmlSerializer serializer = new XmlSerializer(typeof(List<CustomTags>));
                TextReader tr = new StreamReader(Path.Combine(this.PortalSettings.HomeDirectoryMapPath, sXmlImport));

                List<CustomTags> listImport = (List<CustomTags>)serializer.Deserialize(tr);

                tr.Close();

                if (listImport.Count > 0)
                {
                    // Check Current List - to Prevent Exceptions when Items Exists
                    List<CustomTags> listCurrent = DataControl.TagCloudItemsGetByModule(this.ModuleId);

                    /////////////
                    int[] iNewTagId = { DataControl.TagCloudItemsGetByModule(this.ModuleId).Count };

                    foreach (CustomTags importTag in listImport)
                    {
                        while (listCurrent.Find(existsTag => existsTag.iTagId.Equals(iNewTagId[0])) != null)
                        {
                            iNewTagId[0]++;
                        }

                        CustomTags tag = new CustomTags
                            {
                                iWeight = importTag.iWeight,
                                sTag = importTag.sTag,
                                iModulId = this.ModuleId,
                                sUrl = importTag.sUrl,
                                iTagId = iNewTagId[0]
                            };

                        iNewTagId[0]++;

                        // Add to Sql
                        DataControl.TagCloudItemsAdd(tag);
                    }
                }
            }
            finally
            {
                this.FillCustomTags();

                this.AddJavaScript(1);

                this.upGrid.Update();
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Languages
            this.iBCancel.Text = string.Format(
                "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                this.ResolveUrl("~/images/cancel.gif"),
                Localization.GetString("Cancel.Text", this.LocalResourceFile));
            this.iBCancel.ToolTip = Localization.GetString("Cancel.Text", this.LocalResourceFile);

            this.iBAdd.Text = string.Format(
                 "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                 this.ResolveUrl("~/images/add.gif"),
                 Localization.GetString("AddExWord.Text", this.LocalResourceFile));
            this.iBAdd.ToolTip = Localization.GetString("AddExWord.Text", this.LocalResourceFile);

            this.iBEdit.Text = string.Format(
                "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                this.ResolveUrl("~/images/edit.gif"),
                Localization.GetString("EditExWord.Text", this.LocalResourceFile));
            this.iBEdit.ToolTip = Localization.GetString("EditExWord.Text", this.LocalResourceFile);

            this.iBDelete.Text = string.Format(
                "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                this.ResolveUrl("~/images/delete.gif"),
                Localization.GetString("DeleteExWord.Text", this.LocalResourceFile));
            this.iBDelete.ToolTip = Localization.GetString("DeleteExWord.Text", this.LocalResourceFile);

            this.iBTagAdd.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("~/images/add.gif"),
                    Localization.GetString("AddTag.Text", this.LocalResourceFile),
                    Localization.GetString("AddTag.Text", this.LocalResourceFile));

            this.iBTagSave.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("~/images/save.gif"),
                    Localization.GetString("SaveTag.Text", this.LocalResourceFile),
                    Localization.GetString("SaveTag.Text", this.LocalResourceFile));

            this.iBTagEditCancel.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("~/images/cancel.gif"),
                    Localization.GetString("Cancel.Text", this.LocalResourceFile),
                    Localization.GetString("Cancel.Text", this.LocalResourceFile));

            this.iBTagLocalize.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("~/images//icon_language_16px.gif"),
                    Localization.GetString("ShowLocalize.Text", this.LocalResourceFile),
                    Localization.GetString("ShowLocalize.Text", this.LocalResourceFile));

            /*this.dshCommOpt.Text = Localization.GetString("lCommOpt.Text", this.LocalResourceFile);
            this.dshFlashOpt.Text = Localization.GetString("lFlashOpt.Text", this.LocalResourceFile);
            this.dshExcludeOpt.Text = Localization.GetString("lExcludeOpt.Text", this.LocalResourceFile);*/

            this.lTab1.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("Settings.png"),
                    Localization.GetString("lCommOpt.Text", this.LocalResourceFile));

            this.lTab2.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("Flash.png"),
                    Localization.GetString("lFlashOpt.Text", this.LocalResourceFile));

            this.lTab3.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("Tag.png"),
                    Localization.GetString("lTagSrcOpt.Text", this.LocalResourceFile));

            this.lTab4.Text =
                string.Format(
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0;height:16px;weight:16px\" /> {1}",
                    this.ResolveUrl("Exclude.png"),
                    Localization.GetString("lExcludeOpt.Text", this.LocalResourceFile));

            this.rangevalidator1.ErrorMessage = Localization.GetString("rangevalidator1.Text", this.LocalResourceFile);

            this.lCustomTag.Text = Localization.GetString("lCustomTag.Text", this.LocalResourceFile);
            this.lTagWeight.Text = Localization.GetString("lTagWeight.Text", this.LocalResourceFile);
            this.lTagUrl.Text = Localization.GetString("lTagUrl.Text", this.LocalResourceFile);

            this.lblImport.Text = string.Format(
                "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                this.ResolveUrl("~/images/action_import.gif"),
                Localization.GetString("cmdImport.Text", this.LocalResourceFile));

            this.lblExport.Text = string.Format(
                "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border:0\" /> {1}",
                this.ResolveUrl("~/images/action_export.gif"),
                Localization.GetString("cmdExport.Text", this.LocalResourceFile));

            this.FillSkinList();

            this.cbTransparent.Text = string.Format("&nbsp;{0}", Localization.GetString("transparent.Text", this.LocalResourceFile));

            this.cbTransparent.CheckedChanged += this.CBTransparentCheckedChanged;

            this.iBAdd.Command += this.IbAddClick;
            this.iBEdit.Click += this.IbEditClick;
            this.iBDelete.Click += this.IbDeleteClick;
            this.iBCancel.Click += this.IbCancelClick;

            this.iBTagAdd.Click += this.TagAddClick;
            this.iBTagSave.Click += this.TagSaveClick;
            this.iBTagLocalize.Click += this.TagLocalizeClick;
            this.iBTagEditCancel.Click += this.TagEditCancelClick;

            this.cmdImport.Click += this.ImportClick;
            this.cmdExport.Click += this.ExportClick;

            this.TagModes.SelectedIndexChanged += this.TagModesSelectedIndexChanged;
            this.RenderModeType.SelectedIndexChanged += this.RenderModeType_SelectedIndexChanged;

            this.dDlTaxMode.SelectedIndexChanged += this.DDlTaxModeSelectedIndexChanged;

            this.grdTagList.ItemDataBound += this.GrdTagListItemDataBound;
            this.grdTagList.ItemCommand += this.GrdTagListItemCommand;

            this.grdTagLocales.ItemDataBound += this.GrdTagLocalesItemDataBound;
            this.grdTagLocales.ItemCommand += this.GrdTagLocalesItemCommand;

            this.AddExtraModes();

            foreach (ListItem listItem in this.TagModes.Items)
            {
                    listItem.Text = Localization.GetString(string.Format("{0}.Text", listItem.Value), this.LocalResourceFile);
            }

            this.FillWeightList();

            this.FillTaxOptions();

            this.FillVocabularies();

            this.FillCustomTags();
            this.FillSearchLst();

            this.FillExportFolders();
        }

        /// <summary>
        /// Loads the Settings
        /// </summary>
        private void LoadModuleSettings()
        {
            // Setting TagSeparator
            if (!string.IsNullOrEmpty((string)this.Settings["TagSeparator"]))
            {
                try
                {
                    this.txtTagSeparator.Text = (string)this.Settings["TagSeparator"];
                }
                catch (Exception)
                {
                    this.txtTagSeparator.Text = string.Empty;
                }
            }

            // Setting TagMode
            // Load old Setting
            if (!string.IsNullOrEmpty((string)this.Settings["TagMode"]))
            {
                string sTagMode = (string)this.Settings["TagMode"];

                switch (sTagMode)
                {
                    case "custom":
                        this.TagModes.Items.FindByValue("ModeCustom").Selected = true;
                        this.bModeCustom = true;
                        break;
                    case "newsarticles":
                        if (Utility.IsNewsArticlesInstalled())
                        {
                            this.TagModes.Items.FindByValue("ModeNewsarticles").Selected = true;
                            this.bModeNewsarticles = true;
                        }

                        break;
                    case "simplegallery":
                        if (Utility.IsSimplyGalleryInstalled())
                        {
                            this.TagModes.Items.FindByValue("ModeSimplegallery").Selected = true;
                            this.bModeSimplegallery = true;
                        }

                        break;
                    case "activeforums":
                        if (Utility.IsActiveForumsInstalled())
                        {
                            this.TagModes.Items.FindByValue("ModeActiveforums").Selected = true;
                            this.bModeActiveforums = true;
                        }

                        break;
                    case "tax":
                        this.TagModes.Items.FindByValue("ModeTax").Selected = true;
                        this.bModeTax = true;
                        break;
                }

                // Delete Old Setting.
                ModuleController objModules = new ModuleController();
                objModules.DeleteTabModuleSetting(this.TabModuleId, "TagMode");
            }

            if (!string.IsNullOrEmpty((string)this.Settings["ModeCustom"]))
            {
                bool.TryParse((string)this.Settings["ModeCustom"], out this.bModeCustom);

                this.TagModes.Items.FindByValue("ModeCustom").Selected = this.bModeCustom;
            }

            if (!string.IsNullOrEmpty((string)this.Settings["ModeTax"]))
            {
                bool.TryParse((string)this.Settings["ModeTax"], out this.bModeTax);

                this.TagModes.Items.FindByValue("ModeTax").Selected = this.bModeTax;
            }

            if (!string.IsNullOrEmpty((string)this.Settings["ModeNewsarticles"]) && Utility.IsNewsArticlesInstalled())
            {
                bool.TryParse((string)this.Settings["ModeNewsarticles"], out this.bModeNewsarticles);

                this.TagModes.Items.FindByValue("ModeNewsarticles").Selected = this.bModeNewsarticles;
            }

            if (!string.IsNullOrEmpty((string)this.Settings["ModeSimplegallery"]) && Utility.IsSimplyGalleryInstalled())
            {
                bool.TryParse((string)this.Settings["ModeSimplegallery"], out this.bModeSimplegallery);

                this.TagModes.Items.FindByValue("ModeSimplegallery").Selected = this.bModeSimplegallery;
            }

            if (!string.IsNullOrEmpty((string)this.Settings["ModeActiveforums"]) && Utility.IsActiveForumsInstalled())
            {
                bool.TryParse((string)this.Settings["ModeActiveforums"], out this.bModeActiveforums);

                this.TagModes.Items.FindByValue("ModeActiveforums").Selected = this.bModeActiveforums;
            }

            // Check if all false, set search
            if (!this.bModeCustom && !this.bModeTax && !this.bModeNewsarticles &&
                !this.bModeSimplegallery && !this.bModeActiveforums)
            {
                //this.TagModes.Items.FindByValue("ModeSearch").Selected = true;
                //this.bModeSearch = true;
            }

            if (this.bModeCustom)
            {
                this.phCustom.Visible = true;
            }

            var tabs = TabController.GetPortalTabs(this.PortalSettings.PortalId, -1, true, true);

            this.CustomSearchPage.Items.Clear();

            foreach (var tab in tabs)
            {
                this.CustomSearchPage.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
            }

            if (!string.IsNullOrEmpty((string)this.Settings["CustomSearchPage"]))
            {
                var searchItem = this.CustomSearchPage.Items.FindByValue((string)this.Settings["CustomSearchPage"]);

                if (searchItem != null)
                {
                    this.CustomSearchPage.Items.FindByValue((string)this.Settings["CustomSearchPage"]).Selected = true;
                }
            }
            else
            {
                var portal = new PortalController().GetPortal(PortalSettings.PortalId);

                var searchItem = this.CustomSearchPage.Items.FindByValue(portal.SearchTabId.ToString());

                if (searchItem != null)
                {
                    searchItem.Selected = true;
                }
            }

            if (!string.IsNullOrEmpty((string)this.Settings["SearchPageQueryString"]))
            {
                this.SearchPageQueryString.Text = (string)this.Settings["SearchPageQueryString"];
            }
            else
            {
                this.SearchPageQueryString.Text = "Search=";
            }

            if (!string.IsNullOrEmpty((string)this.Settings["SearchPageTaxQueryString"]))
            {
                this.SearchPageTaxQueryString.Text = (string)this.Settings["SearchPageTaxQueryString"];
            }
            else
            {
                this.SearchPageTaxQueryString.Text = "Tag=";
            }

            if (this.bModeNewsarticles && Utility.IsNewsArticlesInstalled())
            {
                this.phVentrianNews.Visible = true;

                this.FillTabList("newsarticles", tabs);
            }

            if (this.bModeSimplegallery && Utility.IsNewsArticlesInstalled())
            {
                this.phVentrianSimple.Visible = true;

                this.FillTabList("simplegallery", tabs);
            }

            if (this.bModeActiveforums && Utility.IsActiveForumsInstalled())
            {
                this.phActiveforums.Visible = true;

                this.FillTabList("activeforums", tabs);
            }

            if (this.bModeTax)
            {
                this.phTax.Visible = true;
            }

            // Setting TaxMode
            if (!string.IsNullOrEmpty((string)this.Settings["TaxMode"]))
            {
                try
                {
                    this.dDlTaxMode.SelectedValue = (string)this.Settings["TaxMode"];

                    if (this.dDlTaxMode.SelectedValue.Equals("custom"))
                    {
                        this.cBlVocabularies.Visible = true;
                        this.lblChooseVoc.Visible = true;
                    }
                }
                catch (Exception)
                {
                    this.dDlTaxMode.SelectedValue = "tab";
                }
            }
            else
            {
                this.dDlTaxMode.SelectedValue = "tab";
            }

            // Setting TaxMode Vocabularies
            if (!string.IsNullOrEmpty((string)this.Settings["TaxVocabularies"]))
            {
                string sVocabularies = (string)this.Settings["TaxVocabularies"];

                this.vocabularies = sVocabularies.Split(';');

                if (this.dDlTaxMode.SelectedValue.Equals("custom"))
                {
                    foreach (string sVocabulary in this.vocabularies.Where(sVocabulary => this.cBlVocabularies.Items.FindByValue(sVocabulary) != null))
                    {
                        this.cBlVocabularies.Items.FindByValue(sVocabulary).Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty((string)this.Settings["SkinName"]))
            {
                try
                {
                    this.dDlSkins.SelectedValue = (string)this.Settings["SkinName"];
                }
                catch (Exception)
                {
                    this.dDlSkins.SelectedValue = "Standard";
                }
            }
            else
            {
                this.dDlSkins.SelectedValue = "Standard";
            }

            // Setting OccurCount
            if (!string.IsNullOrEmpty((string)this.Settings["occurcount"]))
            {
                this.tbOccurCount.Text = (string)this.Settings["occurcount"];
            }
            else
            {
                this.tbOccurCount.Text = "1";
            }

            // Setting Tagscount
            if (!string.IsNullOrEmpty((string)this.Settings["tagscount"]))
            {
                this.tbTags.Text = (string)this.Settings["tagscount"];
            }
            else
            {
                this.tbTags.Text = "30";
            }

            this.ExclusionType.DataSource = Enum.GetNames(typeof(ExcludeType));
            this.ExclusionType.DataBind();

            // Setting Font Family
            if (!string.IsNullOrEmpty((string)this.Settings["FontFamily"]))
            {
                try
                {
                    this.FontFamily.Text = (string)this.Settings["FontFamily"];
                }
                catch (Exception)
                {
                    this.FontFamily.Text = "Georgia, Arial, sans-serif";
                }
            }
            else
            {
                this.FontFamily.Text = "Georgia, Arial, sans-serif";
            }

            this.CanvasWeightMode.Items.Clear();

            foreach (var weightMode in Enum.GetNames(typeof(WeightMode)))
            {
                this.CanvasWeightMode.Items.Add(new ListItem(Localization.GetString(string.Format("{0}.Text", weightMode), this.LocalResourceFile), weightMode));
            }

            // Setting Weight Mode
            if (!string.IsNullOrEmpty((string)this.Settings["WeightMode"]))
            {
                try
                {
                    this.CanvasWeightMode.SelectedValue = (string)this.Settings["WeightMode"];
                }
                catch (Exception)
                {
                    this.CanvasWeightMode.SelectedValue = WeightMode.size.ToString();
                }
            }
            else
            {
                this.CanvasWeightMode.SelectedValue = WeightMode.size.ToString();
            }

            this.RenderModeType.DataSource = Enum.GetNames(typeof(RenderMode));
            this.RenderModeType.DataBind();

            // Setting RenderMode Type
            if (!string.IsNullOrEmpty((string)this.Settings["RenderMode"]))
            {
                try
                {
                    this.RenderModeType.SelectedValue = (string)this.Settings["RenderMode"];
                }
                catch (Exception)
                {
                    this.RenderModeType.SelectedValue = RenderMode.HTML5.ToString();
                }
            }
            else
            {
                // Import old setting
                if (!string.IsNullOrEmpty((string)this.Settings["flashenabled"]))
                {
                    bool flashEnabled = bool.Parse((string)this.Settings["flashenabled"]);

                    this.Settings.Remove("flashenabled");

                    this.RenderModeType.SelectedValue = flashEnabled
                                                            ? RenderMode.Flash.ToString()
                                                            : RenderMode.BasicHTML.ToString();
                }
                else
                {
                    this.RenderModeType.SelectedValue = RenderMode.HTML5.ToString();
                }
            }

            this.ChangeDimensionInputsVisibility();

            // Setting Render Item Weight
            if (!string.IsNullOrEmpty((string)this.Settings["RenderItemWeight"]))
            {
                try
                {
                    this.RenderItemWeight.Checked = bool.Parse((string)this.Settings["RenderItemWeight"]);
                }
                catch (Exception)
                {
                    this.RenderItemWeight.Checked = false;
                }
            }
            else
            {
                this.RenderItemWeight.Checked = false;
            }

            // Setting Exclude Common
            if (!string.IsNullOrEmpty((string)this.Settings["ExcludeCommon"]))
            {
                try
                {
                    this.ExcludeCommon.Checked = bool.Parse((string)this.Settings["ExcludeCommon"]);
                }
                catch (Exception)
                {
                    this.ExcludeCommon.Checked = true;
                }
            }
            else
            {
                this.ExcludeCommon.Checked = true;
            }

            // Setting Exclude Words
            this.exlusionWords.AddRange(DataControl.TagCloudExcludeWordsGetByModule(this.TabModuleId));

            foreach (
                ListItem item in this.exlusionWords.Select(wordItem => new ListItem { Text = wordItem.Word, Value = wordItem.WordID.ToString() }))
            {
                this.lBExList.Items.Add(item);
            }

            this.SortTags.Items.Clear();

            foreach (var sortType in Enum.GetNames(typeof(SortType)))
            {
                this.SortTags.Items.Add(new ListItem(Localization.GetString(string.Format("{0}.Text", sortType), this.LocalResourceFile), sortType));
            }

            // Setting Sort Tags
            if (!string.IsNullOrEmpty((string)this.Settings["SortType"]))
            {
                try
                {
                    this.SortTags.SelectedValue = (string)this.Settings["SortType"];
                }
                catch (Exception)
                {
                    this.SortTags.SelectedValue = SortType.AlphabeticAsc.ToString();
                }
            }
            else
            {
                this.SortTags.SelectedValue = SortType.AlphabeticAsc.ToString();
            }

            // Setting Render as Ul
            if (!string.IsNullOrEmpty((string)this.Settings["RenderUl"]))
            {
                try
                {
                    this.cbRenderUl.Checked = bool.Parse((string)this.Settings["RenderUl"]);
                }
                catch (Exception)
                {
                    this.cbRenderUl.Checked = true;
                }
            }
            else
            {
                this.cbRenderUl.Checked = true;
            }

            // Setting Tags link
            if (!string.IsNullOrEmpty((string)this.Settings["tagslink"]))
            {
                try
                {
                    this.cbTagsLink.Checked = bool.Parse((string)this.Settings["tagslink"]);
                }
                catch (Exception)
                {
                    this.cbTagsLink.Checked = true;
                }
            }
            else
            {
                this.cbTagsLink.Checked = true;
            }

            // Setting Tags Url Visiblity
            if (!string.IsNullOrEmpty((string)this.Settings["tagslinkChk"]))
            {
                try
                {
                    this.cbTagsLinkChk.Checked = bool.Parse((string)this.Settings["tagslinkChk"]);
                }
                catch (Exception)
                {
                    this.cbTagsLinkChk.Checked = false;
                }
            }
            else
            {
                this.cbTagsLinkChk.Checked = false;
            }

            // Setting Cache Items
            if (!string.IsNullOrEmpty((string)this.Settings["CacheItems"]))
            {
                try
                {
                    this.cbCacheItems.Checked = bool.Parse((string)this.Settings["CacheItems"]);
                }
                catch (Exception)
                {
                    this.cbCacheItems.Checked = true;
                }
            }
            else
            {
                this.cbCacheItems.Checked = true;
            }

            // Setting Flash Width
            if (!string.IsNullOrEmpty((string)this.Settings["flashwidth"]))
            {
                this.tbFlashWidth.Text = (string)this.Settings["flashwidth"];
            }
            else
            {
                this.tbFlashWidth.Text = "500";
            }

            // Setting Flash Height
            if (!string.IsNullOrEmpty((string)this.Settings["flashheight"]))
            {
                this.tbFlashHeight.Text = (string)this.Settings["flashheight"];
            }
            else
            {
                this.tbFlashHeight.Text = "300";
            }

            // Setting Tags Cloud Width
            if (!string.IsNullOrEmpty((string)this.Settings["tagcloudwidth"]))
            {
                this.tbTagsCloudWidth.Text = (string)this.Settings["tagcloudwidth"];
            }
            else
            {
                this.tbTagsCloudWidth.Text = "500";
                this.dDlWidth.SelectedValue = "pixel";
            }

            if (this.tbTagsCloudWidth.Text.EndsWith("px"))
            {
                this.dDlWidth.SelectedValue = "pixel";
                this.tbTagsCloudWidth.Text =
                    this.tbTagsCloudWidth.Text.Replace(
                        this.tbTagsCloudWidth.Text.Substring(this.tbTagsCloudWidth.Text.IndexOf("px")), string.Empty);
            }
            else if (this.tbTagsCloudWidth.Text.EndsWith("%"))
            {
                this.dDlWidth.SelectedValue = "percent";
                this.tbTagsCloudWidth.Text =
                    this.tbTagsCloudWidth.Text.Replace(
                        this.tbTagsCloudWidth.Text.Substring(this.tbTagsCloudWidth.Text.IndexOf("%")), string.Empty);
            }

            // Setting Tags Cloud Height
            /*if (!string.IsNullOrEmpty(((string)TabModuleSettings["tagcloudheight"])))
            {
                tbTagsCloudHeight.Text = (string)TabModuleSettings["tagcloudheight"];
            }
            else
            {
                tbTagsCloudHeight.Text = "300";
                dDlHeight.SelectedValue = "pixel";
            }

            if (tbTagsCloudHeight.Text.EndsWith("px"))
            {
                dDlHeight.SelectedValue = "pixel";
                tbTagsCloudHeight.Text = tbTagsCloudHeight.Text.Replace(tbTagsCloudHeight.Text.Substring(tbTagsCloudHeight.Text.IndexOf("px")), "");
            }
            else if (tbTagsCloudHeight.Text.EndsWith("%"))
            {
                dDlHeight.SelectedValue = "percent";
                tbTagsCloudHeight.Text = tbTagsCloudHeight.Text.Replace(tbTagsCloudHeight.Text.Substring(tbTagsCloudHeight.Text.IndexOf("%")), "");
            }*/
            if (!string.IsNullOrEmpty((string)this.Settings["tcolor"]))
            {
                this.tbTcolor.Text = (string)this.Settings["tcolor"];
            }
            else
            {
                this.tbTcolor.Text = "000000";
            }

            if (!string.IsNullOrEmpty((string)this.Settings["tcolor2"]))
            {
                this.tbTcolor2.Text = (string)this.Settings["tcolor2"];
            }
            else
            {
                this.tbTcolor2.Text = "000000";
            }

            if (!string.IsNullOrEmpty((string)this.Settings["hicolor"]))
            {
                this.tbHicolor.Text = (string)this.Settings["hicolor"];
            }
            else
            {
                this.tbHicolor.Text = "42a5ff";
            }

            if (!string.IsNullOrEmpty((string)this.Settings["bgcolor"]))
            {
                this.tbBgcolor.Text = (string)this.Settings["bgcolor"];
            }
            else
            {
                this.tbBgcolor.Text = "transparent";
            }

            if (this.tbBgcolor.Text.ToLower().Equals("transparent"))
            {
                this.tbBgcolor.Text = string.Empty;
                this.tbBgcolor.Enabled = false;
                this.cbTransparent.Checked = true;
            }
            else
            {
                this.tbBgcolor.Enabled = true;
                this.cbTransparent.Checked = false;
            }

            if (!string.IsNullOrEmpty((string)this.Settings["tspeed"]))
            {
                this.tbTspeed.Text = (string)this.Settings["tspeed"];
            }
            else
            {
                this.tbTspeed.Text = "75";
            }

            // Word Cloud Settings
            this.Shape.Items.Clear();

            foreach (var shape in Enum.GetNames(typeof(WordCloudShape)))
            {
                this.Shape.Items.Add(new ListItem(shape, shape));
            }

            // Word Cloud Shape Setting
            if (!string.IsNullOrEmpty((string)Settings["Shape"]))
            {
                try
                {
                    this.Shape.SelectedValue = (string)Settings["Shape"];
                }
                catch (Exception)
                {
                    this.Shape.SelectedValue = WordCloudShape.circle.ToString();
                }
            }
            else
            {
                this.Shape.SelectedValue = WordCloudShape.circle.ToString();
            }

            // World Cloud GridSize Setting
            if (!string.IsNullOrEmpty((string)Settings["GridSize"]))
            {
                this.GridSize.Text = (string)Settings["GridSize"];
            }
            else
            {
                this.GridSize.Text = "8";
            }

            // World Cloud Ellipticity Setting
            if (!string.IsNullOrEmpty((string)Settings["Ellipticity"]))
            {
                this.Ellipticity.Text = (string)Settings["Ellipticity"];
            }
            else
            {
                this.Ellipticity.Text = "0.65";
            }

            // World Cloud WeightFactor Setting
            if (!string.IsNullOrEmpty((string)Settings["WeightFactor"]))
            {
                this.WeightFactor.Text = (string)Settings["WeightFactor"];
            }
            else
            {
                this.WeightFactor.Text = "2.1";
            }

            // World Cloud MinSize Setting
            if (!string.IsNullOrEmpty((string)Settings["MinSize"]))
            {
                this.MinSize.Text = (string)Settings["MinSize"];
            }
            else
            {
                this.MinSize.Text = "0";
            }

            // World Cloud FillBox Setting
            if (!string.IsNullOrEmpty((string)Settings["FillBox"]))
            {
                try
                {
                    this.FillBox.Checked = bool.Parse((string)Settings["FillBox"]);
                }
                catch (Exception)
                {
                    this.FillBox.Checked = false;
                }
            }
            else
            {
                this.FillBox.Checked = false;
            }
        }

        /// <summary>
        /// Save a Setting
        /// </summary>
        /// <param name="modController">
        /// The mod Controller.
        /// </param>
        /// <param name="name">
        /// Setting Name
        /// </param>
        /// <param name="value">
        /// Setting Value
        /// </param>
        private void SaveSetting(ModuleController modController, string name, string value)
        {
            modController.UpdateTabModuleSetting(this.TabModuleId, name, value);
        }

        /// <summary>
        /// Save the <see cref="Settings"/> to Database
        /// </summary>
        private void SaveChanges()
        {
            var modController = new ModuleController();

            // Setting TagSeparator
            this.SaveSetting(modController, "TagSeparator", this.txtTagSeparator.Text);

            /////////////////
            foreach (ListItem itemMode in this.TagModes.Items)
            {
                switch (itemMode.Value)
                {
                    case "ModeCustom":
                        {
                            this.bModeCustom = itemMode.Selected;
                        }

                        break;
                    case "ModeNewsarticles":
                        {
                            this.bModeNewsarticles = itemMode.Selected;
                        }

                        break;
                    case "ModeSimplegallery":
                        {
                            this.bModeSimplegallery = itemMode.Selected;
                        }

                        break;
                    case "ModeActiveforums":
                        {
                            this.bModeActiveforums = itemMode.Selected;
                        }

                        break;
                    case "ModeTax":
                        {
                            this.bModeTax = itemMode.Selected;
                        }

                        break;
                }
            }

            ////////////////

            // Setting TagMode
            if (this.bModeNewsarticles && this.ddLTabsVentrianNews.Items.Count.Equals(0))
            {
                this.bModeNewsarticles = false;
            }

            if (this.bModeSimplegallery && this.ddLTabsVentrianSimple.Items.Count.Equals(0))
            {
                this.bModeSimplegallery = false;
            }

            if (this.bModeActiveforums && this.ddLTabsActiveforums.Items.Count.Equals(0))
            {
                this.bModeActiveforums = false;
            }

            this.SaveSetting(modController, "ModeCustom", this.bModeCustom.ToString());
            this.SaveSetting(modController, "ModeTax", this.bModeTax.ToString());
            this.SaveSetting(modController, "ModeNewsarticles", this.bModeNewsarticles.ToString());
            this.SaveSetting(modController, "ModeSimplegallery", this.bModeSimplegallery.ToString());
            this.SaveSetting(modController, "ModeActiveforums", this.bModeActiveforums.ToString());

            // Setting TaxMode
            this.SaveSetting(modController, "TaxMode", this.dDlTaxMode.SelectedValue);

            // Setting TaxMode Vocabularies
            string sVocabularies = string.Empty;

            if (this.dDlTaxMode.SelectedValue.Equals("custom"))
            {
                sVocabularies =
                    this.cBlVocabularies.Items.Cast<ListItem>().Where(item => item.Selected).Aggregate(
                        sVocabularies, (current, item) => current + string.Format("{0};", item.Value));
            }

            if (sVocabularies.EndsWith(";"))
            {
                sVocabularies = sVocabularies.Remove(sVocabularies.Length - 1);
            }

            this.SaveSetting(modController, "TaxVocabularies", sVocabularies);

            // Setting SkinName
            this.SaveSetting(modController, "SkinName", this.dDlSkins.SelectedValue);

            if (Utility.IsNumeric(this.tbOccurCount.Text))
            {
                this.SaveSetting(modController, "occurcount", this.tbOccurCount.Text);
            }

            if (Utility.IsNumeric(this.tbTags.Text))
            {
                this.SaveSetting(modController, "tagscount", this.tbTags.Text);
            }

            this.SaveSetting(modController, "FontFamily", this.FontFamily.Text);

            this.SaveSetting(modController, "WeightMode", this.CanvasWeightMode.SelectedValue);

            this.SaveSetting(modController, "RenderMode", this.RenderModeType.SelectedValue);

            this.SaveSetting(modController, "RenderItemWeight", this.RenderItemWeight.Checked.ToString());

            this.SaveSetting(modController, "ExcludeCommon", this.ExcludeCommon.Checked.ToString());

            this.SaveSetting(modController, "SortType", this.SortTags.SelectedValue);

            this.SaveSetting(modController, "RenderUl", this.cbRenderUl.Checked.ToString());

            this.SaveSetting(modController, "tagslink", this.cbTagsLink.Checked.ToString());

            this.SaveSetting(modController, "tagslinkChk", this.cbTagsLinkChk.Checked.ToString());

            this.SaveSetting(modController, "CacheItems", this.cbCacheItems.Checked.ToString());

            this.SaveSetting(modController, "CustomSearchPage", this.CustomSearchPage.SelectedValue);

            this.SaveSetting(modController, "SearchPageQueryString", this.SearchPageQueryString.Text);

            this.SaveSetting(modController, "SearchPageTaxQueryString", this.SearchPageTaxQueryString.Text);

            if (!string.IsNullOrEmpty(this.tbFlashWidth.Text))
            {
                this.SaveSetting(modController, "flashwidth", this.tbFlashWidth.Text);
            }

            if (!string.IsNullOrEmpty(this.tbFlashHeight.Text))
            {
                this.SaveSetting(modController, "flashheight", this.tbFlashHeight.Text);
            }

            if (!string.IsNullOrEmpty(this.tbTagsCloudWidth.Text))
            {
                switch (this.dDlWidth.SelectedValue)
                {
                    case "pixel":
                        this.SaveSetting(modController, "tagcloudwidth", string.Format("{0}px", this.tbTagsCloudWidth.Text));
                        break;
                    case "percent":
                        this.SaveSetting(modController, "tagcloudwidth", string.Format("{0}%", this.tbTagsCloudWidth.Text));
                        break;
                }
            }

            /*if (!string.IsNullOrEmpty(tbTagsCloudHeight.Text))
            {
                if (dDlHeight.SelectedValue == "pixel")
                {
                    objModules.UpdateTabModuleSetting(TabModuleId, "tagcloudheight", string.Format("{0}px", tbTagsCloudHeight.Text));
                }
                else if (dDlWidth.SelectedValue == "percent")
                {
                    objModules.UpdateTabModuleSetting(TabModuleId, "tagcloudheight", string.Format("{0}%", tbTagsCloudHeight.Text));
                }
            }*/
            string sLogCacheKey = string.Format("CloudItems{0}", this.TabModuleId);

            if (DataCache.GetCache(sLogCacheKey) != null)
            {
                DataCache.RemoveCache(sLogCacheKey);
            }

            if (!string.IsNullOrEmpty(this.tbTcolor.Text))
            {
                this.SaveSetting(modController, "tcolor", this.tbTcolor.Text);
            }

            if (!string.IsNullOrEmpty(this.tbTcolor2.Text))
            {
                this.SaveSetting(modController, "tcolor2", this.tbTcolor2.Text);
            }

            if (!string.IsNullOrEmpty(this.tbHicolor.Text))
            {
                this.SaveSetting(modController, "hicolor", this.tbHicolor.Text);
            }

            if (this.cbTransparent.Checked)
            {
                this.SaveSetting(modController, "bgcolor", "transparent");
            }
            else
            {
                if (!string.IsNullOrEmpty(this.tbBgcolor.Text))
                {
                    this.SaveSetting(modController, "bgcolor", this.tbBgcolor.Text);
                }
            }

            if (!string.IsNullOrEmpty(this.tbTspeed.Text))
            {
                this.SaveSetting(modController, "tspeed", this.tbTspeed.Text);
            }

            // Save Ventrian Tab Setting
            if (this.bModeNewsarticles)
            {
                if (this.ddLTabsVentrianNews.Items.Count > 0)
                {
                    string[] values = this.ddLTabsVentrianNews.SelectedValue.Split(Convert.ToChar("-"));

                    if (values.Length == 2)
                    {
                        this.SaveSetting(modController, "NewsArticlesTab", values[0]);
                        this.SaveSetting(modController, "NewsArticlesModule", values[1]);
                    }
                }
            }

            if (this.bModeSimplegallery)
            {
                if (this.ddLTabsVentrianSimple.Items.Count > 0)
                {
                    string[] values = this.ddLTabsVentrianSimple.SelectedValue.Split(Convert.ToChar("-"));

                    if (values.Length == 2)
                    {
                        this.SaveSetting(modController, "SimpleGalleryTab", values[0]);
                        this.SaveSetting(modController, "SimpleGalleryModule", values[1]);
                    }
                }
            }

            this.SaveSetting(modController, "Shape", this.Shape.SelectedValue);
            this.SaveSetting(modController, "GridSize", this.GridSize.Text);
            this.SaveSetting(modController, "Ellipticity", this.Ellipticity.Text);
            this.SaveSetting(modController, "WeightFactor", this.WeightFactor.Text);
            this.SaveSetting(modController, "MinSize", this.MinSize.Text);
            this.SaveSetting(modController, "FillBox", this.FillBox.Checked.ToString());

            if (!this.bModeActiveforums)
            {
                return;
            }

            if (this.ddLTabsActiveforums.Items.Count > 0)
            {
                string[] values = this.ddLTabsActiveforums.SelectedValue.Split(Convert.ToChar("-"));

                if (values.Length == 2)
                {
                    this.SaveSetting(modController, "ActiveForumsTab", values[0]);
                    this.SaveSetting(modController, "ActiveForumsModule", values[1]);
                }
            }
        }

        /// <summary>
        /// Add new Tag
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void TagAddClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbCustomTag.Text))
            {
                string sTagName = this.tbCustomTag.Text;

                // Check if Tag is only one word
                if (sTagName.Contains(" "))
                {
                    sTagName.Remove(sTagName.IndexOf(" "));
                }

                CustomTags tag = new CustomTags
                    {
                        iWeight = int.Parse(this.dDlTagWeight.SelectedValue),
                        sTag = sTagName,
                        iModulId = this.ModuleId,
                        sUrl = this.ctlTagUrl.Url,
                        iTagId = DataControl.TagCloudItemsGetByModule(this.ModuleId).Count
                    };

                // Add to Sql
                try
                {
                    DataControl.TagCloudItemsAdd(tag);
                }
                catch (Exception)
                {
                    // Retrieve Highest Tag Id to Generate new TagId
                    var list = DataControl.TagCloudItemsGetByModule(this.ModuleId);

                    Utility.SortDescending(list, item => item.iTagId);

                    var itemHighest = list.FirstOrDefault();

                    tag.iTagId = itemHighest.iTagId + 1;

                    DataControl.TagCloudItemsAdd(tag);
                }
            }

            this.tbCustomTag.Text = string.Empty;
            this.lTagId.Text = string.Empty;
            this.lTagLocale.Text = string.Empty;

            this.FillCustomTags();

            this.AddJavaScript(1);
        }

        /// <summary>
        /// Localize Tag
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void TagLocalizeClick(object sender, EventArgs e)
        {
            this.FillLocalizedTag(this.tbCustomTag.Text, this.ctlTagUrl.Url);

            this.iBTagLocalize.Visible = false;

            this.AddJavaScript(1);
        }

        /// <summary>
        /// Changing Tag Mode and Hide/Show Custom Tag List
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void TagModesSelectedIndexChanged(object sender, EventArgs e)
        {
            List<TabInfo> tabs = TabController.GetPortalTabs(this.PortalSettings.PortalId, -1, true, true);

            var panelOpenSelector = string.Empty;

            foreach (ListItem itemMode in this.TagModes.Items)
            {
                switch (itemMode.Value)
                {
                    case "ModeCustom":
                        {
                            if (itemMode.Selected)
                            {
                                this.bModeCustom = true;
                                this.phCustom.Visible = true;

                                // Expand Panel
                               panelOpenSelector = "TagSrcCustomPanel";
                            }
                            else
                            {
                                this.bModeCustom = false;
                                this.phCustom.Visible = false;
                            }
                        }

                        break;
                    case "ModeNewsarticles":
                        {
                            if (itemMode.Selected)
                            {
                                this.bModeNewsarticles = true;
                                this.phVentrianNews.Visible = true;

                                this.FillTabList("newsarticles", tabs);

                                // Expand Panel
                                panelOpenSelector = "VentrianNewsOptionsPanel";
                            }
                            else
                            {
                                this.bModeNewsarticles = false;
                                this.phVentrianNews.Visible = false;
                            }
                        }

                        break;
                    case "ModeSimplegallery":
                        {
                            if (itemMode.Selected)
                            {
                                this.bModeSimplegallery = true;
                                this.phVentrianSimple.Visible = true;

                                this.FillTabList("simplegallery", tabs);

                                // Expand Panel
                                panelOpenSelector = "VentrianSimpleOptionsPanel";
                            }
                            else
                            {
                                this.bModeSimplegallery = false;
                                this.phVentrianSimple.Visible = false;
                            }
                        }

                        break;
                    case "ModeActiveforums":
                        {
                            if (itemMode.Selected)
                            {
                                this.bModeActiveforums = true;
                                this.phActiveforums.Visible = true;

                                this.FillTabList("activeforums", tabs);

                                // Expand Panel
                                panelOpenSelector = "ActiveForumsOptionsPanel";
                            }
                            else
                            {
                                this.bModeActiveforums = false;
                                this.phActiveforums.Visible = false;
                            }
                        }

                        break;
                    case "ModeTax":
                        {
                            if (itemMode.Selected)
                            {
                                this.bModeTax = true;
                                this.phTax.Visible = true;

                                // Expand Panel
                                panelOpenSelector = "TaxOptionPanel";
                            }
                            else
                            {
                                this.bModeTax = false;
                                this.phTax.Visible = false;
                            }
                        }

                        break;
                }
            }

            this.AddJavaScript(1, panelOpenSelector);
        }

        /// <summary>
        /// Changing Tag Mode and Hide/Show Custom Tag List
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void RenderModeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeDimensionInputsVisibility();

            this.AddJavaScript(0);
        }

        /// <summary>
        /// Save Tag
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void TagSaveClick(object sender, EventArgs e)
        {
            CustomTags tag = null;

            if (!string.IsNullOrEmpty(this.tbCustomTag.Text) && !string.IsNullOrEmpty(this.lTagId.Text))
            {
                string sTagName = this.tbCustomTag.Text;

                // Check if Tag is only one word
                if (sTagName.Contains(" "))
                {
                    sTagName.Remove(sTagName.IndexOf(" "));
                }

                tag = new CustomTags
                    {
                        iWeight = int.Parse(this.dDlTagWeight.SelectedValue),
                        sTag = sTagName,
                        iModulId = this.ModuleId,
                        sUrl = this.ctlTagUrl.Url,
                        iTagId = int.Parse(this.lTagId.Text)
                    };
            }

            if (!string.IsNullOrEmpty(this.lTagLocale.Text))
            {
                if (tag != null)
                {
                    DataControl.TagCloudItemsDeleteMl(tag.iTagId, tag.iModulId, this.lTagLocale.Text);

                    DataControl.TagCloudItemsAddMl(
                        tag.iTagId, this.lTagLocale.Text, this.tbCustomTag.Text, tag.iModulId, this.ctlTagUrl.Url);
                }

                this.FillLocalizedTag(string.Empty, string.Empty);
            }
            else
            {
                // Update Sql
                DataControl.TagCloudItemsUpdate(tag);

                this.grdTagLocales.Visible = false;

                this.FillCustomTags();

                this.iBTagAdd.Visible = true;
            }

            this.tbCustomTag.Text = string.Empty;
            this.lTagId.Text = string.Empty;
            this.lTagLocale.Text = string.Empty;

            this.iBTagSave.Visible = false;
            this.iBTagLocalize.Visible = false;
            this.iBTagEditCancel.Visible = false;

            this.AddJavaScript(1);
        }

        /// <summary>
        /// Cancel Edit Tag
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TagEditCancelClick(object sender, EventArgs e)
        {
            this.tbCustomTag.Text = string.Empty;
            this.lTagId.Text = string.Empty;
            this.lTagLocale.Text = string.Empty;

            this.iBTagSave.Visible = false;
            this.iBTagLocalize.Visible = false;
            this.iBTagEditCancel.Visible = false;

            this.grdTagLocales.Visible = false;

            this.AddJavaScript(1);
        }

        /// <summary>
        /// Changes the dimension inputs visibility
        /// based on the selected Render Mode
        /// </summary>
        private void ChangeDimensionInputsVisibility()
        {
            var selectedMode = (RenderMode)Enum.Parse(typeof(RenderMode), this.RenderModeType.SelectedValue);

            if (selectedMode.Equals(RenderMode.BasicHTML))
            {
                this.lblFlashWidth.Visible = false;
                this.lblFlashHeight.Visible = false;
                this.tbFlashWidth.Visible = false;
                this.tbFlashHeight.Visible = false;

                this.lblTagsCloudWidth.Visible = true;
                this.lblTagsCloudHeight.Visible = true;
                this.tbTagsCloudWidth.Visible = true;
                this.tbTagsCloudHeight.Visible = true;
                this.dDlWidth.Visible = true;
            }
            else
            {
                this.lblFlashWidth.Visible = true;
                this.lblFlashHeight.Visible = true;
                this.tbFlashWidth.Visible = true;
                this.tbFlashHeight.Visible = true;

                this.lblTagsCloudWidth.Visible = false;
                this.lblTagsCloudHeight.Visible = false;
                this.tbTagsCloudWidth.Visible = false;
                this.tbTagsCloudHeight.Visible = false;
                this.dDlWidth.Visible = false;
            }
        }

        #endregion
    }
}