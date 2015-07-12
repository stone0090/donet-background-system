using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Entity.Extension;
using stonefw.Entity.SystemModule;
using stonefw.Utility;

namespace stonefw.Web.Utility.BaseClass
{
    public abstract class BasePage : System.Web.UI.Page
    {
        #region 属性

        protected SysGlobalSettingEntity SysGlobalSetting
        {
            get
            {
                return new SysGlobalSettingBiz().GetSysSettingEntity();
            }
        }
        protected BcUserInfoEntity CurrentUserInfo
        {
            get
            {
                if (this.Context.User.Identity.IsAuthenticated)
                {
                    if (Session["CurrentUserInfo"] == null)
                    {
                        //重建Session，以避免Session丢失
                        var identity = HttpContext.Current.User.Identity as FormsIdentity;
                        if (identity != null)
                            Session["CurrentUserInfo"] = new BcUserInfoBiz().GetBcUserInfoWithPermission(identity.Ticket.UserData);
                    }
                    if (Session["CurrentUserInfo"] == null)
                        throw new Exception("获取用户信息失败！");
                    return (BcUserInfoEntity)Session["CurrentUserInfo"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["CurrentUserInfo"] = value;
            }
        }
        protected PermissionEntity CurrentPagePermission
        {
            get
            {
                return CurrentUserInfo != null ? LoadPermission() : null;
            }
        }
        protected SysPageFuncPointEntity CurrentPageFuncPoint
        {
            get { return new SysPageFuncPointBiz().GetSingleSysPageFuncPoint(Request.Path); }
        }

        protected bool IsDevelopMode
        {
            get { return ConfigHelper.GetConfigBool("IsDevelopMode"); }
        }

        #endregion

        #region 页面事件

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            CheckPermission();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitPageHeaderResousce();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #endregion

        #region 权限校验

        /// <summary>
        /// 检查页面是否有访问权限
        /// </summary>
        private void CheckPermission()
        {
            //未登录状态，不需要判断权限
            if (!this.Context.User.Identity.IsAuthenticated)
            {
                //1.获取页面自定义的访问权限
                if (!InitPermission())
                {
                    Response.Write("您没有权限访问该页面！");
                    Response.End();
                }

                //2.非开发模式下，不允许访问后台管理功能
                if (!IsDevelopMode && Request.Path.ToLower().Contains("systemmodule"))
                {
                    Response.Write("您没有权限访问该页面！");
                    Response.End();
                }
            }
        }

        #endregion

        #region 私有方法
        private void InitPageHeaderResousce()
        {
            var js = new[]
            {
                "/Resource/js/json2.js",
                "/Resource/js/common.js",
                "/Resource/easyui/locale/easyui-lang-zh_CN.js", 
                "/Resource/easyui/jquery.easyui.min.js",
                "/Resource/easyui/jquery.min.js"
            };
            foreach (string s in js)
            {
                var script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = s;
                this.Header.Controls.AddAt(0, script);
            }

            var css = new[] { 
                "/Resource/css/common.css" ,
                "/Resource/easyui/themes/icon.css", 
                "/Resource/easyui/themes/default/easyui.css"
            };
            foreach (string s in css)
            {
                var link = new HtmlGenericControl("link");
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["type"] = "text/css";
                link.Attributes["href"] = s;
                this.Header.Controls.AddAt(0, link);
            }

            var meta1 = new HtmlGenericControl("meta");
            meta1.Attributes["http-equiv"] = "X-UA-Compatible";
            meta1.Attributes["content"] = "IE=9";
            this.Header.Controls.AddAt(0, meta1);

            var meta2 = new HtmlGenericControl("meta");
            meta2.Attributes["name"] = "viewport";
            meta2.Attributes["content"] = "width=800px,user-scalable=no;";
            this.Header.Controls.AddAt(0, meta2);
        }

        #endregion

        #region 供子类使用的通用方法

        public void FatherQuery()
        {
            const string script = "<script type='text/javascript'>window.parent.__doPostBack('btnQuery', '');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), null, script);
        }
        public void FatherQuery2()
        {
            string script = string.Format("<script type='text/javascript'>window.parent.refreshRelationTab('{0}');window.parent.closeSelectedTab(true);</script>", Request.Path);
            ClientScript.RegisterClientScriptBlock(this.GetType(), null, script);
        }
        public PermissionEntity LoadPermission()
        {
            try
            {
                var pageUrl = Request.Path;
                if (pageUrl.Contains("?")) pageUrl = pageUrl.Split('?')[0];
                var entity = new SysPageFuncPointBiz().GetSingleSysPageFuncPoint(pageUrl);
                return CurrentUserInfo.PermisionList.Where(n => n.FuncPointId == entity.FuncPointId).ToList()[0];
            }
            catch
            {
                Response.Write("您没有权限访问该页面！");
                Response.End();
                return null;
            }
        }
        public bool LoadPermission(SysPermsPointEnum permsPoint)
        {
            var permissionEntity = LoadPermission();
            if (permissionEntity != null)
                return permissionEntity.PermissionList.Any(n => n == permsPoint.ToString());
            return false;
        }

        #endregion

        #region 子类必须实现的方法

        public virtual bool InitPermission()
        {
            return true;
        }

        #endregion

    }
}
