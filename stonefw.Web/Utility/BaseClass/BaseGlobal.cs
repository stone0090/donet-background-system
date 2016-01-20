using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Utility;

namespace Stonefw.Web.Utility.BaseClass
{
    public class BaseGlobal : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var errorLogPath = Server.MapPath("/TempFile/ErrorLogPath");
            var errorPage = "/CustomPage/error.html";
            Exception ex = null;
            try
            {
                ex = Server.GetLastError().GetBaseException();

                var sysSettingEntity = new SysGlobalSettingBiz().GetSysSettingEntity();
                errorLogPath = Server.MapPath(sysSettingEntity.ErrorLogPath);
                errorPage = sysSettingEntity.ErrorPage;

                BcUserInfoEntity userInfo = null;
                var identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (identity != null)
                    userInfo = new BcUserInfoBiz().GetBcUserInfoWithPermission(identity.Ticket.UserData);

                var entity = new BcLogErrorEntity();
                if (userInfo != null)
                {
                    entity.UserId = userInfo.UserId;
                    entity.UserName = userInfo.UserName;
                }
                else
                {
                    entity.UserId = 0;
                    entity.UserName = "";
                }
                entity.OpUrl = Request.Url.ToString();
                entity.OpTime = DateTime.Now;
                entity.OpHostAddress = Request.UserHostAddress;
                entity.OpHostName = Request.UserHostName;
                entity.OpUserAgent = Request.UserAgent;
                entity.OpQueryString = Request.QueryString.ToString();
                entity.OpHttpMethod = Request.HttpMethod;
                entity.Message = ex.ToString();

                try
                {
                    new BcLogErrorBiz().AddNewBcLogError(entity);
                }
                catch (Exception ex2)
                {
                    WriteLocalLog(errorLogPath, ex.ToString());
                    WriteLocalLog(errorLogPath, ex2.ToString());
                }
            }
            catch (Exception ex3)
            {
                if (ex != null) WriteLocalLog(errorLogPath, ex.ToString());
                WriteLocalLog(errorLogPath, ex3.ToString());
            }
            finally
            {
                if (!ConfigHelper.GetConfigBool("IsDevelopMode"))
                {
                    Server.ClearError();
                    Response.Redirect(errorPage);
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        private void WriteLocalLog(string errorLogPath, string message)
        {
            var fileName = DateTime.Now.ToString("yy-MM-dd") + ".txt";
            var filePath = Path.Combine(errorLogPath, fileName);
            if (!Directory.Exists(errorLogPath))
                Directory.CreateDirectory(errorLogPath);
            using (var sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("----------------------------------------------------------");
                sw.WriteLine(message);
                sw.WriteLine("----------------------------------------------------------");
                sw.WriteLine("");
            }
        }
    }
}