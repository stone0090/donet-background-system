using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcAutoCode
{
    public partial class BcAutoCodeList : BasePage
    {
        private BcAutoCodeBiz _biz;

        private BcAutoCodeBiz Biz
        {
            get { return _biz ?? (_biz = new BcAutoCodeBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvAutoCode.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            gvAutoCode.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
            return LoadPermission(SysPermsPointEnum.View);
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

        protected void gvAutoCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteBcAutoCode(int.Parse(arg[0]));
                BindData();
            }
        }

        protected void gvAutoCode_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvAutoCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAutoCode.PageIndex = e.NewPageIndex;
        }

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