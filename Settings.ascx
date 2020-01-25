<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.FBSecurity.Settings" %>
	
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>

<div class="dnnForm" id="form-settings">

    <fieldset>

<dnn:sectionhead id="sectGeneralSettings" cssclass="Head" runat="server" text="General Settings" section="GeneralSection"
	includerule="False" isexpanded="True"></dnn:sectionhead>

<div id="GeneralSection" runat="server">   
            


	<div class="dnnFormItem">
    
        <dnn:label id="lblRemoteUserRole" runat="server" controlname="ddlRemoteUserRole" suffix=":" />
        <asp:DropDownList ID="ddlRemoteUserRole" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>

    	<div class="dnnFormItem">
        <dnn:label id="lblAllowedIPAddress" runat="server" controlname="txtAllowedIPAddress" suffix=":" />
        <asp:TextBox ID="txtAllowedIPAddress" cssclass="NormalTextBox" runat="server" />
	</div>


    <div class="dnnFormItem">					
        <dnn:label id="lblShowResult" runat="server" controlname="cbxShowResult" suffix=":" />
        <asp:CheckBox ID="cbxShowResult" runat="server" />
    </div>

	<div class="dnnFormItem">
        <dnn:label id="lblRedirectPage" runat="server" controlname="ddlPageList" suffix=":" />
        <asp:DropDownList ID="ddlPageList" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />
	</div>
	
    <div class="dnnFormItem">					
        <dnn:label id="lblEnableRedirect" runat="server" controlname="cbxEnableRedirect" suffix=":" />
        <asp:CheckBox ID="cbxEnableRedirect" runat="server" />
    </div>


    <div class="dnnFormItem">					
        <dnn:label id="lblFlagForReviewNotify" runat="server" controlname="txtFlagForReviewNotify" suffix=":" />
        <asp:textbox id="txtFlagForReviewNotify" runat="server" />
    </div>


 </div>
        
      			


    </fieldset>

</div>	