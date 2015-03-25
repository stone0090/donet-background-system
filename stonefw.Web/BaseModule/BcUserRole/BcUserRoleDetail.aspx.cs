using System;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcUserRole
{
    public partial class BcUserRoleDetail : BasePage
    {
        private BcUserRoleBiz _biz;
        private BcUserRoleBiz Biz { get { return _biz ?? (_biz = new BcUserRoleBiz()); } }

        public override bool InitPermission()
        {
            return LoadPermission(PermsPointEnum.Add) || LoadPermission(PermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BcUserRoleEntity entity = PrepareFormData();
                ExcuteResult er = Biz.AddNewBcUserRole(entity);
                if (er != ExcuteResult.Success)
                {
                    this.lMessage.Text = er.GetName();
                    return;
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindControlData()
        {
            this.ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            this.ddlUser.DataValueField = "UserId";
            this.ddlUser.DataTextField = "UserName";
            this.ddlUser.DataBind();

            this.ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            this.ddlRole.DataValueField = "RoleId";
            this.ddlRole.DataTextField = "RoleName";
            this.ddlRole.DataBind();
        }
        private BcUserRoleEntity PrepareFormData()
        {
            this.ddlUser.SelectedValue.InitValidation("用户").NotEmpty();
            this.ddlRole.SelectedValue.InitValidation("角色").NotEmpty();
            var entity = new BcUserRoleEntity();
            entity.UserId = int.Parse(this.ddlUser.SelectedValue);
            entity.RoleId = int.Parse(this.ddlRole.SelectedValue);
            return entity;
        }
    }
}
