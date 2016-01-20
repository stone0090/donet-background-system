using System;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcUserRole
{
    public partial class BcUserRoleDetail : BasePage
    {
        private BcUserRoleBiz _biz;

        private BcUserRoleBiz Biz
        {
            get { return _biz ?? (_biz = new BcUserRoleBiz()); }
        }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);
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
                var entity = PrepareFormData();
                var er = Biz.AddNewBcUserRole(entity);
                if (er != ExcuteResultEnum.Success)
                {
                    lMessage.Text = er.GetDescription();
                    return;
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void BindControlData()
        {
            ddlUser.DataSource = new BcUserInfoBiz().GetEnabledBcUserInfoList();
            ddlUser.DataValueField = "UserId";
            ddlUser.DataTextField = "UserName";
            ddlUser.DataBind();

            ddlRole.DataSource = new BcRoleBiz().GetBcRoleList();
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataBind();
        }

        private BcUserRoleEntity PrepareFormData()
        {
            ddlUser.SelectedValue.InitValidation("用户").NotEmpty();
            ddlRole.SelectedValue.InitValidation("角色").NotEmpty();
            var entity = new BcUserRoleEntity();
            entity.UserId = int.Parse(ddlUser.SelectedValue);
            entity.RoleId = int.Parse(ddlRole.SelectedValue);
            return entity;
        }
    }
}