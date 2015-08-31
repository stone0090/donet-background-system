<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuCustomerList.aspx.cs" Inherits="Stonefw.Web.CustomerModule.CuCustomer.CuCustomerList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<script>
    var url = '<%= Request.Path %>';
</script>
<body>
<form id="form1" runat="server">
    <div class="query">
        <table>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" class="easyui-linkbutton" data-options="iconCls:'icon-search'" OnClick="btnQuery_Click"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return window.parent.addNewTab('客户管理 - 新增', '/CustomerModule/CuCustomer/CuCustomerDetail.aspx?cuid=-1', this, url);">新增</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvCuCustomer" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvCuCustomer_RowCommand" OnPageIndexChanged="gvCuCustomer_PageIndexChanged" OnPageIndexChanging="gvCuCustomer_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("CuId") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                <ItemTemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "window.parent.addNewTab(\"客户管理 - 修改(" + Eval("CuId") + ")\", \"/CustomerModule/CuCustomer/CuCustomerDetail.aspx?cuid=" + Eval("CuId") + "\",undefined,url);" %>'>修改</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CuId" HeaderText="编号"/>
            <asp:BoundField DataField="District" HeaderText="地区"/>
            <asp:BoundField DataField="CuName" HeaderText="姓名"/>
            <asp:TemplateField HeaderText="状态" ItemStyle-Width="60px">
                <ItemTemplate>
                    <%# (bool) Eval("ActivityFlag") ? "启用" : "停用" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>