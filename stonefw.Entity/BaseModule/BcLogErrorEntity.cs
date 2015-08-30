using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_LogError")]
    public partial class BcLogErrorEntity : BaseEntity
    {
        [Field("Id")]
        public int? Id { get; set; }

        [Field("UserId")]
        public int? UserId { get; set; }

        [Field("UserName")]
        public string UserName { get; set; }

        [Field("OpUrl")]
        public string OpUrl { get; set; }

        [Field("OpTime")]
        public DateTime? OpTime { get; set; }

        [Field("OpHostAddress")]
        public string OpHostAddress { get; set; }

        [Field("OpHostName")]
        public string OpHostName { get; set; }

        [Field("OpUserAgent")]
        public string OpUserAgent { get; set; }

        [Field("OpQueryString")]
        public string OpQueryString { get; set; }

        [Field("OpHttpMethod")]
        public string OpHttpMethod { get; set; }

        [Field("Message")]
        public string Message { get; set; }
    }
}