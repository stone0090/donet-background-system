<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysRelationList.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysRelation.SysRelationList" %>

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
                <td>模块列表： </td>
                <td>
                    <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" class="easyui-linkbutton" data-options="iconCls:'icon-search'" OnClick="btnQuery_Click"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'SysRelationDetail.aspx?moduleid=-1&funcpointid=-1', this);">新增</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvSysRelation" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvSysRelation_RowCommand" OnPageIndexChanged="gvSysRelation_PageIndexChanged" OnPageIndexChanging="gvSysRelation_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("ModuleId") + "|" + Eval("FuncPointId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                <ItemTemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"修改\", \"SysRelationDetail.aspx?moduleid=" + Eval("ModuleId") + "&funcpointid=" + Eval("FuncPointId") + "&permissions=" + Eval("Permissions") + "\");" %>'>修改</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ModuleName" HeaderText="模块名称"/>
            <asp:BoundField DataField="FuncPointName" HeaderText="功能名称"/>
            <asp:BoundField DataField="PermissionsName" HeaderText="权限名称"/>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>