using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.FBSecurity.Components;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;

namespace GIBS.Modules.FBSecurity
{
    public partial class Settings : ModuleSettingsBase
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {

                    GetRoles();
                    GetPageList();

                    FBSecuritySettings settingsData = new FBSecuritySettings(this.TabModuleId);

                    if (settingsData.RemoteUserRole != null)
                    {
                        ddlRemoteUserRole.SelectedValue = settingsData.RemoteUserRole;
                    }

                    if (settingsData.AllowedIPAddress != null)
                    {
                        txtAllowedIPAddress.Text = settingsData.AllowedIPAddress;
                    }
                    
                    if (settingsData.ShowResult != null)
                    {
                        cbxShowResult.Checked = Convert.ToBoolean(settingsData.ShowResult);
                    }

                    if (settingsData.RedirectPage != null)
                    {
                        ddlPageList.SelectedValue = settingsData.RedirectPage;
                    }
                    if (settingsData.EnableRedirect != null)
                    {
                        cbxEnableRedirect.Checked = Convert.ToBoolean(settingsData.EnableRedirect);
                    }

                    //txtFlagForReviewNotify
                    if (settingsData.FlagForReviewNotify != null)
                    {
                        txtFlagForReviewNotify.Text = settingsData.FlagForReviewNotify;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void GetPageList()
        {
            try
            {
                ddlPageList.DataSource = TabController.GetPortalTabs(this.PortalId, -1, true, "None Specified", true, false, true, true, true);

                TabController.GetPortalTabs(this.PortalId, 1, false, true);

                ddlPageList.DataBind();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }					
			

        public void GetRoles()
        {
            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
            var myRoles = rc.GetRoles(this.PortalId);

            // REPORTS ROLE
            ddlRemoteUserRole.DataSource = myRoles;
            ddlRemoteUserRole.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem item1 = new ListItem();
            item1.Text = "-- Select Role to Allow Remote Access --";
            item1.Value = "";
            ddlRemoteUserRole.Items.Insert(0, item1);
            // REMOVE DEFAULT ROLES
            ddlRemoteUserRole.Items.Remove("Administrators");
            ddlRemoteUserRole.Items.Remove("Registered Users");
            ddlRemoteUserRole.Items.Remove("Subscribers");
            
        }

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                FBSecuritySettings settingsData = new FBSecuritySettings(this.TabModuleId);
                settingsData.RemoteUserRole = ddlRemoteUserRole.SelectedValue.ToString();
                settingsData.ShowResult = cbxShowResult.Checked.ToString();
                settingsData.AllowedIPAddress = txtAllowedIPAddress.Text.ToString();
                settingsData.RedirectPage = ddlPageList.SelectedValue.ToString();
                settingsData.EnableRedirect = cbxEnableRedirect.Checked.ToString();
                settingsData.FlagForReviewNotify = txtFlagForReviewNotify.Text.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}