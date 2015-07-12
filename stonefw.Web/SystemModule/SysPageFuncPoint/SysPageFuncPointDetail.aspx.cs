using System;
using stonefw.Biz.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.SystemModule.SysPageFuncPoint
{
    public partial class SysPageFuncPointDetail : BasePage
    {
        private SysPageFuncPointBiz _biz;
        private SysPageFuncPointBiz Biz
        {
            get { return _biz ?? (_biz = new SysPageFuncPointBiz()); }
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
                SysPageFuncPointEntity entity = PrepareFormData();
                if (this.hdPageUrl.Value == "-1")
                {
                    Biz.AddNewSysPageFuncPoint(entity);
                }
                else
                {
                    Biz.UpdateSysPageFuncPoint(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }

        private void BindControlData()
        {
            this.ddlFuncPointId.DataSource = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            this.ddlFuncPointId.DataValueField = "Name";
            this.ddlFuncPointId.DataTextField = "Description";
            this.ddlFuncPointId.DataBind();
        }
        private void FillFormData()
        {
            try
            {
                this.hdPageUrl.Value = Request["pageurl"];
                SysPageFuncPointEntity entity = Biz.GetSingleSysPageFuncPoint(this.hdPageUrl.Value);
                if (entity != null)
                {
                    this.txtPageUrl.Text = entity.PageUrl.ToString();
                    this.ddlFuncPointId.SelectedValue = entity.FuncPointId.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private SysPageFuncPointEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysPageFuncPointEntity();
            entity.PageUrl = this.txtPageUrl.Text;
            entity.FuncPointId = this.ddlFuncPointId.SelectedValue;
            return entity;
        }
    }
}
