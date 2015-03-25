<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysMenuList.aspx.cs" Inherits="stonefw.Web.SystemModule.SysMenu.SysMenuList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>菜单列表</title>
    <script src="/Resource/js/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="query">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                            <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add'" onclick="showDialog('新增', 'SysMenuDetail.aspx?menuid=-1');">新增</a>
                            <a href="#" class="easyui-linkbutton" onclick="showDialog('调整顺序', 'SysMenuSorting.aspx');">调整顺序</a>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="50" Width="100%"
                OnRowCommand="gvMenu_RowCommand" OnPageIndexChanged="gvMenu_PageIndexChanged" OnPageIndexChanging="gvMenu_PageIndexChanging"
                CssClass="gridview">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                        <ItemTemplate>
                            <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("MenuId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                        <ItemTemplate>
                            <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"修改\", \"SysMenuDetail.aspx?menuid="+Eval("MenuId")+"\");" %>'>修改</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ModuleName" HeaderText="所属模块" />
                    <asp:BoundField DataField="FuncPointName" HeaderText="所属功能" />
                    <asp:BoundField DataField="MenuTreeName" HeaderText="菜单结构" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="菜单状态" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <%# (bool)Eval("ActivityFlag")?"启用":"停用"  %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="error">
                <asp:Label ID="lMessage" runat="server"></asp:Label>
            </div>
            <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
        </div>
    </form>
</body>
</html>

