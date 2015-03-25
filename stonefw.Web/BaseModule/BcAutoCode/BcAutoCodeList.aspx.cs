using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcAutoCode
{
    public partial class BcAutoCodeList : BasePage
    {
        private BcAutoCodeBiz _biz;
        private BcAutoCodeBiz Biz
        { get { return _biz ?? (_biz = new BcAutoCodeBiz()); } }

        public override bool InitPermission()
        {
            this.btnAddNew.Visible = LoadPermission(PermsPointEnum.Add);
            this.gvAutoCode.Columns[0].Visible = LoadPermission(PermsPointEnum.Delete);
            this.gvAutoCode.Columns[1].Visible = LoadPermission(PermsPointEnum.Edit);
            return LoadPermission(PermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack)BindData(); }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvAutoCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcAutoCode(int.Parse(arg[0]));
                BindData();
            }
        }
        protected void gvAutoCode_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvAutoCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvAutoCode.PageIndex = e.NewPageIndex; }

        private void BindData()
        {
            gvAutoCode.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvAutoCode.DataSource = Biz.GetBcAutoCodeList();
            gvAutoCode.DataBind();
        }
        public string GetResult(string prefix, string digit)
        {
            return prefix + "140101" + "00000000000000".Substring(0, int.Parse(digit) - 1) + '1';
        }
    }
}
