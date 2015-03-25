<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcUserRoleDetail.aspx.cs" Inherits="stonefw.Web.BaseModule.BcUserRole.BcUserRoleDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdUserId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdRoleId" runat="server"></asp:HiddenField>
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>用户：</td>
                    <td>
                        <asp:DropDownList ID="ddlUser" runat="server" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>角色：</td>
                    <td>
                        <asp:DropDownList ID="ddlRole" runat="server" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false"></asp:DropDownList></td>
                </tr>
            </table>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
            <div>
                <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClientClick="return saveForm(this);" OnClick="btnSave_Click">保存</asp:LinkButton>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="window.parent.closeDialog();">取消</a>
            </div>
        </div>
    </form>
</body>
</html>
