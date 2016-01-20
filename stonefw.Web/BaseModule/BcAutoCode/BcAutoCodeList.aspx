<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcAutoCodeList.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcAutoCode.BcAutoCodeList" %>
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
                    <asp:LinkButton runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" class="easyui-linkbutton" data-options="iconCls:'icon-search'"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddNew" Text="新增" class="easyui-linkbutton" data-options="iconCls:'icon-add'" OnClientClick="return showDialog('新增', 'BcAutoCodeDetail.aspx?id=-1', this);"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvAutoCode" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvAutoCode_RowCommand" OnPageIndexChanged="gvAutoCode_PageIndexChanged" OnPageIndexChanging="gvAutoCode_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="删除" ItemStyle-Width="45px">
                <ItemTemplate>
                    <asp:LinkButton class="easyui-linkbutton" runat="server" CommandName="Row_Delete" CommandArgument='<%# Eval("Id") %>' OnClientClick="return deleteWarning(this);">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-Width="45px">
                <itemtemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"修改\", \"BcAutoCodeDetail.aspx?id=" + Eval("Id") + "\");" %>'>修改</a>
                </itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="功能点">
                <ItemTemplate>
                    <%# SysEnumNameExBiz.GetDescription<SysFuncPointEnum>((string) Eval("FuncPointId").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Prefix" HeaderText="前缀"/>
            <asp:BoundField DataField="DateFormat" HeaderText="日期格式"/>
            <asp:BoundField DataField="Digit" HeaderText="位数"/>
            <asp:TemplateField HeaderText="是否默认">
                <ItemTemplate>
                    <%# (bool) Eval("IsDefault") ? "是" : "否" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="效果展示">
                <ItemTemplate>
                    <%# GetResult(Eval("Prefix").ToString(), Eval("Digit").ToString()) %>
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