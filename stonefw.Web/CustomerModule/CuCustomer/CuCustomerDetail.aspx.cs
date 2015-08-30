using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.BaseModule;
using Stonefw.Biz.CustomerModule;
using Stonefw.Entity.CustomerModule;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.CustomerModule.CuCustomer
{
    public partial class CuCustomerDetail : BasePage
    {
        private CuContactPersonBiz _contactPersonBiz;
        private CuCustomerBiz _customerBiz;

        private CuCustomerBiz CustomerBiz
        {
            get { return _customerBiz ?? (_customerBiz = new CuCustomerBiz()); }
        }

        private CuContactPersonBiz ContactPersonBiz
        {
            get { return _contactPersonBiz ?? (_contactPersonBiz = new CuContactPersonBiz()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                FillFormData();
            }
        }

        protected void gvCuContactPerson_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Row_Delete")
            {
                var arg = e.CommandArgument.ToString().Split('|');
                ContactPersonBiz.DeleteCuContactPerson(int.Parse(arg[0]));
                BindData();
            }
        }

        protected void gvCuContactPerson_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvCuContactPerson_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.gvCuContactPerson.PageIndex = e.NewPageIndex;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
            lMessage.Text = "执行成功！";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                if (hdCuId.Value == "-1")
                {
                    entity.CuId = new BcAutoCodeBiz().GetCode(CurrentPageFuncPoint.FuncPointId);
                    CustomerBiz.AddNewCuCustomer(entity);
                }
                else
                {
                    CustomerBiz.UpdateCuCustomer(entity);
                }
                FatherQuery2();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void BindData()
        {
            gvCuContactPerson.PageSize = int.Parse(SysGlobalSetting.GridViewPageSize);
            gvCuContactPerson.DataSource = ContactPersonBiz.GetCuContactPersonList();
            gvCuContactPerson.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                hdCuId.Value = Request["cuid"];
                var entity = CustomerBiz.GetCuCustomerEntity(hdCuId.Value);
                if (entity != null)
                {
                    txtCuId.Text = entity.CuId;
                    txtCuName.Text = entity.CuName;
                    txtDistrict.Text = entity.District;
                    txtAddress.Text = entity.Address;
                    txtRemark.Text = entity.Remark;
                    rEnabled.Checked = (bool) entity.ActivityFlag;
                    rDisabled.Checked = !(bool) entity.ActivityFlag;
                }
                else
                {
                    btnAddNew.Enabled = false;
                    txtCuId.Text = "*后台自动生成*";
                }
                txtCuId.Enabled = false;
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private CuCustomerEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            txtCuId.Text.InitValidation("编号").NotEmpty().ShorterThan(25);
            txtCuName.Text.InitValidation("姓名").NotEmpty().ShorterThan(25);
            txtDistrict.Text.InitValidation("地区").NotEmpty().ShorterThan(25);
            txtAddress.Text.InitValidation("地址").ShorterThan(250);
            txtRemark.Text.InitValidation("备注").ShorterThan(1000);

            var entity = new CuCustomerEntity();
            entity.CuId = txtCuId.Text;
            entity.CuName = txtCuName.Text;
            entity.District = txtDistrict.Text;
            entity.Address = txtAddress.Text;
            entity.Remark = txtRemark.Text;
            entity.ActivityFlag = rEnabled.Checked;
            return entity;
        }
    }
}