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
    public class FBSecuritySettings
    {
        ModuleController controller;
        int tabModuleId;

        public FBSecuritySettings(int tabModuleId)
        {
            controller = new ModuleController();
            this.tabModuleId = tabModuleId;
        }

        protected T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = controller.GetTabModuleSettings(this.tabModuleId);

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        protected void WriteSetting(string settingName, string value)
        {
            controller.UpdateTabModuleSetting(this.tabModuleId, settingName, value);
        }

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        public string RemoteUserRole
        {
            get { return ReadSetting<string>("remoteUserRole", null); }
            set { WriteSetting("remoteUserRole", value); }
        }
        
        public string AllowedIPAddress
        {
            get { return ReadSetting<string>("allowedIPAddress", null); }
            set { WriteSetting("allowedIPAddress", value); }
        }

        public string ShowResult
        {
            get { return ReadSetting<string>("showResult", null); }
            set { WriteSetting("showResult", value); }
        }

        public string RedirectPage
        {
            get { return ReadSetting<string>("redirectPage", null); }
            set { WriteSetting("redirectPage", value); }
        }

        //EnableRedirect
        public string EnableRedirect
        {
            get { return ReadSetting<string>("enableRedirect", null); }
            set { WriteSetting("enableRedirect", value); }
        }

        public string FlagForReviewNotify
        {

            get { return ReadSetting<string>("flagForReviewNotify", null); }
            set { WriteSetting("flagForReviewNotify", value); }
        }

        #endregion
    }
}
