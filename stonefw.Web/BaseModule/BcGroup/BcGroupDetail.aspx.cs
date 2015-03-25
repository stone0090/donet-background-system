using System;
using stonefw.Biz.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Utility;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcGroup
{
    public partial class BcGroupDetail : BasePage
    {
        private BcGroupBiz _biz;
        private BcGroupBiz Biz { get { return _biz ?? (_biz = new BcGroupBiz()); } }

        public override bool InitPermission()
        {
            return LoadPermission(PermsPointEnum.Add) || LoadPermission(PermsPointEnum.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BcGroupEntity entity = PrepareFormData();
                if (this.hdGroupId.Value == "-1")
                {
                    Biz.AddNewBcGroup(entity);
                }
                else
                {
                    Biz.UpdateBcGroup(entity);
                }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.hdGroupId.Value = Request["groupid"];
                BcGroupEntity entity = Biz.GetSingleBcGroup(int.Parse(this.hdGroupId.Value));
                if (entity != null)
                {
                    this.hdGroupId.Value = entity.GroupId.ToString();
                    this.txtGroupName.Text = entity.GroupName.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private BcGroupEntity PrepareFormData()
        {
            //校验参数的合法性
            this.txtGroupName.Text.InitValidation("组别名称").NotEmpty().ShorterThan(25);

            var entity = new BcGroupEntity();
            entity.GroupId = int.Parse(this.hdGroupId.Value);
            entity.GroupName = this.txtGroupName.Text;
            return entity;
        }
    }
}
