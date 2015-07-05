using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;

namespace stonefw.Biz.SystemModule
{
    public class SysGlobalSettingBiz
    {
        const string CacheKey = "SysSettingBiz-GetSysSettingEntity";

        private SysGlobalSettingDao _dao;
        private SysGlobalSettingDao Dao
        {
            get { return _dao ?? (_dao = new SysGlobalSettingDao()); }
        }

        public SysGlobalSettingEntity GetSysSettingEntity()
        {

            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                objModel = Dao.GetSysSettingEntity();
                if (objModel != null)
                {
                    DataCache.SetCache(CacheKey, objModel);
                }
            }
            return objModel != null ? (SysGlobalSettingEntity)objModel : null;
        }
        public ExcuteResultEnum UpdateSysSettingEntity(SysGlobalSettingEntity entity)
        {
            Dao.SaveSysSettingEntity(entity);
            DataCache.SetCache(CacheKey, entity);
            return ExcuteResultEnum.Success;
        }

        public bool IsSuperAdmin(string userAccount)
        {
            return GetSysSettingEntity().SuperAdmins.Contains(userAccount);
        }

    }
}
