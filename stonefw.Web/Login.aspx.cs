using System;
using System.Web;
using System.Web.Security;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web
{
    public partial class Login : BasePage
    {
        private string ReturnUrl
        {
            get { return /*Request["ReturnUrl"] ??*/ FormsAuthentication.DefaultUrl; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
                Response.Redirect(ReturnUrl);

            if (!IsPostBack)
            {
                if (Request.Cookies["webfwu"] != null && !string.IsNullOrEmpty(Request.Cookies["webfwu"].Value.Trim()))
                    name.Text = Request.Cookies["webfwu"].Value.Trim();

                if (Request.Cookies["webfwp"] != null && !string.IsNullOrEmpty(Request.Cookies["webfwp"].Value.Trim()))
                {
                    password.Attributes["value"] = Request.Cookies["webfwp"].Value.Trim();
                    remember.Checked = true;
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            var usId = name.Text.Trim();
            var pswd = password.Text.Trim();

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

            if (remember.Checked)
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
                    var ticket = new FormsAuthenticationTicket(1, usId, DateTime.Now, DateTime.Now.AddMonths(2), false,
                        usId, FormsAuthentication.FormsCookiePath);
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
            divError.Attributes.Remove("class");
            divError.Attributes.Add("class", "error");
            errormsg.Text = msg;
        }
    }
}