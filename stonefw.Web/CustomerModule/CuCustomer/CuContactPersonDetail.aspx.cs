using System;
using stonefw.Biz.CustomerModule;
using stonefw.Entity.CustomerModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.CustomerModule.CuCustomer
{
    public partial class CuContactPersonDetail : BasePage
    {
        private CuContactPersonBiz _biz;
        private CuContactPersonBiz Biz
        {
            get { return _biz ?? (_biz = new CuContactPersonBiz()); }
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
                CuContactPersonEntity entity = PrepareFormData();
                if (this.hdCpId.Value == "-1")
                {
                    Biz.AddNewCuContactPerson(entity);
                }
                else
                {
                    Biz.UpdateCuContactPerson(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void FillFormData()
        {
            try
            {
                this.hdCpId.Value = Request["cpid"];
                this.hdCuId.Value = Request["cuid"];
                CuContactPersonEntity entity = Biz.GetCuContactPersonEntity(int.Parse(this.hdCpId.Value));
                if (entity != null)
                {
                    this.txtCpName.Text = entity.CpName.ToString();
                    this.txtMobile.Text = entity.Mobile.ToString();
                    this.txtPhone.Text = entity.Phone.ToString();
                    this.rYes.Checked = (bool)entity.IsDefault;
                    this.rNo.Checked = !(bool)entity.IsDefault;
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private CuContactPersonEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new CuContactPersonEntity();
            entity.CuId = this.hdCuId.Value;
            entity.CpId = int.Parse(this.hdCpId.Value);
            entity.CpName = this.txtCpName.Text;
            entity.Mobile = this.txtMobile.Text;
            entity.Phone = this.txtPhone.Text;
            entity.IsDefault = this.rYes.Checked;
            return entity;
        }
    }
}
