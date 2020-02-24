using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.FBSecurity.Components;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;

namespace GIBS.Modules.FBSecurity
{
    public partial class Settings : FBSecuritySettings
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

                    //        FBSecuritySettings settingsData = new FBSecuritySettings(this.TabModuleId);

                    if (Settings.Contains("remoteUserRole"))
                    {
                        ddlRemoteUserRole.SelectedValue = RemoteUserRole.ToString();
                    }

                    if (Settings.Contains("allowedIPAddress"))
                    {
                        txtAllowedIPAddress.Text = AllowedIPAddress.ToString();
                    }

                    if (Settings.Contains("showResult"))
                    {
                        cbxShowResult.Checked = Convert.ToBoolean(ShowResult.ToString());
                    }

                    if (Settings.Contains("redirectPage"))
                    {
                        ddlPageList.SelectedValue = RedirectPage.ToString();
                    }
                    if (Settings.Contains("enableRedirect"))
                    {
                        cbxEnableRedirect.Checked = Convert.ToBoolean(EnableRedirect.ToString());
                    }

                    //txtFlagForReviewNotify
                    if (Settings.Contains("flagForReviewNotify"))
                    {
                        txtFlagForReviewNotify.Text = FlagForReviewNotify.ToString();
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
              //  FBSecuritySettings settingsData = new FBSecuritySettings(this.TabModuleId);
                RemoteUserRole = ddlRemoteUserRole.SelectedValue.ToString();
                ShowResult = cbxShowResult.Checked.ToString();
                AllowedIPAddress = txtAllowedIPAddress.Text.ToString();
                RedirectPage = ddlPageList.SelectedValue.ToString();
                EnableRedirect = cbxEnableRedirect.Checked.ToString();
                FlagForReviewNotify = txtFlagForReviewNotify.Text.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}