

using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using System.Data;
using System.Reflection;
using stonefw.Utility.EntitySql.Attribute;
using System.Text;

namespace stonefw.Biz.SystemModule
{
    public class SysGlobalSettingBiz
    {
        const string CacheKey = "SysSettingBiz-GetSysSettingEntity";


        public SysGlobalSettingEntity GetSysSettingEntity()
        {
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                objModel = GetSysSettingEntity2();
                if (objModel != null)
                {
                    DataCache.SetCache(CacheKey, objModel);
                }
            }
            return objModel != null ? (SysGlobalSettingEntity)objModel : null;
        }
        public ExcuteResultEnum UpdateSysSettingEntity(SysGlobalSettingEntity entity)
        {
            SaveSysSettingEntity(entity);
            DataCache.SetCache(CacheKey, entity);
            return ExcuteResultEnum.Success;
        }

        public bool IsSuperAdmin(string userAccount)
        {
            return GetSysSettingEntity().SuperAdmins.Contains(userAccount);
        }

        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public string GetSysEnumName()
        {
            DataTable dt = Db.ExecuteDataTable(" select * from Sys_GlobalSetting where SysKey = 'SysEnumName' ");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows[0]["SysValue"].ToString();
        }

        public SysGlobalSettingEntity GetSysSettingEntity2()
        {
            DataTable dt = Db.ExecuteDataTable(" select * from Sys_GlobalSetting ");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            SysGlobalSettingEntity entity = new SysGlobalSettingEntity();
            PropertyInfo[] pis = entity.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                object[] attributes = pi.GetCustomAttributes(typeof(Field), false);
                if (attributes.Length == 0)
                    continue;

                Field theAttribute = (Field)attributes[0];
                DataRow[] drs = dt.Select("SysKey='" + theAttribute.FieldName + "'");
                if (drs.Length == 0)
                    continue;

                pi.SetValue(entity, drs[0]["SysValue"], null);
            }

            return entity;
        }

        public void SaveSysSettingEntity(SysGlobalSettingEntity entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" delete from Sys_GlobalSetting ");
            PropertyInfo[] pis = entity.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                object[] attributes = pi.GetCustomAttributes(typeof(Field), false);
                if (attributes.Length == 0)
                    continue;

                Field theAttribute = (Field)attributes[0];
                sb.AppendFormat(" insert into Sys_GlobalSetting values ('{0}','{1}') ", theAttribute.FieldName, pi.GetValue(entity, null));
            }

            Db.ExecuteNonQuery(sb.ToString());
        }

    }
}
