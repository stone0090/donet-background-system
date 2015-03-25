using System;
using System.Web.UI.WebControls;
using stonefw.Biz.BaseModule;
using stonefw.Biz.CustomerModule;
using stonefw.Entity.CustomerModule;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.CustomerModule.CuCustomer
{
    public partial class CuCustomerDetail : BasePage
    {
        private CuCustomerBiz _customerBiz;
        private CuCustomerBiz CustomerBiz
        {
            get { return _customerBiz ?? (_customerBiz = new CuCustomerBiz()); }
        }

        private CuContactPersonBiz _contactPersonBiz;
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
                string[] arg = e.CommandArgument.ToString().Split('|');
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
            this.lMessage.Text = "执行成功！";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CuCustomerEntity entity = PrepareFormData();
                if (this.hdCuId.Value == "-1")
                {
                    entity.CuId = new BcAutoCodeBiz().GetCode(base.CurrentPageFuncPoint.FuncPointId);
                    CustomerBiz.AddNewCuCustomer(entity);
                }
                else
                {
                    CustomerBiz.UpdateCuCustomer(entity);
                }
                base.FatherQuery2();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindData()
        {
            gvCuContactPerson.PageSize = int.Parse(base.SysGlobalSetting.GridViewPageSize);
            gvCuContactPerson.DataSource = ContactPersonBiz.GetCuContactPersonList();
            gvCuContactPerson.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                this.hdCuId.Value = Request["cuid"];
                CuCustomerEntity entity = CustomerBiz.GetCuCustomerEntity(this.hdCuId.Value);
                if (entity != null)
                {
                    this.txtCuId.Text = entity.CuId.ToString();
                    this.txtCuName.Text = entity.CuName.ToString();
                    this.txtDistrict.Text = entity.District.ToString();
                    this.txtAddress.Text = entity.Address.ToString();
                    this.txtRemark.Text = entity.Remark.ToString();
                    this.rEnabled.Checked = (bool)entity.ActivityFlag;
                    this.rDisabled.Checked = !(bool)entity.ActivityFlag;
                }
                else
                {
                    this.btnAddNew.Enabled = false;
                    this.txtCuId.Text = "*后台自动生成*";
                }
                this.txtCuId.Enabled = false;
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private CuCustomerEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            this.txtCuId.Text.InitValidation("编号").NotEmpty().ShorterThan(25);
            this.txtCuName.Text.InitValidation("姓名").NotEmpty().ShorterThan(25);
            this.txtDistrict.Text.InitValidation("地区").NotEmpty().ShorterThan(25);
            this.txtAddress.Text.InitValidation("地址").ShorterThan(250);
            this.txtRemark.Text.InitValidation("备注").ShorterThan(1000);

            var entity = new CuCustomerEntity();
            entity.CuId = this.txtCuId.Text;
            entity.CuName = this.txtCuName.Text;
            entity.District = this.txtDistrict.Text;
            entity.Address = this.txtAddress.Text;
            entity.Remark = this.txtRemark.Text;
            entity.ActivityFlag = this.rEnabled.Checked;
            return entity;
        }
    }
}
