<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysPageFuncPointList.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysPageFuncPoint.SysPageFuncPointList" %>
<%@ Import Namespace="Stonefw.Entity.Enum" %>
<%@ Import Namespace="Stonefw.Biz.SystemModule" %>

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
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" class="easyui-linkbutton" data-options="iconCls:'icon-search'" OnClick="btnQuery_Click"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'SysPageFuncPointDetail.aspx?pageurl=-1', this);">新增</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvSysPageFuncPoint" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvSysPageFuncPoint_RowCommand" OnPageIndexChanged="gvSysPageFuncPoint_PageIndexChanged" OnPageIndexChanging="gvSysPageFuncPoint_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("PageUrl") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="功能点">
                <ItemTemplate>
                    <%# SysEnumNameExBiz.GetDescription<SysFuncPointEnum>((string) Eval("FuncPointId").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FuncPointId" HeaderText="FuncPointId"/>
            <asp:BoundField DataField="PageUrl" HeaderText="PageUrl"/>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>