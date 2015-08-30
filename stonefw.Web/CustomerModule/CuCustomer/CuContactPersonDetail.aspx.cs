using System;
using Stonefw.Biz.CustomerModule;
using Stonefw.Entity.CustomerModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.CustomerModule.CuCustomer
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
                var entity = PrepareFormData();
                if (hdCpId.Value == "-1")
                {
                    Biz.AddNewCuContactPerson(entity);
                }
                else
                {
                    Biz.UpdateCuContactPerson(entity);
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
                hdCpId.Value = Request["cpid"];
                hdCuId.Value = Request["cuid"];
                var entity = Biz.GetCuContactPersonEntity(int.Parse(hdCpId.Value));
                if (entity != null)
                {
                    txtCpName.Text = entity.CpName;
                    txtMobile.Text = entity.Mobile;
                    txtPhone.Text = entity.Phone;
                    rYes.Checked = (bool) entity.IsDefault;
                    rNo.Checked = !(bool) entity.IsDefault;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private CuContactPersonEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new CuContactPersonEntity();
            entity.CuId = hdCuId.Value;
            entity.CpId = int.Parse(hdCpId.Value);
            entity.CpName = txtCpName.Text;
            entity.Mobile = txtMobile.Text;
            entity.Phone = txtPhone.Text;
            entity.IsDefault = rYes.Checked;
            return entity;
        }
    }
}