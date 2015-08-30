<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BcLogErrorList.aspx.cs" Inherits="Stonefw.Web.BaseModule.BcLogError.BcLogErrorList" %>

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
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvLogError" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnRowCommand="gvLogError_RowCommand" OnPageIndexChanged="gvLogError_PageIndexChanged" OnPageIndexChanging="gvLogError_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="详细" ItemStyle-Width="45px">
                <ItemTemplate>
                    <a href="#" class="easyui-linkbutton" onclick='<%# "showDialog(\"详细\", \"BcLogErrorDetail.aspx?id=" + Eval("Id") + "\");" %>'>详细</a>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField DataField="UserName" HeaderText="用户" />--%>
            <asp:TemplateField HeaderText="错误页面">
                <ItemTemplate>
                    <%# CutStr(Eval("OpUrl").ToString(), 22) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="错误信息">
                <ItemTemplate>
                    <%# CutStr(Eval("Message").ToString(), 22) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OpTime" HeaderText="错误时间"/>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>