using System;
using stonefw.Biz.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Web.Utility.BaseClass;

namespace stonefw.Web.BaseModule.BcLogError
{
    public partial class BcLogErrorDetail : BasePage
    {
        private BcLogErrorBiz _biz;
        private BcLogErrorBiz Biz { get { return _biz ?? (_biz = new BcLogErrorBiz()); } }

        public override bool InitPermission()
        {
            return LoadPermission(PermsPointEnum.View);
        }

        protected void Page_Load(object sender, EventArgs e)
        { if (!IsPostBack) { FillFormData(); } }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BcLogErrorEntity entity = PrepareFormData();
                if (this.txtId.Text == "-1")
                {
                    Biz.AddNewBcLogError(entity);
                }
                else
                { Biz.UpdateBcLogError(entity); }
                base.FatherQuery();
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("保存失败，原因：{0}", ex.Message); }
        }
        private void FillFormData()
        {
            try
            {
                this.txtId.Text = Request["id"];
                BcLogErrorEntity entity = Biz.GetSingleBcLogError(int.Parse(this.txtId.Text));
                if (entity != null)
                {
                    this.txtId.Text = entity.Id.ToString();
                    this.txtUserId.Text = entity.UserId.ToString();
                    this.txtUserName.Text = entity.UserName.ToString();
                    this.txtOpUrl.Text = entity.OpUrl.ToString();
                    this.txtOpTime.Text = entity.OpTime.ToString();
                    this.txtOpHostAddress.Text = entity.OpHostAddress.ToString();
                    this.txtOpHostName.Text = entity.OpHostName.ToString();
                    this.txtOpUserAgent.Text = entity.OpUserAgent.ToString();
                    this.txtOpQueryString.Text = entity.OpQueryString.ToString();
                    this.txtOpHttpMethod.Text = entity.OpHttpMethod.ToString();
                    this.txtMessage.Text = entity.Message.ToString();
                }
            }
            catch (Exception ex) { this.lMessage.Text = string.Format("数据加载失败，原因：{0}", ex.Message); }
        }
        private BcLogErrorEntity PrepareFormData()
        {
            //TODO:需要校验参数的合法性
            var entity = new BcLogErrorEntity();
            entity.Id = int.Parse(this.txtId.Text);
            entity.UserId = int.Parse(this.txtUserId.Text);
            entity.UserName = this.txtUserName.Text;
            entity.OpUrl = this.txtOpUrl.Text;
            entity.OpTime = DateTime.Parse(this.txtOpTime.Text);
            entity.OpHostAddress = this.txtOpHostAddress.Text;
            entity.OpHostName = this.txtOpHostName.Text;
            entity.OpUserAgent = this.txtOpUserAgent.Text;
            entity.OpQueryString = this.txtOpQueryString.Text;
            entity.OpHttpMethod = this.txtOpHttpMethod.Text;
            entity.Message = this.txtMessage.Text;
            return entity;
        }
    }
}
