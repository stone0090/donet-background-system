using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysModule
{
    public partial class SysModuleDetail : BasePage
    {
        private SysModuleBiz _biz;
        private SysModuleBiz Biz { get { return _biz ?? (_biz = new SysModuleBiz()); } }
        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SysModuleEntity entity = PrepareFormData();
                if (this.hdModuleId.Value == "-1")
                {
                    Biz.AddNewSysModule(entity);
                }
                else
                { Biz.UpdateSysModule(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdModuleId.Value = Request["moduleid"];
                SysModuleEntity entity = Biz.GetSingleSysModule(this.hdModuleId.Value);
                if (entity != null)
                {
                    this.txtModuleId.Enabled = false;
                    this.txtModuleId.Text = entity.ModuleId.ToString();
                    this.txtModuleName.Text = entity.ModuleName.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysModuleEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysModuleEntity();
            entity.ModuleId = this.txtModuleId.Text;
            entity.ModuleName = this.txtModuleName.Text;
            return entity;
        }
    }
}
