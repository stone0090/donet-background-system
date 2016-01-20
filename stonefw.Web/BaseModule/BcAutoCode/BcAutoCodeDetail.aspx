<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcAutoCodeDetail.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcAutoCode.BcAutoCodeDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script>
        $(function() {
            showResult();
            $('#txtPrefix').textbox({ onChange: function() { showResult(); } });
        });

        function showResult() {
            if (!$.isEmpty($('#txtPrefix').val())) {
                var prefix = $('#txtPrefix').val().toUpperCase();
                var digit = parseInt($('#txtDigit').val());
                $('#txtResult').textbox('setValue', prefix + '140101' + '00000000000000'.substr(0, digit - 1) + '1');
            }
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hdId"/>
    <div class="form">
        <table cellpadding="5">
            <tr>
                <td>功能点：</td>
                <td>
                    <asp:DropDownList ID="ddlFuncPoint" runat="server" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>前缀：</td>
                <td>
                    <asp:TextBox ID="txtPrefix" runat="server" MaxLength="5" class="easyui-textbox" data-options="required:true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>日期格式：</td>
                <td>
                    <asp:TextBox ID="txtDateFormat" runat="server" MaxLength="10" class="easyui-textbox" Enabled="False" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>位数：</td>
                <td>
                    <asp:TextBox ID="txtDigit" runat="server" class="easyui-textbox" Enabled="False" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>效果展示：</td>
                <td>
                    <asp:TextBox ID="txtResult" runat="server" class="easyui-textbox" Enabled="False" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>是否默认：</td>
                <td>
                    <asp:RadioButton ID="rYes" runat="server" Text="是" GroupName="isDefault" Checked="True"/>
                    <asp:RadioButton ID="rNo" runat="server" Text="否" GroupName="isDefault"/>
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