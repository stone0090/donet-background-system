using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcLogError
{
    public partial class BcLogErrorList : BasePage
    {
        private BcLogErrorBiz _biz;

        private BcLogErrorBiz Biz
        {
            get { return _biz ?? (_biz = new BcLogErrorBiz()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void gvLogError_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcLogError(int.Parse(arg[0]));
                BindData();
            }
        }

        protected void gvLogError_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvLogError_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLogError.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvLogError.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvLogError.DataSource = Biz.GetBcLogErrorList();
            gvLogError.DataBind();
        }

        public string CutStr(string text, int len)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (text.Length < len) return text;
            return text.Substring(0, len) + "...";
        }
    }
}