using System;
using System.Web.UI.WebControls;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysFuncPointEnum
{
    public partial class SysFuncPointEnumList : BasePage
    {
        private SysFuncPointEnumBiz _biz;
        private SysFuncPointEnumBiz Biz
        { get { return _biz ?? (_biz = new SysFuncPointEnumBiz()); } }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }        
        protected void gvSysFuncPointEnum_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvSysFuncPointEnum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvSysFuncPointEnum.PageIndex = e.NewPageIndex; }
        private void BindData()
        {
            gvSysFuncPointEnum.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvSysFuncPointEnum.DataSource = Biz.GetSysFuncPointEnumList();
            gvSysFuncPointEnum.DataBind();
        }
    }
}
