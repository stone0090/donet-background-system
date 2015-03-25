function loadjs() {
    try {
        var jsList = new Array();
        jsList[jsList.length] = "/Resource/easyui/jquery.min.js";
        jsList[jsList.length] = "/Resource/easyui/jquery.easyui.min.js";
        jsList[jsList.length] = "/Resource/easyui/locale/easyui-lang-zh_CN.js";
        var oHead = document.getElementsByTagName("head").item(0);
        for (var i = 0; i < jsList.length; i++) {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.language = "javascript";
            script.src = jsList[i];
            oHead.appendChild(script);
        }
    }
    catch (e) {
        //alert(e.message);
    }
}

function loadcss() {
    try {
        var cssList = new Array();
        cssList[cssList.length] = "/Resource/easyui/themes/icon.css";
        cssList[cssList.length] = "/Resource/easyui/themes/default/easyui.css";
        var oHead = document.getElementsByTagName("head").item(0);
        for (var i = 0; i < cssList.length; i++) {
            var css = document.createElement("link");
            css.type = "text/css";
            css.rel = "stylesheet";
            css.href = cssList[i];
            oHead.appendChild(css);
        }
    }
    catch (e) {
        //alert(e.message);
    }
}

loadjs();
loadcss();
