using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysPermsPoint
{
    public partial class SysPermsPointDetail : BasePage
    {
        private SysPermsPointBiz _biz;
        private SysPermsPointBiz Biz { get { return _biz ?? (_biz = new SysPermsPointBiz()); } }
        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SysPermsPointEntity entity = PrepareFormData();
                if (this.hdPermsPointId.Value == "-1")
                {
                    Biz.AddNewSysPermsPoint(entity);
                }
                else
                { Biz.UpdateSysPermsPoint(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdPermsPointId.Value = Request["permspointid"];
                SysPermsPointEntity entity = Biz.GetSingleSysPermsPoint(this.hdPermsPointId.Value);
                if (entity != null)
                {
                    this.txtPermsPointId.Enabled = false;
                    this.txtPermsPointId.Text = entity.PermsPointId.ToString();
                    this.txtPermsPointName.Text = entity.PermsPointName.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysPermsPointEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysPermsPointEntity();
            entity.PermsPointId = this.txtPermsPointId.Text;
            entity.PermsPointName = this.txtPermsPointName.Text;
            return entity;
        }
    }
}
