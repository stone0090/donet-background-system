using System;
using Stonefw.Biz.SystemModule;
using Stonefw.Entity.SystemModule;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.SystemModule.SysEnumName
{
    public partial class SysEnumNameDetail : BasePage
    {
        private SysEnumNameBiz _biz;

        private SysEnumNameBiz Biz
        {
            get { return _biz ?? (_biz = new SysEnumNameBiz()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillFormData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = PrepareFormData();
                if (hdType.Value == "-1" && hdValue.Value == "-1")
                {
                    Biz.AddNewSysEnumName(entity);
                }
                else
                {
                    Biz.UpdateSysEnumName(entity);
                }
                FatherQuery();
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message);
            }
        }

        private void FillFormData()
        {
            try
            {
                hdType.Value = Request["type"];
                hdValue.Value = Request["value"];
                var entity = Biz.GetSingleSysEnumName(hdType.Value, hdValue.Value);
                if (entity != null)
                {
                    txtType.Text = entity.Type;
                    txtValue.Text = entity.Value;
                    txtName.Text = entity.Name;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private SysEnumNameEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new SysEnumNameEntity();
            entity.Type = txtType.Text;
            entity.Value = txtValue.Text;
            entity.Name = txtName.Text;
            return entity;
        }
    }
}