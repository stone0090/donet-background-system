<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="stonefw.Web.MainPage.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow: auto">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= base.SysGlobalSetting.SysName %></title>
    <link href="/resource/css/mainpage.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var _tabCache = new Object;
        var _urlRelation = new Object;
        var _isSilentClose = false;

        $(function () {

            //缓存已打开的tab，再次打开同一个tab时，选中已打开的tab
            $('#tabs').tabs({
                onSelect: function (title, index) {
                    _tabCache[title] = index;
                },
                onBeforeClose: function (title, index) {
                    //是否为静默关闭tab
                    if (!_isSilentClose)
                        return confirm('您确定要关闭“' + title + '”吗？');
                    _isSilentClose = false;
                },
                onClose: function (title, index) {
                    delete _tabCache[title];
                }
            });

            ////当浏览器被缩放到小于800*540时，则把body的宽高设置为800*540
            //autoMinSize();

            //$(window).resize(function () {
            //    autoMinSize();
            //});

        });

        function closeSelectedTab(isSilent) {
            if (!$.isEmpty(isSilent))
                _isSilentClose = isSilent;
            var tab = $('#tabs').tabs('getSelected');
            var index = $('#tabs').tabs('getTabIndex', tab);
            $('#tabs').tabs('close', index);
        }

        function refreshRelationTab(url) {
            $("iframe").each(function () {
                var srcUrl = _urlRelation[url];
                if (!$.isEmpty(srcUrl) && $(this).attr("src") == srcUrl) {
                    $(this)[0].contentWindow.__doPostBack('btnQuery', '');
                }
            });
        }

        function addNewTab(title, url, self, srcUrl) {
            if ($.isEmpty(_tabCache[title])) {
                if (!$.isEmpty(url)) {
                    $('#tabs').tabs('add', {
                        title: title,
                        content: '<iframe width="100%" height="99%" frameborder="0" marginwidth="5" marginheight="5" ' +
                                 ' allowtransparency="yes" src="' + url + '" ></iframe>',
                        closable: true
                    });
                    if (!$.isEmpty(srcUrl)) {
                        if (url.indexOf('?') > -1)
                            url = url.split('?')[0];
                        _urlRelation[url] = srcUrl;
                    }
                }
            } else {
                $('#tabs').tabs('select', parseInt(_tabCache[title]));
            }
            $(self).unbind('click');
            return false;
        }

        //function autoMinSize() {
        //    var minWidth = 800;
        //    var minHeight = 540;

        //    if (document.documentElement.clientWidth < minWidth) {
        //        if (document.documentElement.clientHeight < minHeight) {
        //            $('body:eq(0)').css({
        //                'width': minWidth + 'px',
        //                'height': minHeight + 'px'
        //            });
        //        } else {
        //            $('body:eq(0)').css({
        //                'width': minWidth + 'px'
        //            });
        //        }
        //    } else {
        //        if (document.documentElement.clientHeight < minHeight) {
        //            $('body:eq(0)').css({
        //                'height': minHeight + 'px'
        //            });
        //        } else {
        //            $('body:eq(0)').removeAttr('style');
        //        }
        //    }
        //}
    </script>
</head>
<body class="easyui-layout">
    <div id="divHeader" data-options="region:'north',border:false,href:'Header.aspx'">
    </div>
    <div id="divMenuBar" data-options="region:'west',split:true,collapsed:false,title:'菜单栏',href:'Menu.aspx'">
    </div>
    <div id="divFooter" data-options="region:'south',border:false,href:'Footer.html'">
    </div>
    <div id="divContent" data-options="region:'center',title:false">
        <div id="tabs" class="easyui-tabs" data-options="fit:true,border:false">
            <div id="divTitleBar" title="公告栏" data-options="href:'BulletinBoard.html'">
            </div>
        </div>
    </div>
</body>
</html>
