using System;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.FBSecurity.Components;

namespace GIBS.Modules.FBSecurity
{
    public partial class EditFBSecurity : PortalModuleBase
    {

        int itemId = Null.NullInteger;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ItemId"] != null)
                {
                    itemId = Int32.Parse(Request.QueryString["ItemId"]);
                }

                if (!IsPostBack)
                {
                    //load the data into the control the first time
                    //we hit this page


                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                    //check we have an item to lookup
                    if (!Null.IsNull(itemId))
                    {
                        //load the item
                        FBSecurityController controller = new FBSecurityController();
                        FBSecurityInfo item = controller.GetFBSecurity(this.ModuleId, itemId);

                        if (item != null)
                        {
                            txtContent.Text = item.Content;
                            ctlAudit.CreatedByUser = item.CreatedByUserName;
                            ctlAudit.CreatedDate = item.CreatedDate.ToLongDateString();
                        }
                        else
                            Response.Redirect(Globals.NavigateURL(), true);
                    }
                    else
                    {
                        cmdDelete.Visible = false;
                        ctlAudit.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FBSecurityController controller = new FBSecurityController();
                FBSecurityInfo item = new FBSecurityInfo();

                item.Content = txtContent.Text;
                item.ItemId = itemId;
                item.ModuleId = this.ModuleId;
                item.CreatedByUser = this.UserId;

                //determine if we are adding or updating
                if (Null.IsNull(item.ItemId))
                    controller.AddFBSecurity(item);
                else
                    controller.UpdateFBSecurity(item);

                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Null.IsNull(itemId))
                {
                    FBSecurityController controller = new FBSecurityController();
                    controller.DeleteFBSecurity(this.ModuleId, itemId);
                    Response.Redirect(Globals.NavigateURL(), true);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}