<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectListBox.ascx.cs" Inherits="Stonefw.Web.Utility.UserControl.SelectListBox" %>
<%-- 引用控件的页面需要引用 easyui 和 jquery --%>
<span id="spanPanel" runat="server">
    <asp:TextBox ID="txtText" runat="server" Enabled="False" Style="height: 15px;"></asp:TextBox>
    <asp:Button ID="btnShowDialog" runat="server" Text="..." Style="border-width: 0; height: 21px; line-height: 20px; margin-bottom: 1px; margin-left: -3px; padding-left: 0; padding-right: 0; width: 20px;" />
</span>
<div style="display: none">
    <div runat="server" id="divDialog" class="easyui-dialog" data-options="closed:'false'" style="padding: 2px;">
        <table runat="server" id="tblDg" class="easyui-datagrid">
        </table>
    </div>
    <div runat="server" id="divToolBar" style="height: auto; padding: 2px;">
        <table>
            <tr>
                <td>
                    <select runat="server" id="selectFiled" class="easyui-combobox" data-options="panelheight:'auto',editable:false">
                    </select>
                </td>
                <td>
                    <input runat="server" id="txtSearch" class="easyui-searchbox"/>
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divButtons">
        <a href="javascript:void(0)" runat="server" id="btnSave" class="easyui-linkbutton">保存</a>
        <a href="javascript:void(0)" runat="server" id="btnClose" class="easyui-linkbutton">关闭</a>
    </div>
    <asp:HiddenField runat="server" ID="hdValue"/>
    <asp:HiddenField runat="server" ID="hdData"/>
    <asp:HiddenField runat="server" ID="hdColumns"/>
    <asp:HiddenField runat="server" ID="hdTextField"/>
    <asp:HiddenField runat="server" ID="hdValueField"/>
    <asp:HiddenField runat="server" ID="hdSingleSelect" Value="true"/>
    <asp:HiddenField runat="server" ID="hdShowSearchBox" Value="false"/>
    <asp:HiddenField runat="server" ID="hdTitle" Value="列表"/>
    <asp:HiddenField runat="server" ID="hdDialogWidth" Value="250"/>
    <asp:HiddenField runat="server" ID="hdDialogHeight" Value="280"/>
</div>
<script>
    //初始化控件宽度和高度
    $('#' + '<%= tblDg.ClientID %>').width($('#' + '<%= hdDialogWidth.ClientID %>').val() - 18);
    $('#' + '<%= tblDg.ClientID %>').height($('#' + '<%= hdDialogHeight.ClientID %>').val() - 77);
    $('#' + '<%= selectFiled.ClientID %>').width(($('#' + '<%= hdDialogWidth.ClientID %>').val() - 33) / 3);
    $('#' + '<%= txtSearch.ClientID %>').width(($('#' + '<%= hdDialogWidth.ClientID %>').val() - 33) / 3 * 2);

    //初始化Text的值
    if (eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")") &&
        eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")").length > 0 &&
        $('#' + '<%= hdValue.ClientID %>').val() != "" &&
        $('#' + '<%= txtText.ClientID %>').val() == "") {
        var texts = "";
        $.each(eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")"), function(index, row) {
            if ((',' + $('#' + '<%= hdValue.ClientID %>').val() + ',').indexOf(',' + row[$('#' + '<%= hdValueField.ClientID %>').val()] + ',') > -1) {
                texts += row[$('#' + '<%= hdTextField.ClientID %>').val()] + ",";
            }
        });
        $('#' + '<%= txtText.ClientID %>').val(texts.substr(0, texts.length - 1));
    }

    //初始化模态框
    $('#' + '<%= btnShowDialog.ClientID %>').click(function() {
        //把最选中的记录显示在最前面
        var data = eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")");
        var data1 = new Array();
        var data2 = new Array();
        if (data && data.length > 0 && $('#' + '<%= hdValue.ClientID %>').val() != "") {
            var texts = "";
            $.each(data, function(index, row) {
                if ((',' + $('#' + '<%= hdValue.ClientID %>').val() + ',').indexOf(',' + row[$('#' + '<%= hdValueField.ClientID %>').val()] + ',') > -1) {
                    data1.push(row);
                    texts += row[$('#' + '<%= hdTextField.ClientID %>').val()] + ",";
                } else {
                    data2.push(row);
                }
            });
            $('#' + '<%= txtText.ClientID %>').val(texts.substr(0, texts.length - 1));
            $('#' + '<%= tblDg.ClientID %>').datagrid({ data: data1.concat(data2) });
            $.each($('#' + '<%= hdValue.ClientID %>').val().split(','), function(i, v) {
                $('#' + '<%= tblDg.ClientID %>').datagrid('selectRow', i);
            });
        }

        //清楚过滤条件
        $('#' + '<%= selectFiled.ClientID %>').combobox('setValue', 0);
        $('#' + '<%= txtSearch.ClientID %>').searchbox('clear');

        //打开列表窗口
        $('#' + '<%= divDialog.ClientID %>').dialog({
            title: $('#' + '<%= hdTitle.ClientID %>').val(),
            width: $('#' + '<%= hdDialogWidth.ClientID %>').val(),
            height: $('#' + '<%= hdDialogHeight.ClientID %>').val(),
            closed: false,
            cache: false,
            modal: true,
            buttons: '#' + '<%= divButtons.ClientID %>',
        });
        return false;
    });

    //初始化搜索框
    $('#' + '<%= txtSearch.ClientID %>').searchbox({
        prompt: '请输入关键字',
        searcher: function(value, name) {
            if (value == "") {
                $('#' + '<%= tblDg.ClientID %>').datagrid({ data: eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")") });
            } else {
                var data = eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")");
                var field = $('#' + '<%= selectFiled.ClientID %>').combobox('getValue');
                if (field == 0) {
                    alert("请选择字段后再查询！");
                    return;
                }
                var searchResult = new Array();
                if (data && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i][field].indexOf(value) > -1) {
                            searchResult.push(data[i]);
                        }
                    }
                }
                $('#' + '<%= tblDg.ClientID %>').datagrid({ data: searchResult });
            }
        }
    });

    //DataGrid初始化
    $('#' + '<%= tblDg.ClientID %>').datagrid({
        rownumbers: true,
        singleSelect: $('#' + '<%= hdSingleSelect.ClientID %>').val().toLowerCase() == 'true',
        columns: eval("(" + $('#' + '<%= hdColumns.ClientID %>').val() + ")"),
        data: eval("(" + $('#' + '<%= hdData.ClientID %>').val() + ")"),
        onDblClickCell: function(index, field, value) {
            if ($('#' + '<%= hdSingleSelect.ClientID %>').val().toLowerCase() == 'true')
                $('#' + '<%= btnSave.ClientID %>').click();
        }
    });

    if ($('#' + '<%= hdShowSearchBox.ClientID %>').val().toLowerCase() == 'true') {
        $('#' + '<%= tblDg.ClientID %>').datagrid({ toolbar: '#' + '<%= divToolBar.ClientID %>' });
    }

    //Button初始化
    $('#' + '<%= btnSave.ClientID %>').click(function() {
        if ($('#' + '<%= hdSingleSelect.ClientID %>').val().toLowerCase() == 'true') {
            var row = $('#' + '<%= tblDg.ClientID %>').datagrid('getSelected');
            if (row) {
                $('#' + '<%= txtText.ClientID %>').val(row[$('#' + '<%= hdTextField.ClientID %>').val()]);
                $('#' + '<%= hdValue.ClientID %>').val(row[$('#' + '<%= hdValueField.ClientID %>').val()]);

            } else {
                $('#' + '<%= txtText.ClientID %>').val("");
                $('#' + '<%= hdValue.ClientID %>').val("");
            }
        } else {
            var rows = $('#' + '<%= tblDg.ClientID %>').datagrid('getSelections');
            if (rows && rows.length > 0) {
                var texts = "", values = "";
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    texts += row[$('#' + '<%= hdTextField.ClientID %>').val()] + ",";
                    values += row[$('#' + '<%= hdValueField.ClientID %>').val()] + ",";
                }
                $('#' + '<%= txtText.ClientID %>').val(texts.substr(0, texts.length - 1));
                $('#' + '<%= hdValue.ClientID %>').val(values.substr(0, values.length - 1));
            } else {
                $('#' + '<%= txtText.ClientID %>').val("");
                $('#' + '<%= hdValue.ClientID %>').val("");
            }
        }
        $('#' + '<%= divDialog.ClientID %>').dialog('close');
    });

    $('#' + '<%= btnClose.ClientID %>').click(function() {
        $('#' + '<%= divDialog.ClientID %>').dialog('close');
    });

</script>