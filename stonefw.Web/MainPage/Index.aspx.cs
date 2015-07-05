using System;
using System.Web.Security;
using stonefw.Web.Utility.BaseClass;
using stonefw.Biz.SystemModule;


namespace stonefw.Web.MainPage
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO 测试代码，用完应删除
            new SysPermsPointEnumBiz().GetSysPermsPointEnumList();
        }
    }
}
