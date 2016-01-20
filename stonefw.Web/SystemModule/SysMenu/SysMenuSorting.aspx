<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysMenuSorting.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysMenu.SysMenuSorting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>位置调整</title>
    <script>
        function up() {
            var index = $("#lbMenuTree").get(0).selectedIndex;
            if (index == -1 || index == 0)
                return;
            index = index - 1;
            $("#lbMenuTree option:eq(" + index + ")").before($("#lbMenuTree option:selected").get(0));
            recalSeq();
        }

        function down() {
            var index = $("#lbMenuTree").get(0).selectedIndex;
            var length = $("#lbMenuTree").get(0).length;
            if (index == -1 || index == length)
                return;
            index = index + 1;
            $("#lbMenuTree option:eq(" + index + ")").after($("#lbMenuTree option:selected").get(0));
            recalSeq();
        }

        function recalSeq() {
            var value = "";
            $("#lbMenuTree option").each(function() {
                value += "|" + $(this).val();
            });
            value = value.substring(1);
            $("#hdSeqValue").val(value);
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hdSeqValue"/>
    <div>
        <div class="form">
            <table cellpadding="5">
                <tr>
                    <td>菜单列表：</td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlMenuTree" Width="265" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuTree_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>位置调整：</td>
                    <td>
                        <asp:ListBox ID="lbMenuTree" Width="265" Height="170" runat="server"></asp:ListBox>
                    </td>
                    <td>
                        <a class="easyui-linkbutton" href="#" onclick="up();">上移</a>
                        <br/>
                        <br/>
                        <br/>
                        <a class="easyui-linkbutton" href="#" onclick="down();">下移</a>
                    </td>
                </tr>
            </table>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
            <div>
                <asp:LinkButton ID="btnSave" runat="server" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" OnClick="btnSave_Click">保存</asp:LinkButton>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="window.parent.closeDialog();">取消</a>
            </div>
        </div>
    </div>
</form>
</body>
</html>