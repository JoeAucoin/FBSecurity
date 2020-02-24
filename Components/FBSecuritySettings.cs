using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;

namespace GIBS.FBSecurity.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class FBSecuritySettings : ModuleSettingsBase
    {
        

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        public string FlagForReviewNotify
        {
            get
            {
                if (Settings.Contains("flagForReviewNotify"))
                    return Settings["flagForReviewNotify"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "flagForReviewNotify", value.ToString());
            }
        }



        public string RemoteUserRole
        {
            get
            {
                if (Settings.Contains("remoteUserRole"))
                    return Settings["remoteUserRole"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "remoteUserRole", value.ToString());
            }
        }


        public string AllowedIPAddress
        {
            get
            {
                if (Settings.Contains("allowedIPAddress"))
                    return Settings["allowedIPAddress"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "allowedIPAddress", value.ToString());
            }
        }


        public string ShowResult
        {
            get
            {
                if (Settings.Contains("showResult"))
                    return Settings["showResult"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "showResult", value.ToString());
            }
        }


        public string RedirectPage
        {
            get
            {
                if (Settings.Contains("redirectPage"))
                    return Settings["redirectPage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "redirectPage", value.ToString());
            }
        }


        public string EnableRedirect
        {
            get
            {
                if (Settings.Contains("enableRedirect"))
                    return Settings["enableRedirect"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "enableRedirect", value.ToString());
            }
        }

        #endregion
    }
}
