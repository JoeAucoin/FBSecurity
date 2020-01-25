using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using GIBS.FBSecurity.Components;
using System.Web;
using System.Text;
using DotNetNuke.Services.Social.Notifications;
using DotNetNuke.Entities.Users;

namespace GIBS.Modules.FBSecurity
{
    public partial class ViewFBSecurity : PortalModuleBase, IActionable
    {

        static string _AllowedIPAddress = "";
        static string _RemoteUserRole = "";
        static string _FlagForReviewNotify = "";
        static string _RedirectPage = "";
        static bool _EnableRedirect = false;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    LoadSettings();

                    if (CheckIPAddress(_AllowedIPAddress.ToString()))
                    {
                        lblDebug.Text = "We have a match - <br />User IP: " + HttpContext.Current.Request.UserHostAddress.ToString() +
                            "<br />Allowed IP: " + _AllowedIPAddress.ToString();

                    }
                    else
                    {
                        lblDebug.Text = "NO MATCH - <br />User IP: " + HttpContext.Current.Request.UserHostAddress.ToString() +
                            "<br />Allowed IP: " + _AllowedIPAddress.ToString() +
                            "<br />Checking on Role group . . .";

                        if (UserInfo.IsInRole(_RemoteUserRole))
                        {
                            lblDebug.Text += "<br />GOOD GUY - User is member of allowed role group!";

                        }
                        else
                        {

                            lblDebug.Text += "<br />UNAUTHORIZED REMOTE ACCESS DETECTED";
                            if (_FlagForReviewNotify.ToString().Length >= 1)
                            {
                                lblDebug.Text += "<br />Sending Email Alert";
                                EmailNotificationHTML(BuildEmailContent(this.UserInfo.FirstName + " " + this.UserInfo.LastName
                                    + " attempted to access the application from a remote location.<br /><br />User ID: " 
                                    + this.UserId.ToString()
                                    + "<br />IP Address: " + HttpContext.Current.Request.UserHostAddress.ToString()
                                    + "<br />" + DateTime.Now.ToString() + "<br />" +
                                    DotNetNuke.Entities.Tabs.TabController.CurrentPage.TabName.ToString() + "<br />" 
                                    + HttpContext.Current.Request.Url.ToString()), "Remote Access Attempt from " + this.UserInfo.FirstName + " " + this.UserInfo.LastName);
                            }

                            if (_EnableRedirect)
                            {
                                string RedirectURL = "http://" + Request.Url.Host + "/TabID/" + _RedirectPage.ToString() + "/Default.aspx";
                                Response.Redirect(RedirectURL.ToString(), true);
                            }

                           // DotNetNuke.Entities.Tabs.TabController.CurrentPage.TabName.ToString()
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public bool CheckIPAddress(string _IPAddress)
        {
            try
            {


                if (HttpContext.Current.Request.UserHostAddress.ToString() == _IPAddress)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return false;
            }
        }	


        public void LoadSettings()
        {
            try
            {


                FBSecuritySettings settingsData = new FBSecuritySettings(this.TabModuleId);


                if (settingsData.AllowedIPAddress != null)
                {
                    _AllowedIPAddress = settingsData.AllowedIPAddress;
                }

                if (settingsData.ShowResult != null)
                {
                    lblDebug.Visible = Convert.ToBoolean(settingsData.ShowResult);
                }
                if (settingsData.RemoteUserRole != null)
                {
                    _RemoteUserRole = settingsData.RemoteUserRole;
                }

                if (settingsData.RedirectPage != null)
                {
                    _RedirectPage = settingsData.RedirectPage.ToString();
                }

                if (settingsData.EnableRedirect != null)
                {
                    _EnableRedirect = Convert.ToBoolean(settingsData.EnableRedirect);
                }


                //_FlagForReviewNotify
                if (settingsData.FlagForReviewNotify != null)
                {
                    _FlagForReviewNotify = settingsData.FlagForReviewNotify.ToString();
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }



        public void EmailNotificationHTML(string content, string subject)
        {

            try
            {

                string _subject = subject.ToString();
                string body = content.ToString();


                if (_FlagForReviewNotify.ToString().Length > 1)
                {

                    string[] valuePair = _FlagForReviewNotify.Split(new char[] { ',' });

                    for (int i = 0; i <= valuePair.Length - 1; i++)
                    {
                        UserInfo _currentUser = DotNetNuke.Entities.Users.UserController.GetUserById(this.PortalId, Int32.Parse(valuePair[i].ToString().Trim()));
                        var notificationType = NotificationsController.Instance.GetNotificationType("HtmlNotification");
                        // NEED THE PORTALID HERE AND AGENTID
                        var sender = UserController.GetUserById(this.PortalId, this.UserId);
                        var notification = new Notification { NotificationTypeID = notificationType.NotificationTypeId, Subject = subject, Body = body, IncludeDismissAction = true, SenderUserID = sender.UserID };

                        NotificationsController.Instance.SendNotification(notification, this.PortalId, null, new List<UserInfo> { _currentUser });

                        
                    }

                    //RESET VALUE TO PREVENT MULTIPLE NOTIFICATIONS
                    _FlagForReviewNotify = "";

                }


           
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }

        }

        public string BuildEmailContent(string ClientName)
        {
            try
            {


                StringBuilder EmailContentHTML = new StringBuilder();
                EmailContentHTML.Capacity = 5000;


                EmailContentHTML.Append("<style type=\"text/css\">" + Environment.NewLine);
                EmailContentHTML.Append(".Section{font-weight: bold; font-family: Verdana, Tahoma;font-size: 14px;}" + Environment.NewLine);
                EmailContentHTML.Append(".Value{font-weight: normal; font-family: Verdana, Tahoma;font-size: 12px;}" + Environment.NewLine);
                EmailContentHTML.Append(".Footer{font-weight: normal; font-family: Verdana, Tahoma;font-size: 10px;line-height:150%;}" + Environment.NewLine);
                EmailContentHTML.Append("</style>" + Environment.NewLine + Environment.NewLine);



                EmailContentHTML.Append(Environment.NewLine);
                EmailContentHTML.Append("<p class=\"Section\">SECURITY ALERT!</p>" + Environment.NewLine);
                EmailContentHTML.Append("<p class=\"Value\">" + ClientName.ToString() + "</p>" + Environment.NewLine);


                EmailContentHTML.Append("<p class=\"Value\"><a href=\"http://ip-address-lookup-v4.com/ip/" + HttpContext.Current.Request.UserHostAddress.ToString() + "\" target=\"_blank\">CLICK HERE TO VIEW IP LOCATION</a></p>" + Environment.NewLine);

             //   EmailContentHTML.Append("<p class=\"Footer\">Some e-mail clients do not support links, cut 'n paste the URL below into a web browser.<br />http://" + PortalSettings.PortalAlias.HTTPAlias.ToString() + System.Web.HttpContext.Current.Request.RawUrl.ToString() + "</p>" + Environment.NewLine);
                // EmailContentHTML.Append("</tr>" + Environment.NewLine);




                return EmailContentHTML.ToString();
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                return "ERROR: " + ex.ToString();
            }
        }




        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                //    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                //     true, false);

                return actions;
            }
        }

        #endregion


        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


    }
}