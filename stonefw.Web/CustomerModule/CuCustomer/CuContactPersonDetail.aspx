<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuContactPersonDetail.aspx.cs" Inherits="stonefw.Web.CustomerModule.CuCustomer.CuContactPersonDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdCpId" />
        <asp:HiddenField runat="server" ID="hdCuId" />
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>姓名：</td>
                    <td>
                        <asp:TextBox ID="txtCpName" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>手机：</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>电话：</td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="25" class="easyui-textbox" data-options="required:true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>是否默认：</td>
                    <td>
                        <asp:RadioButton ID="rYes" runat="server" Text="是" GroupName="isDefault" Checked="True" />
                        <asp:RadioButton ID="rNo" runat="server" Text="否" GroupName="isDefault" />
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
