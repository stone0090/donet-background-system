<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcUserRoleList.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcUserRole.BcUserRoleList" %>

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
                <td>用户列表：</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUser" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false"/>
                </td>
                <td>角色列表：</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRole" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false"/>
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'BcUserRoleDetail.aspx?userid=-1&roleid=-1', this);"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvBcUserRole" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvBcUserRole_RowCommand" OnPageIndexChanged="gvBcUserRole_PageIndexChanged" OnPageIndexChanging="gvBcUserRole_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("UserId") + "|" + Eval("RoleId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserName" HeaderText="用户名称"/>
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