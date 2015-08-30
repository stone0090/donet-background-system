using System;
using System.Web.Security;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.MainPage
{
    public partial class Header : BasePage
    {
        public string Today
        {
            get
            {
                var today = DateTime.Now;
                return string.Format("{0} {1} 农历{2}", today.ToString("yyyy年MM月dd日"), ChinaDate.GetWeek(today),
                    ChinaDate.GetChinaDate(today));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            liSysManage.Visible = IsDevelopMode;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            CurrentUserInfo = null;
            Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }
}