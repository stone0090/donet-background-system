<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysPageFuncPointDetail.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysPageFuncPoint.SysPageFuncPointDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdPageUrl" />
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>FuncPointId：</td>
                    <td>
                        <asp:DropDownList ID="ddlFuncPointId" runat="server" Width="335"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>PageUrl：</td>
                    <td>
                        <asp:TextBox ID="txtPageUrl" runat="server" MaxLength="250" Width="330"></asp:TextBox>
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
