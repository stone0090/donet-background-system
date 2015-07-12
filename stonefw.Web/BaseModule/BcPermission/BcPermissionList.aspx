<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcPermissionList.aspx.cs" Inherits="stonefw.Web.BaseModule.BcPermission.BcPermissionList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        function AddNewOrEdit(self) {
            if ($("#ddlPermissionType").val() == "1")
                return showDialog('新增/修改 - 角色（' + $('#ddlRole option:selected').text() + '）', 'BcPermissionDetail.aspx?permissionid=' + $("#ddlRole").val() + '&permissiontype=1', self);
            else
                return showDialog('新增/修改 - 用户（' + $('#ddlUser option:selected').text() + '）', 'BcPermissionDetail.aspx?permissionid=' + $("#ddlUser").val() + '&permissiontype=2', self);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="query">
            <table>
                <tr>
                    <td>类型：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPermissionType" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false">
                            <asp:ListItem Value="2">用户</asp:ListItem>
                            <asp:ListItem Value="1">角色</asp:ListItem>
                        </asp:DropDownList></td>
                    <td id="tName">角色：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRole" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false" />
                        <asp:DropDownList runat="server" ID="ddlUser" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged" class="easyui-combobox" data-options="required:true,panelHeight:'auto',editable:false" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnAddNew" Text="新增/修改" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return AddNewOrEdit(this);"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <asp:GridView ID="gvBcPermission" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvBcPermission_RowCommand" OnPageIndexChanged="gvBcPermission_PageIndexChanged" OnPageIndexChanging="gvBcPermission_PageIndexChanging">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <RowStyle HorizontalAlign="Center"></RowStyle>
            <Columns>
                <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                    <ItemTemplate>
                        <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("UserRoleId")+"|"+Eval("PermissionType")+"|"+Eval("ModuleId")+"|"+Eval("FuncPointId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ModuleName" HeaderText="模块名称" />
                <asp:BoundField DataField="FuncPointName" HeaderText="功能点名称" />
                <asp:BoundField DataField="PermissionNames" HeaderText="包含权限点" />
            </Columns>
        </asp:GridView>
        <div class="error">
            <asp:Label ID="lMessage" runat="server"></asp:Label>
        </div>
        <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
    </form>
</body>
</html>
