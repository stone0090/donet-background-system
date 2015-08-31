using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.CustomerModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.CustomerModule.CuCustomer
{
    public partial class CuCustomerList : BasePage
    {
        private CuCustomerBiz _biz;

        private CuCustomerBiz Biz
        {
            get { return _biz ?? (_biz = new CuCustomerBiz()); }
        }

        protected override bool InitPermission()
        {
            btnAddNew.Visible = LoadPermission(SysPermsPointEnum.Add);
            gvCuCustomer.Columns[0].Visible = LoadPermission(SysPermsPointEnum.Delete);
            gvCuCustomer.Columns[1].Visible = LoadPermission(SysPermsPointEnum.Edit);
            return LoadPermission(SysPermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void gvCuCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                Biz.DeleteCuCustomer(arg[0]);
                BindData();
            }
        }

        protected void gvCuCustomer_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvCuCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCuCustomer.PageIndex = e.NewPageIndex;
        }

        private void BindData()
        {
            gvCuCustomer.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvCuCustomer.DataSource = Biz.GetCuCustomerList();
            gvCuCustomer.DataBind();
        }
    }
}