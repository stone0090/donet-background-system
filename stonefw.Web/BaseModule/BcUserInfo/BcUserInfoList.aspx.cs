using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcUserInfo
{
    public partial class BcUserInfoList : BasePage
    {
        private BcUserInfoBiz _biz;
        private BcUserInfoBiz Biz
        { get { return _biz ?? (_biz = new BcUserInfoBiz()); } }

        public override bool InitPermission()
        {
            this.btnAddNew.Visible = LoadPermission(PermsPointEnum.Add);
            this.gvUserInfo.Columns[0].Visible = LoadPermission(PermsPointEnum.Delete);
            this.gvUserInfo.Columns[1].Visible = LoadPermission(PermsPointEnum.Edit);
            return LoadPermission(PermsPointEnum.View);
        }
        //public override void InitPermission()
        //{
        //    this.btnAddNew.Visible = LoadPermission(PermsPointEnum.Add);
        //    this.gvUserInfo.Columns[0].Visible = LoadPermission(PermsPointEnum.Delete);
        //    this.gvUserInfo.Columns[1].Visible = LoadPermission(PermsPointEnum.Edit);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                BindData();
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e) { BindData(); this.lMessage.Text = "执行成功！"; }
        protected void gvUserInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                string[] arg = e.CommandArgument.ToString().Split('|');
                var er = Biz.DeleteBcUserInfo(int.Parse(arg[0]));
                this.lMessage.Text = er.GetName();
                if (er != ExcuteResult.Success) return;
                BindData();
            }
        }
        protected void gvUserInfo_PageIndexChanged(object sender, EventArgs e) { BindData(); }
        protected void gvUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { this.gvUserInfo.PageIndex = e.NewPageIndex; }

        private void BindControlData()
        {
            //this.ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            //this.ddlRole.DataValueField = "Role_Id";
            //this.ddlRole.DataTextField = "Role_Name";
            //this.ddlRole.DataBind();
            //this.ddlRole.Items.Insert(0, new ListItem("*全部*", "0"));

            this.ddlGroup.DataSource = new BcGroupBiz().GetBcGroupList();
            this.ddlGroup.DataValueField = "GroupId";
            this.ddlGroup.DataTextField = "GroupName";
            this.ddlGroup.DataBind();
            this.ddlGroup.Items.Insert(0, new ListItem("*全部*", "0"));
        }
        private void BindData()
        {
            gvUserInfo.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvUserInfo.DataSource = Biz.GetBcUserInfoList(
                int.Parse(ddlGroup.SelectedValue),
                0, //int.Parse(ddlRole.SelectedValue)
                txtUserName.Text.Trim());
            gvUserInfo.DataBind();
        }
    }
}
