<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysPermsPointEnumList.aspx.cs" Inherits="Stonefw.Web.SystemModule.SysPermsPointEnum.SysPermsPointEnumList" %>

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
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvSysPermsPointEnum" runat="server" AutoGenerateColumns="False" AllowPaging="True" Width="100%" CssClass="gridview" OnPageIndexChanged="gvSysPermsPointEnum_PageIndexChanged" OnPageIndexChanging="gvSysPermsPointEnum_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
            <asp:BoundField DataField="Value" HeaderText="Value"/>
            <asp:BoundField DataField="Name" HeaderText="Name"/>
            <asp:BoundField DataField="Description" HeaderText="Description"/>
        </Columns>
    </asp:GridView>
    <div class="error">
        <asp:Label ID="lMessage" runat="server"></asp:Label>
    </div>
    <div id="dlg" class="easyui-dialog" data-options="closed:'false'"></div>
</form>
</body>
</html>