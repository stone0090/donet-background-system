using System;
using stonefw.Biz.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcRole
{
    public partial class BcRoleDetail : BasePage
    {
        private BcRoleBiz _biz;
        private BcRoleBiz Biz { get { return _biz ?? (_biz = new BcRoleBiz()); } }

        public override bool InitPermission()
        {
            return LoadPermission(PermsPointEnum.Add) || LoadPermission(PermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BcRoleEntity entity = PrepareFormData();
                if (this.hdRoleId.Value == "-1")
                {
                    Biz.AddNewBcRole(entity);
                }
                else
                {
                    Biz.UpdateBcRole(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdRoleId.Value = Request["roleid"];
                BcRoleEntity entity = Biz.GetSingleBcRole(int.Parse(this.hdRoleId.Value));
                if (entity != null)
                {
                    this.hdRoleId.Value = entity.RoleId.ToString();
                    this.txtRoleName.Text = entity.RoleName.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private BcRoleEntity PrepareFormData()
        {
            //校验参数的合法性
            this.txtRoleName.Text.InitValidation("角色名称").NotEmpty().ShorterThan(25);

            var entity = new BcRoleEntity();
            entity.RoleId = int.Parse(this.hdRoleId.Value);
            entity.RoleName = this.txtRoleName.Text;
            return entity;
        }
    }
}
