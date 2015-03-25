using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysEnumName
{
    public partial class SysEnumNameDetail : BasePage
    {
        private SysEnumNameBiz _biz;
        private SysEnumNameBiz Biz { get { return _biz ?? (_biz = new SysEnumNameBiz()); } }
        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SysEnumNameEntity entity = PrepareFormData();
                if (this.hdType.Value == "-1" && this.hdValue.Value == "-1")
                {
                    Biz.AddNewSysEnumName(entity);
                }
                else
                { Biz.UpdateSysEnumName(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdType.Value = Request["type"];
                this.hdValue.Value = Request["value"];
                SysEnumNameEntity entity = Biz.GetSingleSysEnumName(this.hdType.Value, this.hdValue.Value);
                if (entity != null)
                {
                    this.txtType.Text = entity.Type.ToString();
                    this.txtValue.Text = entity.Value.ToString();
                    this.txtName.Text = entity.Name.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysEnumNameEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysEnumNameEntity();
            entity.Type = this.txtType.Text;
            entity.Value = this.txtValue.Text;
            entity.Name = this.txtName.Text;
            return entity;
        }
    }
}
