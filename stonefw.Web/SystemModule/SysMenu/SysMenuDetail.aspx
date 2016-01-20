<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysMenuDetail.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysMenu.SysMenuDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>菜单详情</title>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hdMenuId"/>
    <asp:HiddenField runat="server" ID="hdFatherNode"/>
    <div class="form">
        <table cellpadding="5">
            <tr>
                <td>所属模块：</td>
                <td>
                    <asp:TextBox ID="txtModule" runat="server" MaxLength="25" Enabled="False"></asp:TextBox>
                </td>
                <td>所属功能：</td>
                <td>
                    <asp:DropDownList ID="ddlFuncPoint" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFuncPoint_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>父级菜单：</td>
                <td>
                    <asp:DropDownList ID="ddlMenuTree" runat="server"></asp:DropDownList>
                </td>
                <td>菜单名称：</td>
                <td>
                    <asp:TextBox ID="txtMenuName" runat="server" MaxLength="25" class="easyui-validatebox" data-options="required:true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>菜单链接：</td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPageUrl" runat="server" Width="100%"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>菜单参数：</td>
                <td colspan="3">
                    <asp:TextBox ID="txtUrlParameter" runat="server" Width="100%" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>菜单说明：</td>
                <td colspan="3">
                    <asp:TextBox ID="txtMenuDescription" runat="server" Width="100%" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>菜单状态：</td>
                <td colspan="3">
                    <asp:CheckBox runat="server" ID="cbMenuStatus" Text="启用" Checked="True"/>
                </td>
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