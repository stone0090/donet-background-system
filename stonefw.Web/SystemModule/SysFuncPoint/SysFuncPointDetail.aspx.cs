using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysFuncPoint
{
    public partial class SysFuncPointDetail : BasePage
    {
        private SysFuncPointBiz _biz;
        private SysFuncPointBiz Biz { get { return _biz ?? (_biz = new SysFuncPointBiz()); } }
        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SysFuncPointEntity entity = PrepareFormData();
                if (this.hdFuncPointId.Value == "-1")
                {
                    Biz.AddNewSysFuncPoint(entity);
                }
                else
                { Biz.UpdateSysFuncPoint(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdFuncPointId.Value = Request["funcpointid"];
                SysFuncPointEntity entity = Biz.GetSingleSysFuncPoint(this.hdFuncPointId.Value);
                if (entity != null)
                {
                    this.txtFuncPointId.Enabled = false;
                    this.txtFuncPointId.Text = entity.FuncPointId.ToString();
                    this.txtFuncPointName.Text = entity.FuncPointName.ToString();
                    this.cbIsAutoCode.Checked = (bool)entity.IsAutoCode;
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysFuncPointEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysFuncPointEntity();
            entity.FuncPointId = this.txtFuncPointId.Text;
            entity.FuncPointName = this.txtFuncPointName.Text;
            entity.IsAutoCode = this.cbIsAutoCode.Checked;
            return entity;
        }
    }
}
