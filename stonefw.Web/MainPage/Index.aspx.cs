using System;
using Stonefw.Biz.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.MainPage
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