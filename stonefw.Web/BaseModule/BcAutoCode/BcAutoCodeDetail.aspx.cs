using System;
using System.Linq;
using stonefw.Biz.BaseModule;
using stonefw.Biz.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcAutoCode
{
    public partial class BcAutoCodeDetail : BasePage
    {
        private BcAutoCodeBiz _biz;
        private BcAutoCodeBiz Biz { get { return _biz ?? (_biz = new BcAutoCodeBiz()); } }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                FillFormData();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BcAutoCodeEntity entity = PrepareFormData();
                if (this.hdId.Value == "-1")
                { Biz.AddNewBcAutoCode(entity); }
                else
                { Biz.UpdateBcAutoCode(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void BindControlData()
        {
            //this.ddlFuncPoint.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList().Where(n => n.IsAutoCode == true).ToList();
            //this.ddlFuncPoint.DataTextField = "Name";
            //this.ddlFuncPoint.DataValueField = "Description";
            //this.ddlFuncPoint.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                this.hdId.Value = Request["id"];
                BcAutoCodeEntity entity = Biz.GetSingleBcAutoCode(int.Parse(this.hdId.Value));
                if (entity != null)
                {
                    this.hdId.Value = entity.Id.ToString();
                    this.txtPrefix.Text = entity.Prefix.ToUpper().ToString();
                    this.txtDateFormat.Text = entity.DateFormat.ToString();
                    this.ddlFuncPoint.SelectedValue = entity.FuncPointId.ToString();
                    this.txtDigit.Text = entity.Digit.ToString();
                    this.rYes.Checked = (bool)entity.IsDefault;
                    this.rNo.Checked = !(bool)entity.IsDefault;
                }
                else
                {
                    this.txtDigit.Text = "4";
                    this.txtDateFormat.Text = "yyMMdd";
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private BcAutoCodeEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new BcAutoCodeEntity();
            entity.Id = int.Parse(this.hdId.Value);
            entity.Prefix = this.txtPrefix.Text.ToUpper();
            entity.DateFormat = this.txtDateFormat.Text;
            entity.FuncPointId = this.ddlFuncPoint.SelectedValue;
            entity.Digit = int.Parse(this.txtDigit.Text);
            entity.IsDefault = this.rYes.Checked;
            return entity;
        }
    }
}
