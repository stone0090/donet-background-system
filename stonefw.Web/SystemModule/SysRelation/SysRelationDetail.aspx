<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysRelationDetail.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysRelation.SysRelationDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div class="form">
        <table cellpadding="5">
            <tr>
                <td>模块编号：</td>
                <td>

                    <asp:DropDownList ID="ddlModule" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>功能点编号：</td>
                <td>

                    <asp:DropDownList ID="ddlFuncPoint" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>包含权限点：</td>
                <td>
                    <asp:CheckBoxList ID="cblPermission" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"></asp:CheckBoxList>
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