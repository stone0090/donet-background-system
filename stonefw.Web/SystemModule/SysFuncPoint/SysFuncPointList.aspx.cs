using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysFuncPoint
{
    public partial class SysFuncPointList : BasePage
    {
        private SysFuncPointBiz _biz;
        private SysFuncPointBiz Biz
        { get { return _biz ?? (_biz = new SysFuncPointBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvSysFuncPoint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteSysFuncPoint(arg[0]);
                this.lMessage.Text = er.GetName();
                if (er != ExcuteResult.Success) return;
                BindData();
            }
        }
        protected void gvSysFuncPoint_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysFuncPoint_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysFuncPoint.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvSysFuncPoint.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysFuncPoint.DataSource = Biz.GetSysFuncPointList();
            gvSysFuncPoint.DataBind();
        }
    }
}
