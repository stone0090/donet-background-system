<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcBcGroupDetail.aspx.cs" Inherits="stonefw.Web.BaseModule.BcGroup.BcGroupDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdGroupId" />
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>组别名称：</td>
                    <td>
                        <asp:TextBox ID="txtGroupName" runat="server" MaxLength="25"
                            class="easyui-textbox" data-options="required:true"></asp:TextBox></td>
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
