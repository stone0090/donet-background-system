using System;
using Stonefw.Biz.BaseModule;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility;
using Stonefw.Web.Utility.BaseClass;

namespace Stonefw.Web.BaseModule.BcGroup
{
    public partial class BcGroupDetail : BasePage
    {
        private BcGroupBiz _biz;

        private BcGroupBiz Biz
        {
            get { return _biz ?? (_biz = new BcGroupBiz()); }
        }

        protected override bool InitPermission()
        {
            return LoadPermission(SysPermsPointEnum.Add) || LoadPermission(SysPermsPointEnum.Edit);
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
                if (hdGroupId.Value == "-1")
                {
                    Biz.AddNewBcGroup(entity);
                }
                else
                {
                    Biz.UpdateBcGroup(entity);
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
                hdGroupId.Value = Request["groupid"];
                var entity = Biz.GetSingleBcGroup(int.Parse(hdGroupId.Value));
                if (entity != null)
                {
                    hdGroupId.Value = entity.GroupId.ToString();
                    txtGroupName.Text = entity.GroupName;
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message);
            }
        }

        private BcGroupEntity PrepareFormData()
        {
            //校验参数的合法性
            txtGroupName.Text.InitValidation("组别名称").NotEmpty().ShorterThan(25);

            var entity = new BcGroupEntity();
            entity.GroupId = int.Parse(hdGroupId.Value);
            entity.GroupName = txtGroupName.Text;
            return entity;
        }
    }
}