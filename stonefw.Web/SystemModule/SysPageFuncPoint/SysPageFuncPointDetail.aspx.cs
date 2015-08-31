using System;
using System.Web.UI.WebControls;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysPageFuncPoint
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
                var entity = PrepareFormData();
                if (hdPageUrl.Value == "-1")
                {
                    Biz.AddNewSysPageFuncPoint(entity);
                }
                else
                {
                    Biz.UpdateSysPageFuncPoint(entity);
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
            var sysRelationList = new SysRelationBiz().GetSysRelationList();
            foreach (SysRelationEntity entity in sysRelationList)
            {
                ddlFuncPointId.Items.Add(new ListItem(entity.ModuleName + "-" + entity.FuncPointName, entity.FuncPointId));
            }
        }

        private void FillFormData()
        {
            try
            {
                hdPageUrl.Value = Request["pageurl"];
                var entity = Biz.GetSingleSysPageFuncPoint(hdPageUrl.Value);
                if (entity != null)
                {
                    txtPageUrl.Text = entity.PageUrl;
                    ddlFuncPointId.SelectedValue = entity.FuncPointId;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private SysPageFuncPointEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysPageFuncPointEntity();
            entity.PageUrl = txtPageUrl.Text;
            entity.FuncPointId = ddlFuncPointId.SelectedValue;
            return entity;
        }
    }
}