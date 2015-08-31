<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcRoleList.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcRole.BcRoleList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div class="query">
        <table>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'BcRoleDetail.aspx?roleid=-1', this);"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvRole" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvRole_RowCommand" OnPageIndexChanged="gvRole_PageIndexChanged" OnPageIndexChanging="gvRole_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("RoleId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                <itemtemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"修改\", \"BcRoleDetail.aspx?roleid=" + Eval("RoleId") + "\");" %>'>修改</a>
                </itemtemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RoleName" HeaderText="角色名称"/>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>