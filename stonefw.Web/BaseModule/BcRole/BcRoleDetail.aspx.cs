using System;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcRole
{
    public partial class BcRoleDetail : BasePage
    {
        private BcRoleBiz _biz;

        private BcRoleBiz Biz
        {
            get { return _biz ?? (_biz = new BcRoleBiz()); }
        }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillFormData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                if (hdRoleId.Value == "-1")
                {
                    Biz.AddNewBcRole(entity);
                }
                else
                {
                    Biz.UpdateBcRole(entity);
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void FillFormData()
        {
            try
            {
                hdRoleId.Value = Request["roleid"];
                var entity = Biz.GetSingleBcRole(int.Parse(hdRoleId.Value));
                if (entity != null)
                {
                    hdRoleId.Value = entity.RoleId.ToString();
                    txtRoleName.Text = entity.RoleName;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private BcRoleEntity PrepareFormData()
        {
            //校验参数的合法性
            txtRoleName.Text.InitValidation("角色名称").NotEmpty().ShorterThan(25);

            var entity = new BcRoleEntity();
            entity.RoleId = int.Parse(hdRoleId.Value);
            entity.RoleName = txtRoleName.Text;
            return entity;
        }
    }
}