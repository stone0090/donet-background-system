using System;
using System.Web;
using System.Web.Security;
using stonefw.Entity.Enum;
using stonefw.Entity.Enum;
using stonefw.Biz.BaseModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web
{
    public partial class Login : BasePage
    {
        private string ReturnUrl
        {
            get { return /*Request["ReturnUrl"] ??*/ FormsAuthentication.DefaultUrl; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Context.User.Identity.IsAuthenticated)
                Response.Redirect(ReturnUrl);

            if (!IsPostBack)
            {
                if (Request.Cookies["webfwu"] != null && !string.IsNullOrEmpty(Request.Cookies["webfwu"].Value.Trim()))
                    this.name.Text = Request.Cookies["webfwu"].Value.Trim();

                if (Request.Cookies["webfwp"] != null && !string.IsNullOrEmpty(Request.Cookies["webfwp"].Value.Trim()))
                {
                    this.password.Attributes["value"] = Request.Cookies["webfwp"].Value.Trim();
                    this.remember.Checked = true;
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string usId = this.name.Text.Trim();
            string pswd = this.password.Text.Trim();

            if (string.IsNullOrEmpty(usId))
            {
                ShowErrorMsg("登录失败，用户名不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(pswd))
            {
                ShowErrorMsg("登录失败，密码不能为空！");
                return;
            }

            Session.Clear();

            Response.Cookies["webfwu"].Value = usId;
            Response.Cookies["webfwu"].Expires = DateTime.Now.AddMonths(2);

            if (this.remember.Checked)
            {
                Response.Cookies["webfwp"].Value = pswd;
                Response.Cookies["webfwp"].Expires = DateTime.Now.AddMonths(2);
            }
            else
            {
                Response.Cookies["webfwp"].Value = "";
            }

            var uiBiz = new BcUserInfoBiz();
            var result = uiBiz.DoLogin(usId, pswd);
            switch (result)
            {
                case LoginStatusEnum.Success:
                    FormsAuthentication.SetAuthCookie(usId, false);
                    //登陆成功，把用户编号保存到票据中    
                    var ticket = new FormsAuthenticationTicket(1, usId, DateTime.Now, DateTime.Now.AddMonths(2), false, usId, FormsAuthentication.FormsCookiePath);
                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    var newCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    HttpContext.Current.Response.Cookies.Add(newCookie);
                    //登陆成功，跳转到首页
                    Response.Redirect(ReturnUrl);
                    break;
                case LoginStatusEnum.PasswordError:
                    ShowErrorMsg("登录失败，密码错误！");
                    break;
                case LoginStatusEnum.UserNotExist:
                    ShowErrorMsg("登录失败，用户名不存在！");
                    break;
                case LoginStatusEnum.UserDisabled:
                    ShowErrorMsg("登录失败，用户被禁用！");
                    break;
                default:
                    ShowErrorMsg("登录失败，请重试！");
                    break;
            }
        }

        private void ShowErrorMsg(string msg)
        {
            this.divError.Attributes.Remove("class");
            this.divError.Attributes.Add("class", "error");
            this.errormsg.Text = msg;
        }
    }
}