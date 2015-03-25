//Date:2013-05-26 Author:Stone 
//Description:公共的js方法

//所有页面默认加载的方法
$(function () {

    //为gridview统一加载默认样式
    $(".gridview tr").each(function () {
        $(this).attr("onmouseover", "this.style.backgroundColor='#E6F5FA'");
        $(this).attr("onmouseout", "this.style.backgroundColor='#FFFFFF'");
    });

    //加宽所有gridview的分页页码，以便兼容移动设备
    $(".gridview table:last").find("td").each(function () {
        $(this).css("width", "18px");
        $(this).css("text-align", "center");
    });

    ////解决的easyui-linkbutton的asp:LinkButton不能出发服务器端事件 
    //$("a").each(function (i, o) {
    //    if (o.href && o.href.indexOf("__doPostBack") >= 0) {
    //        var func = o.href.replace("javascript:", "");
    //        $(o).click(function () { try { eval(func); } catch (e) { } });
    //    }
    //});

});


//扩展jquery的方法
$.extend({
    /* check is json or not */
    isJson: function (obj) {
        return typeof (obj) == "object" && Object.prototype.toString.call(obj).toLowerCase() == "[object object]" && !obj.length && Object.keys(obj).length;
    },
    /*input more json data to o*/
    apply: function (o) {
        var c = arguments;
        for (var i = 1; i < c.length; i++) {
            if (o && c[i] && typeof c[i] == 'object')
                for (var p in c[i]) o[p] = (c[i])[p];
        }
        return o;
    },
    copy: function (o) {
        var c = {};
        for (var p in o) {
            switch (typeof o[p]) {
                case "object": c[p] = this.copy(o[p]); break;
                default: c[p] = o[p];
            }
        }
        return c;
    },
    reverse: function (str) {
        var tmp = str.toString();
        var ret = [];
        for (i = tmp.length; i >= 0; i -= 1)
            ret.push(tmp.substring(i, i + 1));
        return ret.join('');
    },
    applyIf: function (o, c) {
        if (o) {
            for (var p in c) {
                if ($.isEmpty(o[p])) {
                    o[p] = c[p];
                }
            }
        }
        return o;
    },
    isEmpty: function (v, allowBlank) {
        return (typeof (v) == 'string' && $.trim(v).length == 0) || v === null || v === undefined || (($.isArray(v) && !v.length)) || (!allowBlank ? v === '' : false);
    },
    isObject: function (v) {
        return v && typeof v == "object";
    },
    isPrimitive: function (v) {
        var t = typeof v;
        return t == 'string' || t == 'number' || t == 'boolean';
    },
    queryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return "";
    },
    value: function (data, name, def) {
        var value = data[name];
        return !$.isEmpty(value) ? value : def;
    },
    json2xml: function (prop, tagName) {
        if (!prop) return "";
        var json = [];
        if (!$.isEmpty(tagName)) json.push("<" + tagName + ">");
        var self = this, v, name;
        for (var key in prop) {
            v = prop[key];
            switch (typeof v) {
                case "function": case "undefined": break;
                case "object":
                    json.push('<', key, '>');
                    if ($.isArray(v))
                        $.every(v, function (i, row) { json.push(self._jsonToXml(row, "ITEM")) });
                    else
                        json.push(this.json2xml(v, "ITEM"));
                    json.push('</', key, '>');
                    break;
                default:
                    json.push('<', key, '>', v, '</', key, '>');
            }
        }
        if (!$.isEmpty(tagName)) json.push("</" + tagName + ">");
        return json.join('');
    },
    /*将数据转为json*/
    array2json: function (field, data) {
        var ret = [];
        var json = {};
        $.each(data, function (i, row) {
            if ($.isArray(row)) {
                ret.push($.array2json(field, row)[0]);
            }
            else {
                json[field[i]] = row;
            }
        });
        if (ret.length == 0) ret.push(json);
        return ret;
    }
});




//$(function () {
//    //解决服务器端控件会失效的问题，把dialog加入到form中
//    $('.window').appendTo('form:first');
//    $('.window-shadow').appendTo('form:first');
//});

//Date:2013-05-26 Author:Stone
//Description:当改变浏览器窗口大小的时候，自动调整DataGrid的大小
function autoSizeDataGrid(elementId) {
    $(window).resize(function () {
        //alert(document.body.clientWidth);
        //alert(document.body.clientHeight);
        $("#" + elementId).datagrid('resize', {
            width: function () { //自动调整宽度
                return document.body.clientWidth;
            }
            //, // 因为DataGrid的高度是随内容条数而变化的，不需要再自动调整高度
            //height: function() { //自动调整高度
            //    return document.body.clientHeight;
            //} 
        });
    });
}

//Date:2013-08-04 Author:Stone
//Description:封装JQueryEasyUI的消息方法，使其更简单易用
var const_title = "系统消息";
var msg = new Msg();
function Msg() {

    this.show = function (msg) {
        $.messager.show({
            title: const_title,
            msg: msg
        });
    };

    this.error = function (msg) {
        $.messager.show({
            title: '错误',
            msg: msg
        });
    };

    this.warnning = function (msg) {
        $.messager.show({
            title: '警告',
            msg: msg
        });
    };

    this.success = function () {
        $.messager.show({
            title: const_title,
            msg: '操作成功！'
        });
    };

    this.failed = function () {
        $.messager.show({
            title: const_title,
            msg: '操作失败，请重试！'
        });
    };

}

var myAlert = new Alert();
function Alert() {

    this.show = function (msg) {
        $.messager.alert(const_title, msg);
    };

    this.error = function (msg) {
        $.messager.alert(const_title, msg, "error");
    };

    this.info = function (msg) {
        $.messager.alert(const_title, msg, "info");
    };

    this.question = function (msg) {
        $.messager.alert(const_title, msg, "question");
    };

    this.warning = function (msg) {
        $.messager.alert(const_title, msg, "warning");
    };
}

//Date:2013-08-04 Author:Stone
//Description:删除确认
function deleteWarning(self) {
    if (confirm("您确定要删除本条记录吗？\r\n删除点击“确定”，不删除点击“取消”。"))
        return true;
    else {
        $(self).unbind("click");
        return false;
    }
}

function saveForm(self) {
    if ($('#form1').form('validate'))
        return true;
    else {
        $(self).unbind("click");
        return false;
    }
}

//Date:2013-08-04 Author:Stone
//Description:弹出窗口，用iframe加载窗口内容
function showDialog(title, url, self, width, height) {
    if ($.isEmpty(width)) width = 520;
    if ($.isEmpty(height)) height = 350;
    $('#dlg').dialog({
        title: title,
        content: '<iframe width="100%" height="99%" frameborder="0" marginwidth="5" marginheight="5" allowtransparency="yes" src="' + url + '" ></iframe>',
        width: width,
        height: height,
        left: 60,
        top: 60,
        closed: false,
        cache: false,
        modal: true
    });
    $(self).unbind('click');
    return false;
}

//Date:2013-08-04 Author:Stone
//Description:关闭窗口
function closeDialog() {
    $('#dlg').dialog('close');
}