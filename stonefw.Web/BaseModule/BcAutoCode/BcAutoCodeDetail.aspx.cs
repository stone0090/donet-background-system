using System;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcAutoCode
{
    public partial class BcAutoCodeDetail : BasePage
    {
        private BcAutoCodeBiz _biz;

        private BcAutoCodeBiz Biz
        {
            get { return _biz ?? (_biz = new BcAutoCodeBiz()); }
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
                FillFormData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                if (hdId.Value == "-1")
                {
                    Biz.AddNewBcAutoCode(entity);
                }
                else
                {
                    Biz.UpdateBcAutoCode(entity);
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
            //this.ddlFuncPoint.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList().Where(n => n.IsAutoCode == true).ToList();
            //this.ddlFuncPoint.DataTextField = "Name";
            //this.ddlFuncPoint.DataValueField = "Description";
            //this.ddlFuncPoint.DataBind();
        }

        private void FillFormData()
        {
            try
            {
                hdId.Value = Request["id"];
                var entity = Biz.GetSingleBcAutoCode(int.Parse(hdId.Value));
                if (entity != null)
                {
                    hdId.Value = entity.Id.ToString();
                    txtPrefix.Text = entity.Prefix.ToUpper();
                    txtDateFormat.Text = entity.DateFormat;
                    ddlFuncPoint.SelectedValue = entity.FuncPointId;
                    txtDigit.Text = entity.Digit.ToString();
                    rYes.Checked = (bool) entity.IsDefault;
                    rNo.Checked = !(bool) entity.IsDefault;
                }
                else
                {
                    txtDigit.Text = "4";
                    txtDateFormat.Text = "yyMMdd";
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private BcAutoCodeEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new BcAutoCodeEntity();
            entity.Id = int.Parse(hdId.Value);
            entity.Prefix = txtPrefix.Text.ToUpper();
            entity.DateFormat = txtDateFormat.Text;
            entity.FuncPointId = ddlFuncPoint.SelectedValue;
            entity.Digit = int.Parse(txtDigit.Text);
            entity.IsDefault = rYes.Checked;
            return entity;
        }
    }
}